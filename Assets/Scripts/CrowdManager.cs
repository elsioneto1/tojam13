using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrowdManager : MonoBehaviour {
    public static float vectorPower = 0.9f;
    public static List<CrowdManager> allManagers;

    public static CrowdManager mainManager;
    public bool beingUsed = false;

    public bool isMainManager = false;
    public enum ColliderType { Sphere,Box}
 //   [HideInInspector]
    public ColliderType colliderType;
    BoxCollider box;
    SphereCollider sphere;

    static Vector3 mainManagerStartPos;

    void Start()
    {
        sphere = GetComponent<SphereCollider>();
        box = GetComponent<BoxCollider>();

        if (sphere)
            colliderType = ColliderType.Sphere;
        if (box)
            colliderType = ColliderType.Box;

        if (allManagers == null)
            allManagers = new List<CrowdManager>();

        allManagers.Add(this);

        if (isMainManager)
        {
            mainManager = this;
            mainManagerStartPos = transform.position;
        }
        SceneManager.sceneUnloaded += Clear;
    }

    void Clear(Scene s )
    {
        mainManager = null;
        allManagers = null;
    }

    public static CrowdManager GetInstanceManager(ColliderType type)
    {

        CrowdManager crowdManager = null;
        for (int i = 0; i < allManagers.Count; i++)
        {
            if (!allManagers[i].isMainManager)
            {
                if (!allManagers[i].beingUsed && type == ColliderType.Box && allManagers[i].GetComponent<BoxCollider>() != null)
                {
                    crowdManager = allManagers[i];
                    crowdManager.beingUsed = true;
                    break;

                }
                if (!allManagers[i].beingUsed && type == ColliderType.Sphere && allManagers[i].GetComponent<SphereCollider>() != null)
                {
                    crowdManager = allManagers[i];
                    crowdManager.beingUsed = true;
                    break;
                }
            }
        }

        return crowdManager;
    }

    public void SetPositionScaleRotation(Vector3 position, float scale, float yaw )
    {
        transform.position = position;
        beingUsed = true;
        transform.localScale = new Vector3(scale,scale,scale);
        transform.rotation = Quaternion.Euler(0,yaw,0);
    }

    public void ResetComp()
    {
        beingUsed = false;
        transform.position = new Vector3(10000,0,0);
    }

    public static void ResetManagerPosition()
    {
        mainManager.transform.position = mainManagerStartPos;
    }

    public static void RemoveManagerFromScreen()
    {
        mainManager.transform.position = new Vector3(100,0,0);
    }

}
