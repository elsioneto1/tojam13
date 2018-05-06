using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private int currentIndex = 0;
    public LevelSequence[] levelSequence;


    [System.Serializable]
    public class LevelConfig
    {

        public string name;
        public Transform crowdPoint;
        public CrowdManager.ColliderType colliderType;
        public float colliderSize;
        public float rotation;
        public float appearAfterSeconds;
        public float staySeconds;

    }
    
    [System.Serializable]
    public class LevelSequence
    {
        [Header("Gravity Location")]
        public Transform gravityLocation;

        [Header("Wave Config")]
        public string name;
        public float startAfterSeconds = 1;
        public float duration = 1;
        public LevelConfig[] sequenceDynamic;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(LevelStart());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LevelStart()
    {
        Debug.Log("starting level sequence " + currentIndex);
        yield return new WaitForSeconds(levelSequence[currentIndex].startAfterSeconds);

        Debug.Log("executing sequence " + currentIndex);
        for (int i = 0; i < levelSequence[currentIndex].sequenceDynamic.Length; i++)
        {
            StartCoroutine(SetManager(levelSequence[currentIndex].sequenceDynamic[i]));
        }
        CrowdGravity.S_INSTANCE.transform.position = levelSequence[currentIndex].gravityLocation.position;
        yield return new WaitForSeconds(levelSequence[currentIndex].duration);

        Debug.Log("exiting " + currentIndex);
        CrowdManager.ResetManagerPosition();
        currentIndex++;
        if (currentIndex < levelSequence.Length )
            StartCoroutine(LevelStart());
        else
        {
            currentIndex = 0;
            StartCoroutine(LevelStart());

        }
    }

    IEnumerator SetManager(LevelConfig lvlConfig)
    {
        CrowdManager manager = CrowdManager.GetInstanceManager(lvlConfig.colliderType);
        Debug.Log("loading type" + manager.name + " to object " + lvlConfig.crowdPoint.name);

        yield return new WaitForSeconds(lvlConfig.appearAfterSeconds);
        CrowdManager.RemoveManagerFromScreen();
        manager.SetPositionScaleRotation(lvlConfig.crowdPoint.transform.position,lvlConfig.colliderSize,lvlConfig.rotation);
        yield return new WaitForSeconds(lvlConfig.staySeconds);
        manager.ResetComp();


    }

}
