using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    SpriteRenderer sr;
    int alphaDecay = 5;
    int alpha = 255;
    // Use this for initialization
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {

        if (this.transform.position.y < 20)
            Destroy(this);

        alpha -= alphaDecay;

    //    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (float)alpha / 255f);
	}
}
