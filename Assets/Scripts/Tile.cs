using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	Image image;
	//[SerializeField] private List<Sprite> player1Sprites = new List<Sprite>();
	//[SerializeField] private List<Sprite> player2Sprites = new List<Sprite>();
	//	[SerializeField] private List<Sprite> player3Sprites = new List<Sprite>();

	[SerializeField] private List<Sprite> actionList = new List<Sprite>();

	//Enums.ActionTypes myAction;

	private void Awake()
	{
		image = GetComponent<Image>();
	}

	public void SetTileAction(Enums.ActionTypes action)
	{
		switch (action)
		{
			case Enums.ActionTypes.Slide:
				image.sprite = actionList[ (int)action ];
				break;

			case Enums.ActionTypes.Jump:
				image.sprite = actionList[ (int)action ];
				break;

			case Enums.ActionTypes.Hurricane:
				image.sprite = actionList[ (int)action ];
				break;
		}	

	}

	//public void SetTileColor(Enums.Players playerNumber)
	//{
	//	switch (playerNumber)
	//	{
	//		//case Enums.Players.Player1:
	//		//	image.sprite = player1Sprites[ (int)myAction ];
	//		//	break;

	//		//case Enums.Players.Player2:
	//		//	image.sprite = player2Sprites[ (int)myAction ];
	//		//	break;

	//		//case Enums.Players.Player3:
	//		//	image.sprite = player3Sprites[ (int)myAction ];
	//		//	break;

	//	}
	//}
}
