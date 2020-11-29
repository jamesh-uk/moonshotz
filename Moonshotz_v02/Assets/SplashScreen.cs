using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    StartCoroutine(WaitForNextScene());
    }
	
	IEnumerator WaitForNextScene()
	{
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(1);
	}
}
