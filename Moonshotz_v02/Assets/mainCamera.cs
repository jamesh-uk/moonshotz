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
	
    // Start is called before the first frame update
    void Start()
	{
		Vector2 teePosition = sideTerrain.GetTeePosition();
		
		player1.SetTeePosition(teePosition);
		
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
			
		}
    }
}
