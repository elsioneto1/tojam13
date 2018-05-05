using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SpecialMovement : ScriptableObject{

    [System.Serializable]
    public class FloatIntervals{
        public float start;
        public float end;
    }

    // vel over time
    public AnimationCurve movementBehasviour = AnimationCurve.Linear(0,0,1,1);
    public FloatIntervals[] controlIntervals;
    public float movementInitialForce;
    public float timeScaler = 1;
    public float speedModifier;
    public float jumpVelocity;

    public bool applyHurricane = false;
    public bool allowSteering = false;
    public float steeringAngle = 1;

    [Header("conditions to perform the movement")]
    public float velocityBiggerThan = 0;
    public bool grounded;

    public int pointsOnSuccess = 10;
    public int pointsOnFailure = 10;

    private float movementCurrentElapsedTime;
    public void UpdateStates(float elapsedTime)
    {

        movementCurrentElapsedTime += elapsedTime;
    }

    private bool controlLock = false;

    public bool GetControlLock()
    {
        controlLock = false;
        for (int i = 0; i < controlIntervals.Length; i++)
        {
            if (movementCurrentElapsedTime > controlIntervals[i].start && movementCurrentElapsedTime < controlIntervals[i].end)
                controlLock = true;
        }
        return controlLock;
    }

    public float GetElapsedTime()
    {

        return movementCurrentElapsedTime;

    }

    public void ResetTime()
    {
        movementCurrentElapsedTime = 0;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/SpecialMovement")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<SpecialMovement>();
    }
#endif
}
