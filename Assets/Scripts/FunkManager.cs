using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FunkManager : MonoBehaviour
{

    public static FunkManager S_INSTANCE;

    [Header("Common Attributes")]
    public int timeBetweenCombos;
    public int resetTime;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Countdown countdown;

    [Header("Waves")]
    public List<WaveScriptableObject> waveList;
    private int currentSet;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    public List<RectTransform> comboHolderPositionList;

    private List<GameObject> waveTiles = new List<GameObject>();
    //	private List<GameObject> player3Tiles = new List<GameObject>();

    //private List<Enums.ActionTypes> player1Actions = new List<Enums.ActionTypes>();
    //private List<Enums.ActionTypes> player2Actions = new List<Enums.ActionTypes>();
    //private List<Enums.ActionTypes> player3Actions = new List<Enums.ActionTypes>();
    private List<WaveSet> waveCombos = new List<WaveSet>();

    public static FunkManager singleton;
    private bool waveCompleted = false;

    private float points = 7;
    public int maxPoints;

    public float comboTimeFrame = 1;

    private int combosCompleted = 0;

    public UnityEvent PositivePointsCB;
    public UnityEvent NegativePointsCB;

    private void Awake()
    {
        singleton = this;
    }
    private void Start()
    {
        
        StartCoroutine(BuildNextWaveSet());
        S_INSTANCE = this;

    }

    private IEnumerator BuildNextWaveSet()
    {
        while (true)
        {
            int time = waveList[currentSet].totalWaveTime;

            waveCombos.Add(waveList[currentSet].FirstSet);
            waveCombos.Add(waveList[currentSet].SecondSet);
            waveCombos.Add(waveList[currentSet].ThirdSet);
            //waveCombos.Add(waveList[currentSet].FourthSet);
            waveCombos.Shuffle();

            combosCompleted = waveCombos.Count;
            BuildPlayerTiles(waveCombos);

            yield return new WaitForSeconds(1);
            ScrollTilesDown(0, 1, 1);
            ScrollTilesDown(1, 1, 1);
            ScrollTilesDown(2, 1, 1);
            //yield return new WaitForSeconds(timeBetweenCombos);
            //ScrollTilesDown(3, 1, 1);
            //	ScrollTilesDown(player3Tiles, 1, 1);
            //yield return new WaitForSeconds(preparingTime);
            //countdown.SetTime(waveList[ currentSet ].totalWaveTime);
            //Libera os botoes dos jogadores
            //yield return new WaitForSeconds(waveList[ currentSet ].totalWaveTime);

            while (combosCompleted > 0)
                yield return null;
            //Cabou o tempo, computar score, reseta
            //if(waveCompleted)
            //WIN AND LOSE
            yield return new WaitForSeconds(resetTime);
            //Limpa tudo que tiver que limpar, comeca de novo, proxima wave ou end
            ResetUI();
            if(currentSet + 1 < waveList.Count)
                currentSet++;
        }
    }

    private void BuildPlayerTiles(List<WaveSet> randomizedList)
    {
        waveCompleted = false;

        for (int i = 0; i < comboHolderPositionList.Count; ++i)
        {
            for (int k = 0; k < randomizedList[i].actions.Count; ++k)
            {
                GameObject tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
                Tile t = tile.GetComponent<Tile>();
                t.SetTileAction(randomizedList[i].actions[k]);
                //t.SetTileColor((Enums.Players)i);
                tile.transform.SetParent(canvas.transform);
                tile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                tile.GetComponent<RectTransform>().anchoredPosition3D = comboHolderPositionList[i].anchoredPosition3D + new Vector3(45 * k, 50, 0);

                waveTiles.Add(tile);
            }
        }
    }

    private void ResetUI()
    {
        //for (int i = waveTiles.Count - 1; i >= 0; --i)
        //{
        //    Destroy(singleton.waveTiles[i]);
        //}
        //waveCombos.Clear();
        //waveTiles.Clear();

        //for(int i = player3Tiles.Count - 1; i >= 0; --i)
        //{
        //	Destroy(singleton.player3Tiles[i]);
        //}	

        //player2Actions.Clear();
        //player3Actions.Clear();

        //player3Tiles.Clear();
    }

    private void ScrollTilesDown(int index, int units, float time)
    {
        if (waveTiles.Count == 0)
            return;
        for (int i = index * waveCombos[index].actions.Count; i < waveCombos[index].actions.Count * (index + 1) ; i++)
        {
            Debug.Log(i);
            iTween.MoveTo(waveTiles[i], waveTiles[i].transform.position + new Vector3(0, -140 * units, 0), time);
        }
    }

    private void DestroyTiles(int index)
    {
        for (int i = ((index + 1) * waveCombos[index].actions.Count) - 1; i >= index * waveCombos[index].actions.Count; i--)
        {
            Destroy(waveTiles[i]);
            waveTiles.Remove(waveTiles[i]);
        }

    }

    public static bool CompleteAction(List<Enums.ActionTypes> comboList)
    {
        if (singleton.waveCombos.Count == 0 || comboList.Count == 0)
			return false;

        //bool shouldReturn = true;
        //for(int p = 0; p < singleton.waveCombos.Count; p++)
        //{
        //    if (comboList.Count == singleton.waveCombos[p].actions.Count)
        //        shouldReturn = false;
        //}

        //if (shouldReturn)
        //    return false;

        if (comboList.Count < 3)
            return false;

		for(int i = 0; i < singleton.waveCombos.Count; i ++)
		{
            bool isWrong = false;

            for (int k = 0; k < singleton.waveCombos[i].actions.Count; k++)
			{
                if (comboList.Count > singleton.waveCombos[i].actions.Count)
                    isWrong = true;

                else if (singleton.waveCombos[ i ].actions[k] != comboList[k])
				{
                    isWrong = true;
                }
				else if(k == singleton.waveCombos[i].actions.Count - 1)
				{
                    if (!isWrong)
                    {
                        singleton.DestroyTiles(i);
                        singleton.waveCombos.Remove(singleton.waveCombos[i]);
                        singleton.ModifyPoints(1);
                        singleton.combosCompleted--;
                        comboList.Clear();
                        return true;
                    }
                }
			}	
		}

		singleton.CheckLevel();
        return false;
	}

	private void CheckLevel()
	{
		if(waveCombos.Count == 0)// && player3Actions.Count == 0)
		{
			waveCompleted = true;
		}
	}

    public void ModifyPoints(int points)
    {

        if (points > 0)
        {
			this.points += points;
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

        if (this.points < 0)
        {
            this.points = 0;
            SceneManager.LoadScene("LoseScreen");
        }
            
        if (this.points > maxPoints)
        {
            this.points = maxPoints;
            SceneManager.LoadScene("WinScreen");
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


