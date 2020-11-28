using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private Rigidbody2D _ballBody = null;
	
	private Rigidbody2D ballBody
	{
		get
		{
			if(_ballBody == null) {
				_ballBody = this.GetComponent<Rigidbody2D>();
			}
			return _ballBody;
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public float getBallX() {
		return this.transform.position.x;
	}
	
	public float getBallY() {
		return this.transform.position.y;
	}
	
	public void hit(float power, float radAngle) {
		ballBody.velocity = new Vector2(power*Mathf.Cos(radAngle),power*Mathf.Sin(radAngle));
	}
    
	public bool isBallStopped() {
		return ballBody.velocity.x == 0 && ballBody.velocity.y == 0 && ballBody.angularVelocity == 0;
	}
}
