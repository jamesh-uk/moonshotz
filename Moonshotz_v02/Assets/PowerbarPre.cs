using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerbarPre : MonoBehaviour
{
	private const float BAR_SPEED = 1f;
	private float dx = BAR_SPEED;
	public GameObject powerbar;
	
	public enum PowerbarState {
		Running,
		Stopped
	}
	
	private PowerbarState state = PowerbarState.Stopped;
	private Image _powerBarImage = null;
	
	private Image powerBarImage
	{
		get
		{
			if(_powerBarImage == null) {
				_powerBarImage = powerbar.GetComponent<Image>();
			}
			return _powerBarImage;
		}
	}
	
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if(state == PowerbarState.Running) {
			powerBarImage.fillAmount += dx * Time.deltaTime;
			
			if(powerBarImage.fillAmount >= 1) {
				powerBarImage.fillAmount = 1;
				dx = -dx;
			}
			
			if(powerBarImage.fillAmount <= 0) {
				powerBarImage.fillAmount = 0;
				state = PowerbarState.Stopped;
			}
		}
	}
	
	public void StartBar() {
		dx = BAR_SPEED;
		powerBarImage.fillAmount = 0;
		state = PowerbarState.Running;
	}
	
	public float ClickBar() {
		state = PowerbarState.Stopped;
		return powerBarImage.fillAmount;
	}
	
	public PowerbarState GetBarState() {
		return state;
	}
	
	public float GetPercent() {
		return powerBarImage.fillAmount;
	}
}
