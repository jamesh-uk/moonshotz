using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SideTerrain : MonoBehaviour
{
	public const int MIN = 1;
	public const int MAX = 30;
	public const int LENGTH = 100;
	public const int SCAN_RADIUS = 2;

	float[] heightmap = new float[LENGTH];
   
	public MeshFilter meshFilter;
	public Mesh mesh;

	void Awake()
	{
		mesh = new Mesh();
		mesh.name = "Terrain mesh";
		meshFilter.mesh = mesh;
        
		for (int i = 0; i < LENGTH; i++)
		{
			heightmap[i] = UnityEngine.Random.Range(MIN, MAX);
		}
        
        
		for (int i = 0; i < 3; i++ )
		{
			Smooth();
		}
		
		mesh.Clear();
		
		List<float> heightMapList = new List<float>();
		
		float teeHeight = heightmap[0];
		
		SetBallPosition(8, teeHeight);
		
		float greenHeight = heightmap[LENGTH -1];
		
		heightMapList.AddRange(Enumerable.Repeat(teeHeight, 10));
		
		heightMapList.AddRange(heightmap);
		
		int holePos = 9;
		
		heightMapList.AddRange(Enumerable.Repeat(greenHeight, holePos));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight-2, 2));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight, 18-holePos));
		heightMapList.AddRange(Enumerable.Repeat(greenHeight+3, 3));
        
		List<Vector3> positions = BuildPositions(heightMapList.ToArray());
		List<int> triangles = BuildTriangles(heightMapList.ToArray());
        
		mesh.vertices = positions.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
		
		CreatePolygonCollider(positions.ToArray(), triangles.ToArray());
	}
	
	void SetBallPosition(int x, float teeHeight) {
		GameObject ball = GameObject.Find("Ball");
		ball.transform.position = new Vector3(x,teeHeight+0.505f,0);
		
		GameObject.Find("Main Camera").transform.position = new Vector3(x,25-ball.transform.position.y,-50);
	}
    
	void Update ()
	{
	}
    
	void Smooth()
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
