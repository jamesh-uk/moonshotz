using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
	public GameObject player1Object;
	public Player player1;
	
    // Start is called before the first frame update
    void Start()
	{
		player1 = player1Object.GetComponent<Player>();
		
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
			player1.StartBar();
		}
    }
}
