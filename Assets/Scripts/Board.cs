using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour
{

    public GameObject[] Collumns;
    public GameObject[] StackCollumns;
    private CardData Data;
    public static string WhatIsDragged;
    public GameObject Card;
    public SpriteRenderer CardRenderer;
    public GameObject StartPosition;
    public Sprite[] CardSprites;
    public static Dictionary<GameObject, CardData> Cards = new Dictionary<GameObject, CardData>();
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
    void Start()
    {
        foreach (Sprite sprite in CardSprites.OrderByDescending(s => int.Parse(s.name.Split('_')[0])))
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
        float freeStace = (renderer.bounds.size.x - (Collumns[0].renderer.bounds.size.x * Collumns.Length)) / (Collumns.Length * 2);
        float leftBound = (transform.position.x - renderer.bounds.size.x / 2);
        float collumnSize = Collumns[0].renderer.bounds.size.x;
        for (int i = 0; i < Collumns.Length; i++)
        {
            float a = (leftBound + ((collumnSize + (freeStace * 2)) * i) + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
            Instantiate(Collumns[i], new Vector3(a, Collumns[i].transform.position.y, Collumns[i].transform.position.z),
                Quaternion.identity);
        }
        for (int i = 0; i < StackCollumns.Length; i++)
        {
            float a = (leftBound + ((collumnSize + (freeStace * 2)) * (i + 3)) + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
            Instantiate(StackCollumns[i],
                new Vector3(a, StartPosition.transform.position.y, StartPosition.transform.position.z),
                Quaternion.identity);
        }
        float b = (leftBound + freeStace);
        Instantiate(StartPosition,
            new Vector3(b, StartPosition.transform.position.y, StartPosition.transform.position.z), Quaternion.identity);
    }
}
