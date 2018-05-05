using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MovementObject : ScriptableObject {


    public float test;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/MovementObject")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<MovementObject>();
    }
#endif


}
