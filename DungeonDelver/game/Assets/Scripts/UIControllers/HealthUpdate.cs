using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthUpdate : MonoBehaviour
{

    private RectTransform rectTransform;
    public int maxHP = 100;
    public PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchorMax = new Vector2(Player.health / (float)maxHP, 1);
    }
}
