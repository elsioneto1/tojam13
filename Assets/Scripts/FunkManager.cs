using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FunkManager : MonoBehaviour
{

    public static FunkManager S_INSTANCE;

	[Header("Common Attributes")]
	public int preparingTime;
	public int resetTime;
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private Countdown countdown;	
	
	[Header("Waves")]
	public List<WaveScriptableObject> waveList;
	private int currentSet;

	[Header("UI")]
	[SerializeField] private Canvas canvas;
	public List<RectTransform> tileHolderPositionList;

	private List<GameObject> player1Tiles = new List<GameObject>();
	private List<GameObject> player2Tiles = new List<GameObject>();
	private List<GameObject> player3Tiles = new List<GameObject>();

	private List<Enums.ActionTypes> player1Actions = new List<Enums.ActionTypes>();
	private List<Enums.ActionTypes> player2Actions = new List<Enums.ActionTypes>();
	private List<Enums.ActionTypes> player3Actions = new List<Enums.ActionTypes>();

    public static FunkManager singleton;
	private bool waveCompleted = false;

    private float points;
    public int maxPoints;

    public float comboTimeFrame = 1;


    public UnityEvent PositivePointsCB;
    public UnityEvent NegativePointsCB;

    private void Start()
	{
		singleton = this;
		StartCoroutine(BuildNextWaveSet());
        S_INSTANCE = this;

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

			BuildPlayerTiles(tempList);

			yield return new WaitForSeconds(1);
			ScrollTilesDown(player1Tiles, 1, 1);
			ScrollTilesDown(player2Tiles, 1, 1);
			ScrollTilesDown(player3Tiles, 1, 1);
			yield return new WaitForSeconds(preparingTime);
			countdown.SetTime(waveList[ currentSet ].totalWaveTime);
			//Libera os botoes dos jogadores
			yield return new WaitForSeconds(waveList[ currentSet ].totalWaveTime);
			//Cabou o tempo, computar score, reseta
			//if(waveCompleted)
			//WIN AND LOSE
			yield return new WaitForSeconds(resetTime);
			//Limpa tudo que tiver que limpar, comeca de novo, proxima wave ou end
			ResetUI();
			currentSet++;
		}

		//Game Over -> EndScreen
	}

	private void BuildPlayerTiles(List<WaveSet> randomizedList)
	{
		waveCompleted = false;

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
				tile.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
				tile.GetComponent<RectTransform>().anchoredPosition3D = tileHolderPositionList[ i ].anchoredPosition3D + new Vector3(0, 50 * (k + 1), 0);
				tileObjectList.Add(tile);

				if (i == 0)
				{
					player1Actions.Add(randomizedList[ i ].wave[ k ]);
					player1Tiles.Add(tile);
				}
					
				else if (i == 1)
				{
					player2Actions.Add(randomizedList[ i ].wave[ k ]);
					player2Tiles.Add(tile);
				}
				else if (i == 2)
				{
					player3Actions.Add(randomizedList[ i ].wave[ k ]);
					player3Tiles.Add(tile);
				}
			}
		}
	}

	private void ResetUI()
	{
		for(int i = player1Tiles.Count - 1; i >= 0; --i)
		{
			Destroy(singleton.player1Tiles[i]);
		}
		for(int i = player2Tiles.Count - 1; i >= 0; --i)
		{
			Destroy(singleton.player2Tiles[i]);
		}
		for(int i = player3Tiles.Count - 1; i >= 0; --i)
		{
			Destroy(singleton.player3Tiles[i]);
		}

		player1Actions.Clear();
		player2Actions.Clear();
		player3Actions.Clear();
		player1Tiles.Clear();
		player2Tiles.Clear();
		player3Tiles.Clear();
	}

	private void ScrollTilesDown(List<GameObject> tileObjects, int units, float time)
	{
		foreach(GameObject go in tileObjects)
		{
			iTween.MoveTo(go, go.transform.position + new Vector3(0, -140 * units, 0), time);
		}
	}

	public static void CompleteAction(Enums.ActionTypes action, Enums.Players playerNumber)
	{
		switch (playerNumber)
		{
			case Enums.Players.Player1:
				if (singleton.player1Actions.Count == 0)
					return;

				else if (singleton.player1Actions[ 0 ] == action)
				{
					Destroy(singleton.player1Tiles[0]);
					singleton.player1Actions.Remove(singleton.player1Actions[ 0 ]);
					singleton.player1Tiles.Remove(singleton.player1Tiles[ 0 ]);

					if(singleton.player1Actions.Count > 0)
					{
						singleton.ScrollTilesDown(singleton.player1Tiles,1,1);
					}
				}
				break;

			case Enums.Players.Player2:
				if (singleton.player2Actions.Count == 0)
					return;

				else if (singleton.player2Actions[ 0 ] == action)
				{
					Destroy(singleton.player2Tiles[0]);
					singleton.player2Actions.Remove(singleton.player2Actions[ 0 ]);
					singleton.player2Tiles.Remove(singleton.player2Tiles[ 0 ]);

					if(singleton.player2Actions.Count > 0)
					{
						singleton.ScrollTilesDown(singleton.player2Tiles,1,1);
					}
				}
				break;

			case Enums.Players.Player3:

				if (singleton.player3Actions.Count == 0)
					return;

				else if (singleton.player3Actions[ 0 ] == action)
				{
					Destroy(singleton.player3Tiles[0]);
					singleton.player3Actions.Remove(singleton.player3Actions[ 0 ]);
					singleton.player3Tiles.Remove(singleton.player3Tiles[ 0 ]);

					if(singleton.player3Actions.Count > 0)
					{
						singleton.ScrollTilesDown(singleton.player3Tiles,1,1);
					}
				}
				break;
		}

		singleton.CheckLevel();
	}

	private void CheckLevel()
	{
		if(player1Actions.Count == 0 && player2Actions.Count == 0 && player3Actions.Count == 0)
		{
			waveCompleted = true;
		}
	}

    public void ModifyPoints(int points)
    {

        if (points > 0)
        {

            //this.points += points * successStreak;
            //successStreak += 0.1f;
            //if (successStreak > 2)
            //    successStreak = 2;
            PositivePointsCB.Invoke();
        }
        else if (points < 0)
        {
            this.points += points ;
          //  successStreak = 1;
            NegativePointsCB.Invoke();
        }

        if (points < 0)
            points = 0;
        if (points > maxPoints)
            points = maxPoints;

        
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


