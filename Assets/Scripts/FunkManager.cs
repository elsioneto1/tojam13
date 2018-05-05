using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkManager : MonoBehaviour
{
	[Header("Common Attributes")]
	public int preparingTime;
	public int resetTime;
	[SerializeField] private GameObject tilePrefab;
	
	
	[Header("Waves")]
	public List<WaveScriptableObject> waveList;
	private int currentSet;

	[Header("UI")]
	[SerializeField] private Canvas canvas;
	public List<RectTransform> tileHolderPositionList;

	private List<Enums.ActionTypes> player1Actions = new List<Enums.ActionTypes>();
	private List<Enums.ActionTypes> player2Actions = new List<Enums.ActionTypes>();
	private List<Enums.ActionTypes> player3Actions = new List<Enums.ActionTypes>();

	//public List<Player> playerList = new List<Player>();

	private void Start()
	{
		StartCoroutine(BuildNextWaveSet());
	}

	private IEnumerator BuildNextWaveSet()
	{
		while(currentSet < waveList.Count)
		{
			int time = waveList[ currentSet ].totalWaveTime;
			List<WaveSet> tempList = new List<WaveSet>();

			tempList.Add(waveList[currentSet].FirstSet);
			tempList.Add(waveList[currentSet].SecondSet);
			tempList.Add(waveList[currentSet].ThirdSet);
			tempList.Shuffle();

			List<GameObject> finalList = BuildPlayerTiles(tempList);
			
			yield return new WaitForSeconds(preparingTime);
			ScrollTilesDown(finalList, 1, 1);
			//Libera os botoes dos jogadores
			yield return new WaitForSeconds(waveList[ currentSet ].totalWaveTime);
			//Cabou o tempo, computar score, reseta
			yield return new WaitForSeconds(resetTime);
			//Limpa tudo que tiver que limpar, comeca de novo, proxima wave ou end
			currentSet++;
		}

		//Game Over -> EndScreen
	}

	private List<GameObject> BuildPlayerTiles(List<WaveSet> randomizedList)
	{
		List<GameObject> tileObjectList = new List<GameObject>();

		for (int i = 0; i < tileHolderPositionList.Count; ++i)
		{
			for (int k  = 0; k < randomizedList[i].wave.Count; ++k)
			{
				GameObject tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
				Tile t = tile.GetComponent<Tile>();
				t.SetTileAction(randomizedList[ i ].wave[ k ]);
				t.SetTileColor((Enums.Players)i);
				tile.transform.SetParent(canvas.transform);
				tile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				tile.GetComponent<RectTransform>().anchoredPosition3D = tileHolderPositionList[ i ].anchoredPosition3D + new Vector3(0, 50 * (k + 1), 0);
				tileObjectList.Add(tile);

				if (i == 0)
					player1Actions.Add(randomizedList[ i ].wave[ k ]);
				else if (i == 1)
					player2Actions.Add(randomizedList[ i ].wave[ k ]);
				else if (i == 2)
					player3Actions.Add(randomizedList[ i ].wave[ k ]);
			}
		}
		return tileObjectList;
	}

	private void ScrollTilesDown(List<GameObject> tileObjects, int units, float time)
	{
		foreach(GameObject go in tileObjects)
		{
			iTween.MoveTo(go, go.transform.position + new Vector3(0, -120 * units, 0), time);
		}
	}

	public static void CompleteAction(Enums.ActionTypes action, Enums.Players playerNumber)
	{
		switch(playerNumber)
		{
			
		}
	}

}

public static class FunkExtensions
{
	public static void Shuffle<T>(this IList<T> list)  
	{
		Random rng = new Random();  

		int n = list.Count;  
		while (n > 1) {  
			n--;
			int k = Random.Range(0, n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
