using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSwapper : MonoBehaviour {

    public Texture2D[] moonTextures;

    Renderer rend;
    int textureID = 0;

   public Scene_Manager sceneManager;
    float control;   

	// Use this for initialization
	void Start () {

        rend = GetComponent<Renderer>();
      //  sceneManager = GetComponent<Scene_Manager>();

	}
	
	// Update is called once per frame
	void Update () {
        // take the average of the three stations

        control = sceneManager.gameTimer;

        int newTextureID = (int)(control * 2);
       
        if (textureID != newTextureID)
        {
            rend.material.mainTexture = moonTextures[newTextureID];
            textureID = newTextureID;
        }
		
	}
}
