using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : MonoBehaviour {


    public static SpritePool S_INSTANCE;
    public Sprite[] pool;

	// Use this for initialization
	void Awake () {
        S_INSTANCE = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
