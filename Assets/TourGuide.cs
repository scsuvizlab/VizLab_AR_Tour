using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.GuidedAR;
using UnityEngine.UI;


public class TourGuide : MonoBehaviour {

    public Transform startPoint;
    public GuidedARController ARController;
    GuidedARController.PlaceState placeState = GuidedARController.PlaceState.PLACE_ANCHOR;
    GuidedARController.PlaceState previousPlaceState = GuidedARController.PlaceState.PLACE_ANCHOR;
    public Camera playerCamera;
    float agility = 20.0f;
    public Text outputText;
    AudioSource audioSource;
    public AudioClip[] dialogues;
    public AudioClip[] generalPhrases;
    int audioIndex = 0;

    public enum TourGuideStates { IDLE,ROAMING,FOLLOWING,SPEAKING};
    TourGuideStates guideState = TourGuideStates.IDLE;

    Animator animator;
    float dist = 0.0f;


    InfoMarker[] infoMarkers;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        infoMarkers = FindObjectsOfType<InfoMarker>();
    }
	
	// Update is called once per frame
	void Update () {

		if ((previousPlaceState != ARController.placeState) && (ARController.placeState == GuidedARController.PlaceState.SHOW))
        {

            gameObject.transform.position = startPoint.position;
            gameObject.transform.rotation = startPoint.rotation;
        }
        previousPlaceState = ARController.placeState;

        if (ARController.placeState == GuidedARController.PlaceState.SHOW)
        {
            CheckforInput();
            CheckState();
            TurnTowardCamera();
            MoveTowardPlayer();
        }

	}

    IEnumerator ResetToIdle()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        animator.SetBool("talking", false);
        animator.SetInteger("idlenum", (int)Random.Range(0, 2));

    }

    internal void Talk()
    {
        TurnTowardCamera();

        int index = 0;
        Vector3 closest = infoMarkers[0].transform.position;

        foreach (InfoMarker i in infoMarkers)
        {
            float d = Vector3.Distance(playerCamera.transform.position, i.transform.position);
            if (d <= 3)
            {
                if (d < Vector3.Distance(playerCamera.transform.position, closest))
                {
                    closest = i.transform.position;
                    index = i.infoIndex;
                }
            }
        }


        if (!audioSource.isPlaying)
        {
            animator.SetBool("talking", true);
            animator.SetInteger("talknum", (int)Random.Range(0, 3));
            if (index > 0)
            {
                audioSource.clip = dialogues[index];
            } 
            else
            {
                audioSource.clip = generalPhrases[audioIndex];
                audioIndex++;
                if (audioIndex == generalPhrases.Length)
                {
                    audioIndex = 0;
                }
            }
            audioSource.Play();
            StartCoroutine(ResetToIdle());
        }
    }

 

    void CheckforInput()
    {
        if (ARController.placeState == GuidedARController.PlaceState.SHOW)
        {

            if (Input.touchCount > 0)// && Input.GetTouch(0).phase == TouchPhase.Began)
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                //Select Hool
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject.name == "TourGuide")
                    {

                        outputText.text = "Hit";
                        if (!audioSource.isPlaying)
                        {

                            audioSource.clip = dialogues[0];
                            audioSource.Play();
                        }


                    }
                    else
                    {
                        outputText.text = "Miss";
                    }
                }
            }
        }
    }
    void CheckState()
    {
        Vector3 target = new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z);
        dist = Vector3.Distance(target, transform.position);
      //  outputText.text = "Dist:" + dist.ToString();
        if (dist >= 3.0f)
        {
            animator.SetBool("walking", true);
            animator.SetInteger("walknum", (int)Random.Range(0, 2));
            guideState = TourGuideStates.FOLLOWING;
         
        }
        else if (dist <= 1.5f)
        {
            guideState = TourGuideStates.IDLE;
            animator.SetBool("walking", false);
         
        }

    }


    void MoveTowardPlayer()
    {
        Vector3 target = new Vector3(playerCamera.transform.position.x, transform.position.y, playerCamera.transform.position.z);

        if (guideState == TourGuideStates.FOLLOWING)
        {
          
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime/8);
        }
       
       


       
    }

    void TurnTowardCamera()
    {

        if (guideState == TourGuideStates.FOLLOWING)
        {
            Vector3 cameraPositionSameY = playerCamera.transform.position;
            cameraPositionSameY.y = transform.position.y;

            // Have Andy look toward the camera respecting his "up" perspective, which may be from ceiling.
          //  Vector3 relativePos = cameraPositionSameY - transform.position;
          //  Quaternion rotation;
          //  rotation = Quaternion.FromToRotation(transform.forward, relativePos);
          //  transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * agility);
            transform.LookAt(cameraPositionSameY, transform.up);

        }

      /*  Quaternion current = transform.rotation;
        Vector3 relativePos = playerCamera.transform.position - transform.position; //Vector points towards target
        Quaternion rotation;
        rotation = Quaternion.FromToRotation(Vector3.up, relativePos);
        float angle = Quaternion.Angle(current, rotation);

        transform.rotation = Quaternion.Slerp(current, rotation, Time.deltaTime * agility);*/
    }
}
