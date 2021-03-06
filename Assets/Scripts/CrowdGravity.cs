﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrowdGravity : MonoBehaviour {
    public static float vectorPower = 1f;
    public static CrowdGravity S_INSTANCE;
    public static CrowdEntity[] entities;
   // public SphereCollider sphereCollider;
    // Use this for initialization
    void Start()
    {

        if (entities == null)
            entities = FindObjectsOfType<CrowdEntity>();

        S_INSTANCE = this;
        // sphereCollider = GetComponent<SphereCollider>();
        SceneManager.sceneUnloaded += Clear;
    }
    void Clear(Scene s)
    {
        entities = null;
    }
    

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].ProcessMovementVector(gameObject);
            entities[i].CustomUpdate();
        }

    }
}
