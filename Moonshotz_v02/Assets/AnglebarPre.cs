﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnglebarPre : MonoBehaviour
{
	private const float BAR_SPEED = 1f;
	private float dx = BAR_SPEED;
	public GameObject anglebar;
	
	public enum AnglebarState {
		Running,
		Stopped
	}
	
	private AnglebarState state = AnglebarState.Stopped;
	private Image _angleBarImage = null;
	
	private Image angleBarImage
	{
		get
		{
			if(_angleBarImage == null) {
				_angleBarImage = anglebar.GetComponent<Image>();
			}
			return _angleBarImage;
		}
	}
	
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if(state == AnglebarState.Running) {
			
			angleBarImage.fillAmount += dx * Time.deltaTime;
			
			if(angleBarImage.fillAmount >= 1) {
				angleBarImage.fillAmount = 1;
				dx = -dx;
			}
			
			if(angleBarImage.fillAmount <= 0) {
				angleBarImage.fillAmount = 0;
				state = AnglebarState.Stopped;
			}
		}
	}
	
	public void StartBar() {
		dx = BAR_SPEED;
		angleBarImage.fillAmount = 0;
		state = AnglebarState.Running;
	}
	
	public float ClickBar() {
		state = AnglebarState.Stopped;
		return angleBarImage.fillAmount;
	}
	
	public AnglebarState GetBarState() {
		return state;
	}
	
	public float GetPercent() {
		return angleBarImage.fillAmount;
	}
}
