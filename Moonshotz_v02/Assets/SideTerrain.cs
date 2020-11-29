using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SideTerrain : MonoBehaviour
{
	public const int MIN = 1;
	public const int MAX = 30;
	public const int SCAN_RADIUS = 2;
	
	public const int HOLE_WIDTH = 2;
	public const int GREEN_WIDTH = 18;
	private int holeStart = 9;
   
	public MeshFilter meshFilter;
	public Mesh mesh;
	
	private int holeXStart = -1;
	
	private Vector2 teePosition;
	
	private int terrainLength = -1;
	
	public void SetLevel(int level) {
		
		mesh = new Mesh();
		mesh.name = "Terrain mesh";
		meshFilter.mesh = mesh;
		
		float[] heightmap = null;
		
		if(level > 0 && level < 8) {
			
			if(level == 1) {
				heightmap = SetRandomHeights(1,30,50);
			} else if (level == 2) {
				heightmap = SetRandomHeights(1,30,100);
			} else if (level == 3) {
				heightmap = SetRandomHeights(1,50,100);
			} else if (level == 4) {
				heightmap = SetRandomHeights(1,30,100);
				heightmap[0] = 200;
			} else if (level == 5) {
				heightmap = SetRandomHeights(1,30,100);
				heightmap[heightmap.Length-1] = 150;
			} else if (level == 6) {
				heightmap = SetRandomHeights(10,30,100);
				heightmap[0] = 200;
				heightmap[heightmap.Length-1] = 150;
			} else if (level == 7) {
				heightmap = SetRandomHeights(10,30,100);
			}
			
			for (int i = 0; i < 3; i++ )
			{
				Smooth(heightmap);
			}
			
			if (level == 7) {
				heightmap[0] = 1;
				heightmap[heightmap.Length-1] = 1;
			}
		} else if (level == 8) {
			heightmap = SetSlopeHeights(10,50,30);
		}  else if (level == 9) {
			heightmap = SetSlopeHeights(30,10,40);
			heightmap[heightmap.Length-1] = 1;
			holeStart = 3;
		}
		
		mesh.Clear();
		
		List<float> heightMapList = new List<float>();
		
		float teeHeight = heightmap[0];
		
		teePosition = new Vector2(8, teeHeight);
		
		float greenHeight = heightmap[heightmap.Length -1];
		
		heightMapList.AddRange(Enumerable.Repeat(teeHeight, 10));
		
		heightMapList.AddRange(heightmap);
		
		this.holeXStart = heightMapList.Count + holeStart;
		
		heightMapList.AddRange(Enumerable.Repeat(greenHeight, holeStart));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight-2, HOLE_WIDTH));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight, GREEN_WIDTH-holeStart));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight+3, 3));
        
		List<Vector3> positions = BuildPositions(heightMapList.ToArray());
		List<int> triangles = BuildTriangles(heightMapList.ToArray());
        
		mesh.vertices = positions.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		
		terrainLength = heightMapList.Count;
		
		CreatePolygonCollider(positions.ToArray(), triangles.ToArray());
	}
	
	float[] SetRandomHeights(int min, int max, int length) {
		float[] heightmap = new float[length];
		for (int i = 0; i < length; i++)
		{
			heightmap[i] = UnityEngine.Random.Range(min, max);
		}
		return heightmap;
	}
	
	float[] SetSlopeHeights(int start, int end, int length) {
		float[] heightmap = new float[length];
		float delta = ((float)end -(float)start) /(float) length;
		for (int i = 0; i < length; i++)
		{
			heightmap[i] = start + i * delta;
		}
		return heightmap;
	}
	
	public Vector2 GetTeePosition() {
		return this.teePosition;
	}
	
	public bool IsXInHole(float x) {
		return (x >= holeXStart - 1) && (x <= holeXStart + HOLE_WIDTH + 1);
	}
	
	public bool IsXOnTerrain(float x) {
		return x < 0 || x >= terrainLength; 
	}
    
	void Update ()
	{
	}
    
	void Smooth(float[] heightmap)
	{
		for (int i = 0; i < heightmap.Length; i++)
		{
			float height = heightmap[i];
            
			float heightSum = 0;
			float heightCount = 0;
            
			for (int n = i - SCAN_RADIUS;
				n < i + SCAN_RADIUS + 1;
				n++)
			{
				if (n >= 0 &&
					n < heightmap.Length)
				{
					float heightOfNeighbour = heightmap[n];

					heightSum += heightOfNeighbour;
					heightCount++;
				}
			}

			float heightAverage = heightSum / heightCount;
			heightmap[i] = heightAverage;
		}
	}
	
	List<Vector3> BuildPositions(float[] heights) {
		List<Vector3> positions = new List<Vector3>();
		
		for (int i = 0; i < heights.Count(); i++)
		{
			positions.Add(new Vector3(i,0,0));
			positions.Add(new Vector3(i,heights[i],0));
		}
        
		return positions;
	}
	
	List<int> BuildTriangles(float[] heights) {
		List<int> triangles = new List<int>();
		
		int offset = 0;
		for (int i = 0; i < heights.Count() - 1; i++)
		{
			triangles.Add(offset+0);
			triangles.Add(offset+1);
			triangles.Add(offset+2);
            
			triangles.Add(offset+1);
			triangles.Add(offset+3);
			triangles.Add(offset+2);
			
			offset+=2;
		}
        
		return triangles;
	}
	
	void CreatePolygonCollider(Vector3[] vertices, int[] triangles)
	{
		PolygonCollider2D oldCollider = gameObject.GetComponent<PolygonCollider2D>();
		// Remove a collider if it already exists. 
		if(oldCollider != null) {
			Destroy(oldCollider);
		}
		
		// Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
		Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
		for (int i = 0; i < triangles.Length; i += 3) {
			for (int e = 0; e < 3; e++) {
				int vert1 = triangles[i + e];
				int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
				string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
				if (edges.ContainsKey(edge)) {
					edges.Remove(edge);
				} else {
					edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
				}
			}
		}

		// Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
		Dictionary<int, int> lookup = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> edge in edges.Values) {
			if (lookup.ContainsKey(edge.Key) == false) {
				lookup.Add(edge.Key, edge.Value);
			}
		}

		// Create empty polygon collider
		PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
		polygonCollider.pathCount = 0;

		// Loop through edge vertices in order
		int startVert = 0;
		int nextVert = startVert;
		int highestVert = startVert;
		List<Vector2> colliderPath = new List<Vector2>();
		while (true) {

			// Add vertex to collider path
			colliderPath.Add(vertices[nextVert]);

			// Get next vertex
			nextVert = lookup[nextVert];

			// Store highest vertex (to know what shape to move to next)
			if (nextVert > highestVert) {
				highestVert = nextVert;
			}

			// Shape complete
			if (nextVert == startVert) {

				// Add path to polygon collider
				polygonCollider.pathCount++;
				polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
				colliderPath.Clear();

				// Go to next shape if one exists
				if (lookup.ContainsKey(highestVert + 1)) {

					// Set starting and next vertices
					startVert = highestVert + 1;
					nextVert = startVert;

					// Continue to next loop
					continue;
				}

				// No more verts
				break;
			}
		}
	}
}
