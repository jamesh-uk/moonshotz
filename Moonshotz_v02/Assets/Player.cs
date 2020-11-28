using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject ball;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public float getBallX() {
		return ball.transform.position.x;
	}
	
	public float getBallY() {
		return ball.transform.position.y;
	}
	
	public void playerClick() {
		int angle = 60;
		float power = 50;
		float radAngle = (angle*Mathf.PI)/180f;

		ball.GetComponent<Rigidbody2D>().velocity = new Vector2(power*Mathf.Cos(radAngle),power*Mathf.Sin(radAngle));
	}
}
