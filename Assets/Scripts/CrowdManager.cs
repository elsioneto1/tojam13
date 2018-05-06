using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour {

    public static float vectorPower = 0.9f;

    public static CrowdEntity[] entities;
    public SphereCollider sphereCollider;
	// Use this for initialization
	void Start () {

        if (entities == null)
            entities = FindObjectsOfType<CrowdEntity>();
        sphereCollider = GetComponent<SphereCollider>();
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].ProcessMovementVector(gameObject);
            entities[i].CustomUpdate();
        }
        
	}
}
