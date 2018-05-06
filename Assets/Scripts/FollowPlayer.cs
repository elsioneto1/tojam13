using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Player player;
    Vector3 startpos;
	// Use this for initialization
	void Start () {
        startpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 v = player.transform.position;
        v.y = startpos.y;
        transform.position = v;
	}
}
