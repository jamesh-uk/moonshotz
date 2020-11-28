using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    GameObject ball = GameObject.Find("Ball");
	    	    
	    Camera.main.orthographicSize = Mathf.Max((ball.transform.position.y/2) + 5f, 25f);
		
	    transform.position = new Vector3(ball.transform.position.x,Camera.main.orthographicSize,-50);
    }
}
