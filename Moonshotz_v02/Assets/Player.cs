using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject ballObject;
	public GameObject powerbarPreObject;
	public GameObject statusTextObject;
	
	private PowerbarPre _powerbarPre = null;
	
	private PowerbarPre powerbarPre
	{
		get
		{
			if(_powerbarPre == null) {
				_powerbarPre = powerbarPreObject.GetComponent<PowerbarPre>();
			}
			return _powerbarPre;
		}
	}
	
	public GameObject anglebarPreObject;
	
	private AnglebarPre _anglebarPre = null;
	
	private AnglebarPre anglebarPre
	{
		get
		{
			if(_anglebarPre == null) {
				_anglebarPre = anglebarPreObject.GetComponent<AnglebarPre>();
			}
			return _anglebarPre;
		}
	}
	
	private Ball _ball = null;
	
	private Ball ball
	{
		get
		{
			if(_ball == null) {
				_ball = ballObject.GetComponent<Ball>();
			}
			return _ball;
		}
	}
	
	private StatusText _statusText = null;
	
	private StatusText statusText
	{
		get
		{
			if(_statusText == null) {
				_statusText = statusTextObject.GetComponent<StatusText>();
			}
			return _statusText;
		}
	}
	
	public enum PlayerState {
		CountDown,
		PowerBar,
		AngleBar,
		BarsFinished,
		SwingClub,
		BallMoving,
		BallStopped,
		BallInHole,
		BallOutOfBounds,
		PlayerDead,
		PlayerNextLevel
	}
	
	private PlayerState playerState = PlayerState.PowerBar;
	
	private int shotCount = 0;
	
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
	    if(playerState == PlayerState.PowerBar) {
	    	if(powerbarPre.GetBarState() == PowerbarPre.PowerbarState.Stopped) {
		    	playerState = PlayerState.AngleBar;
		    	anglebarPre.StartBar();
		    }
	    } else if(playerState == PlayerState.AngleBar) {
	    	if(anglebarPre.GetBarState() == AnglebarPre.AnglebarState.Stopped) {
		    	playerState = PlayerState.BarsFinished;
		    }
	    } else if(playerState == PlayerState.BallMoving) {
	    	if(ball.isBallStopped()) {
	    		playerState = PlayerState.BallStopped;
	    	}
	    }
    }
    
	public void SetTeePosition(Vector2 tee) {
		ballObject.transform.position = new Vector3(tee.x,tee.y+0.505f,0);
	}
    
	public float getBallX() {
		return ball.getBallX();
	}
	
	public float getBallY() {
		return ball.getBallY();
	}
	
	public void playerClick() {
		if(playerState == PlayerState.PowerBar) {
			powerbarPre.ClickBar();
			playerState = PlayerState.AngleBar;
			anglebarPre.StartBar();
		} else if(playerState == PlayerState.AngleBar) {
			anglebarPre.ClickBar();
			playerState = PlayerState.BarsFinished;
		}
	}
	
	public void StartBar() {
		playerState = PlayerState.PowerBar;
		powerbarPre.StartBar();
		statusText.TakeShot();
	}
	
	public void StartHit() {
		playerState = PlayerState.BallMoving;
		statusText.Hide();
		
		float angle = 180 * anglebarPre.GetPercent();
		float power = 100 * powerbarPre.GetPercent();
		float radAngle = (angle*Mathf.PI)/180f;
		
		shotCount++;
		
		ball.hit(power,radAngle);
	}
	
	public PlayerState GetPlayerState() {
		return playerState;
	}
	
	public void SetBallInHole() {
		playerState = PlayerState.BallInHole;
		statusText.InTheHole();
	}
	
	public void SetBallOutOfBounds() {
		playerState = PlayerState.BallOutOfBounds;
		statusText.OutOfBounds();
	}
}
