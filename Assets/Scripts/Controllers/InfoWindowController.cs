using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utilites.UI;

public class InfoWindowController : MonoBehaviour {

    private Text Header;
    private GameObject Viewport;

    private void Awake() {
        
    }

    public void Construct(string headerText, ImageWBullet[] panels) {

        Header.text = headerText;
        foreach(ImageWBullet panel in panels) {

            // Load asset and build it
        }
    }
}