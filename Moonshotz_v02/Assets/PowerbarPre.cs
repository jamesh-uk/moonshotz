using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarPre : MonoBehaviour
{
	private float dx = 0.005f;
	
	// Start is called before the first frame update
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
		GameObject powerbar = GameObject.Find("Powerbar");
	    
		Image powerBarImage = powerbar.GetComponent<Image>();
		powerBarImage.fillAmount += dx;
		
		if(powerBarImage.fillAmount >= 1) {
			powerBarImage.fillAmount = 1;
			dx = -dx;
		}
		
		if(powerBarImage.fillAmount <= 0) {
			powerBarImage.fillAmount = 0;
			dx = -dx;
		}
	}
}
