using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Slider : MonoBehaviour {

    public Camera FOVCam;
    public UnityEngine.UI.Text outputText;
	// Use this for initialization
	void Start () {
		
	}
	
    public void SetFOV( float value)
    {
        FOVCam.fieldOfView = value;
        outputText.text = "FOV:  " + value.ToString();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
