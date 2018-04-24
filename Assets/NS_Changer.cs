using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NS_Changer : MonoBehaviour {
    Scene_Manager sceneManager;

    [Tooltip("Affects the object's scale")]
    public bool scale = false;
    [Tooltip("Fade between two colors")]
    public bool fade = false;// fade between two colors
    public Color happyColor = Color.white;
    public Color sadColor = Color.gray;

    public bool useGradient = false;
    public Texture2D gradientTexture;

    [Tooltip("Multiplies the sensor reading.  Only affects scale")]
    public float scaleValue = 5000.0f;
    [Tooltip("Ignores Sensor readings below this value \n for particle systems this is a cutoff")]
    public float floor = 0.3f;

    [Tooltip("Ignores Sensor readings above this value\n for particle systems this is a cutoff")]
    public float ceiling = 2.0f;
   // [Tooltip("Affects a light")]
   // public bool lightEffect = false;
    [Tooltip("Uses a custon shader to blend two textures")]
    public bool blendTexture = false;
   // [Tooltip("Affects a Philips Hue Light")]
   // public bool hueLight = false;

    [Tooltip("Affects sound volume")]
    public bool soundVolume = false;

    [Tooltip("flip the direction of the action")]
    public bool reverseDirection = false;

    [Tooltip("Affects a Particle System")]
    public bool particleControl = false;
    public ParticleSystem particleSystem;
    [Tooltip("Which sensor affects the change")]
    
    //public HueLamp hueLamp;
    Renderer rend;

    internal int currentColor = 0;
   public Light effectLight;

	// Use this for initialization
	void Start () {
        sceneManager = FindObjectOfType<Scene_Manager>();
        rend = GetComponent<Renderer>();
	}

    Color CycleColors()
    {
        if (Random.Range(0.0f, 1.0f) < 0.1f)
        {
            if (currentColor == 1)
            {
                currentColor = 0;

            }
            else
            {
                currentColor = 1;
            }
        }
        if (currentColor == 1)
        {
            return happyColor;
        }
        else
        {
         return sadColor;
        }
    }
	// Update is called once per frame
	void Update () {

        float control = sceneManager.gameTimer;
      

		if (control < floor)
        {
            control = floor;
        }
        if (control > ceiling)
        {
            control = ceiling;
        }

        if (reverseDirection)
        {
            control = 2.0f - control;
        }

        Color newColor;

        if (useGradient)
        {
            if (sceneManager.gameState == Scene_Manager.GameState.RUNNING)
            {
                newColor = gradientTexture.GetPixelBilinear((control / 2), 0.5f);
            }
            else
            {
              newColor =  CycleColors();
            }
        }
        else
        {
            newColor = Color.Lerp(happyColor, sadColor, (control));
        }


        if (soundVolume)
        {
            foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
            {
                if (reverseDirection && control < ceiling)
                {
                    source.volume = 0.0f;
                }
                else
                {
                    source.volume = control / 2;
                }
            }
        }
        if (scale )
        {

            transform.localScale = new Vector3((scaleValue * control), 
                                                (scaleValue * control), 
                                                (scaleValue * control));
        }
    
        if (fade)
        {
           foreach (Material m in GetComponent<MeshRenderer>().materials)
            {
                m.SetColor("_Color", newColor);
            }
        }
    
        if (particleControl)
        {
            ParticleSystem.EmissionModule em = particleSystem.emission;
            if (control > floor && control < ceiling)
            {
               
                em.enabled = true;
            }
            else
            {
                em.enabled = false;
            }
        }
        if (blendTexture)
        {
            rend.material.SetFloat("_BlendPoint", control / 2);
        }

    }


   
}
