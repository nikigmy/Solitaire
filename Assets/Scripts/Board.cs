using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

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
        public float XDiference;
        public float YDiference;
        public Vector3 MoveLocation;
        public Vector3 PreviousPosition;
        public int PreviousCollumn;
        public bool IsDragged;
        public bool Black;
        public int PositionOnStart;
    }
    void Start()
    {
        int position = 1;
        foreach (Sprite sprite in CardSprites.OrderByDescending(s => int.Parse(s.name.Split('_')[0])))
        {
            Data = new CardData();
            string[] name = sprite.name.Split('_');
            CardRenderer.sprite = sprite;
            Data.Number = int.Parse(name[0]);
            Data.Type = name[2];
            Data.Collumn = 0;
            Data.Flipped = true;
            Data.PositionOnStart = position;
            Data.MoveLocation = StartPosition.transform.position;
            Data.Black = name[2] == "spades" || name[2] == "clubs";
            Cards.Add((GameObject)Instantiate(Card, StartPosition.transform.position, Quaternion.identity), Data);
            position++;
        }
        float freeStace = (renderer.bounds.size.x - (Collumns[0].renderer.bounds.size.x * Collumns.Length)) / (Collumns.Length * 2);
        float leftBound = (transform.position.x - renderer.bounds.size.x / 2);
        float collumnSize = Collumns[0].renderer.bounds.size.x;
        float[] xPositions = new float[7];
        for (int i = 0; i < 7; i++)
        {
            xPositions[i] = (leftBound + ((collumnSize + (freeStace * 2)) * i) + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
        }
        for (int i = 0; i < Collumns.Length; i++)
        {
            float a = (leftBound + ((collumnSize + (freeStace * 2)) * i) + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
            Collumns[i].transform.position = new Vector3(a, Collumns[i].transform.position.y,
                Collumns[i].transform.position.z);
            Instantiate(Collumns[i], new Vector3(a, Collumns[i].transform.position.y, Collumns[i].transform.position.z),
                Quaternion.identity);
        }
        for (int i = 0; i < StackCollumns.Length; i++)
        {
            float a = (leftBound + ((collumnSize + (freeStace * 2)) * (i + 3)) + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
            StackCollumns[i].transform.position = new Vector3(a, StartPosition.transform.position.y,
                StartPosition.transform.position.z);
            Instantiate(StackCollumns[i],
                new Vector3(a, StartPosition.transform.position.y, StartPosition.transform.position.z),
                Quaternion.identity);
        }
        float b = (leftBound + freeStace + (Collumns[0].renderer.bounds.size.x / 2));
        StartPosition.transform.position = new Vector3(b, StartPosition.transform.position.y,
            StartPosition.transform.position.z);
        Instantiate(StartPosition,
            new Vector3(b, StartPosition.transform.position.y, StartPosition.transform.position.z), Quaternion.identity);
        Deal();
    }
    void Deal()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 6; j >= i; j--)
            {
                IEnumerable<KeyValuePair<GameObject, CardData>> CollumnCards =
                    Cards.Where(c => c.Value.Collumn == 0).OrderByDescending(c => c.Value.PositionOnStart);
                CollumnCards.Last().Key.transform.position = new Vector3(Collumns[j].transform.position.x,
                    Collumns[j].transform.position.y -
                    (Cards.Count(c => c.Value.Collumn == j + 1)*(Collumns[j].renderer.bounds.size.y/6)), 0);
                CollumnCards.Last().Value.Collumn = j + 1;
            }
        }
        
    }
}
