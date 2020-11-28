using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickBehaviourScript : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
        
	}
	
	// Back ground image:
	// https://www.rawpixel.com/image/1207204/moon-landing-photograph
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			int angle = 60;
			float power = 50;
			float radAngle = (angle*Mathf.PI)/180f;

			this.GetComponent<Rigidbody2D>().velocity = new Vector2(power*Mathf.Cos(radAngle),power*Mathf.Sin(radAngle));
		}
	}
}