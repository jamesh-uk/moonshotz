using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
	
    // Start is called before the first frame update
    void Start()
    {
	    Animator animator = GetComponent<Animator>();
	    animator.StopPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void hide() {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.enabled = false;
	}
	
	public void show() {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.enabled = true;
	}
	
	public bool isShown() {
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		return renderer.enabled;
	}
	
	public void animate() {
		Animator animator = GetComponent<Animator>();
		animator.Play("astronaught");
	}
	
	public void stopAnim() {
		Animator animator = GetComponent<Animator>();
		animator.Play("astronaughtStand");
	}
}
