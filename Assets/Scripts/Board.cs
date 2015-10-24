using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public class CardData
    {
        public int Number;
        public string Type;
        public int Collumn;
        public bool Flipped;
        public float xDiference;
        public float yDiference;
        public Vector3 MoveLocation;
        public Vector3 PreviousPosition;
        public int PreviousCollumn;
        public bool isDragged;
    }
    private CardData Data;
    public static string WhatIsDragged;
    public  GameObject Card;
    public SpriteRenderer CardRenderer;
    public GameObject StartPosition;
    public Sprite[] CardSprites;
    public static Dictionary<GameObject, CardData> Cards = new Dictionary<GameObject, CardData>(); 
    void Start ()
    {
        foreach (Sprite sprite in CardSprites)
        {
            Data = new CardData();
            string[] name = sprite.name.Split('_');
            CardRenderer.sprite = sprite;
            Data.Number = int.Parse(name[0]);
            Data.Type = name[2];
            Data.Collumn = 0;
            Data.Flipped = false;
            Data.MoveLocation = StartPosition.transform.position;
            Cards.Add((GameObject)Instantiate(Card, StartPosition.transform.position, Quaternion.identity), Data);
        }
    }

    void Update () {
	
	}
}
