using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
	public GameObject player1Object;
	private Player _player1 = null;
	
	private Player player1
	{
		get
		{
			if(_player1 == null) {
				_player1 = player1Object.GetComponent<Player>();
			}
			return _player1;
		}
	}
	
	
	public GameObject sideTerrainObject;
	
	private SideTerrain _sideTerrain = null;
	
	private SideTerrain sideTerrain
	{
		get
		{
			if(_sideTerrain == null) {
				_sideTerrain = sideTerrainObject.GetComponent<SideTerrain>();
			}
			return _sideTerrain;
		}
	}
	
	public GameObject holeTextObject;
	
	private HoleText _holeText = null;
	
	private HoleText holeText
	{
		get
		{
			if(_holeText == null) {
				_holeText = holeTextObject.GetComponent<HoleText>();
			}
			return _holeText;
		}
	}
	
	private int level = 1;
	
    // Start is called before the first frame update
    void Start()
	{
		StartLevel(level);
        
	}
    
	void StartLevel(int level) {
		sideTerrain.SetLevel(level);
		holeText.SetLevel(level);
		Vector2 teePosition = sideTerrain.GetTeePosition();
		
		player1.SetLevel(teePosition, level);
		
		player1.StartBar();
	}

    // Update is called once per frame
    void Update()
	{
		Player player1 = player1Object.GetComponent<Player>();
	    	    
		Camera.main.orthographicSize = Mathf.Max((player1.getBallY()/2) + 5f, 25f);
		
		transform.position = new Vector3(player1.getBallX(),Camera.main.orthographicSize,-50);
		
		if (Input.GetMouseButtonDown(0)) {
			player1.playerClick();
		}
		
		if(player1.GetPlayerState() == Player.PlayerState.BarsFinished) {
			player1.StartHit();
		} else if(player1.GetPlayerState() == Player.PlayerState.BallStopped) {
			if(sideTerrain.IsXInHole(player1.getBallX())) {
				player1.SetBallInHole();
			} else {
				player1.StartBar();
			}
		} else if(player1.GetPlayerState() == Player.PlayerState.BallMoving) {
			if(sideTerrain.IsXOnTerrain(player1.getBallX())) {
				player1.SetBallOutOfBounds();
			}
		} else if(player1.GetPlayerState() == Player.PlayerState.PlayerNextLevel) {
			level++;
			if(level > 9) {
				StartLevel(1);
			} else {
				StartLevel(level);
			}
		} else if(player1.GetPlayerState() == Player.PlayerState.PlayerDead) {
			StartLevel(1);
		}
    }
}
