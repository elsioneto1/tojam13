using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarFill : MonoBehaviour {

    RectTransform fillWidth;

    private void Awake()
    {
        fillWidth = transform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        FunkManager.singleton.PositivePointsCB.AddListener(MoveUp);
        FunkManager.singleton.NegativePointsCB.AddListener(MoveDown);
    }
    private void MoveUp()
    {
        fillWidth.sizeDelta = new Vector2(fillWidth.sizeDelta.x + 25, fillWidth.sizeDelta.y);
    }
    private void MoveDown()
    {
        fillWidth.sizeDelta = new Vector2(fillWidth.sizeDelta.x - 25 , fillWidth.sizeDelta.y);
    }
}
