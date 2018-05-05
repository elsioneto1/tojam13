using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
	[SerializeField] private Text tileText;
	Image imageColor;
	[SerializeField] private Color player1color, player2color, player3color;

	private void Awake()
	{
		imageColor = GetComponent<Image>();
	}

	public void SetTileAction(Enums.ActionTypes action)
	{
		tileText.text = action.ToString();
	}

	public void SetTileColor(Enums.Players playerNumber)
	{
		switch (playerNumber)
		{
			case Enums.Players.Player1:
				imageColor.color = player1color;
				break;

			case Enums.Players.Player2:
				imageColor.color = player2color;
				break;

			case Enums.Players.Player3:
				imageColor.color = player3color;
				break;

		}
	}
}
