using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusText : MonoBehaviour
{
	private TextMeshProUGUI _textMesh = null;
	
	private TextMeshProUGUI textMesh
	{
		get
		{
			if(_textMesh == null) {
				_textMesh = GetComponent<TextMeshProUGUI>();
			}
			return _textMesh;
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
    
	public void Hide() {
		textMesh.enabled = false;
	}
	
	public void TakeShot() {
		textMesh.enabled = true;
		textMesh.SetText("Hit The Ball !!!");
	}
	
	public void OutOfBounds() {
		textMesh.enabled = true;
		textMesh.SetText("Out Of Bounds");
	}
	
	public void InTheHole() {
		textMesh.enabled = true;
		textMesh.SetText("In The Hole !!!");
	}
}
