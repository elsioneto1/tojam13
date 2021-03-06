﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSet", menuName = "Waves")]
public class WaveScriptableObject :  ScriptableObject
{
	public WaveSet FirstSet, SecondSet, ThirdSet; //FourthSet;
	public int totalWaveTime;
	
}

[System.Serializable]
public class WaveSet
{
	public List<Enums.ActionTypes> actions;
}
