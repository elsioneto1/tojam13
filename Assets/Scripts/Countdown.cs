using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour {

	private bool isCounting = false;
	int currentTime;

	TextMesh tmComponent;

	private void Awake()
	{
		tmComponent = GetComponent<TextMesh>();
	}

	public void SetTime(int time)
	{
		isCounting = true;
		currentTime = time;
		tmComponent.text = currentTime.ToString();
		StartCoroutine(CountTime());
	}

	private IEnumerator CountTime()
	{
		while(currentTime > 0)
		{
			yield return new WaitForSeconds(1);
			currentTime--;
			tmComponent.text = currentTime.ToString();
		}

		isCounting = false;
		tmComponent.text = string.Empty;
	}

}
