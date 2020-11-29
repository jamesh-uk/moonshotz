using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoleText : MonoBehaviour
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
    
	public void SetLevel(int level) {
		textMesh.SetText("Hole " + level);
	}
}
