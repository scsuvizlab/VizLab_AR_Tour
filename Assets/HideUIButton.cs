using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIButton : MonoBehaviour {

    public GameObject panel;

	public void HideUI()
    {
        panel.SetActive(false);
    }
}
