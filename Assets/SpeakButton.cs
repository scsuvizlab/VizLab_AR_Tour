using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakButton : MonoBehaviour {

    public TourGuide tourGuide;

	// Use this for initialization
	void Start () {
		
	}
	

    public void Speak()
    {
        tourGuide.Talk();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
