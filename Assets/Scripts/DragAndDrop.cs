using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 MousePosition;
    public Sprite BackSprite;
    private Sprite CardSprite;
    public GameObject[] Collumns;
    public GameObject[] StackCollumns;
    private SpriteRenderer CardRenderer;

    void Start()
    {

        CardRenderer =
            Board.Cards.First(c => c.Key == gameObject).Key.GetComponent("SpriteRenderer") as SpriteRenderer;
        CardSprite = CardRenderer.sprite;
    }
    void Drop()
    {
        for (int i = 0; i < Collumns.Length; i++)
        {
            if (transform.position.x < Collumns[i].transform.position.x + Collumns[i].renderer.bounds.size.x / 2 &&
                transform.position.x > Collumns[i].transform.position.x - Collumns[i].renderer.bounds.size.x / 2 &&
                transform.position.y < Collumns[i].transform.position.y + Collumns[i].renderer.bounds.size.y / 2)
            {
                if (Board.Cards[gameObject].PreviousCollumn == i + 1)
                {
                    break;
                }

                IEnumerable<KeyValuePair<GameObject, Board.CardData>> CollumnCards =
                    Board.Cards.Where(c => c.Value.Collumn == i + 1);
                if (!CollumnCards.Any())
                {
                    if (Board.Cards[gameObject].Number == 13)
                    {
                        Board.Cards[gameObject].MoveLocation = new Vector3(Collumns[i].transform.position.x,
                            Collumns[i].transform.position.y -
                            ((Board.Cards.Count(c => c.Value.Collumn == i + 1)) * (renderer.bounds.size.y / 6)),
                            0);
                        Board.Cards[gameObject].Collumn = i + 1;
                        return;
                    }
                    break;
                }
                KeyValuePair<GameObject, Board.CardData> TopCard =
                    CollumnCards.OrderByDescending(c => c.Key.transform.position.y).Last();
                if (TopCard.Value.Number == Board.Cards[gameObject].Number + 1 && TopCard.Value.Black != Board.Cards[gameObject].Black)
                {
                    Board.Cards[gameObject].MoveLocation = new Vector3(Collumns[i].transform.position.x,
                        Collumns[i].transform.position.y -
                        ((Board.Cards.Count(c => c.Value.Collumn == i + 1)) * (renderer.bounds.size.y / 6)),
                        0);
                    Board.Cards[gameObject].Collumn = i + 1;
                    return;
                }
                break;
            }
        }
        for (int i = 0; i < StackCollumns.Length; i++)
        {
            if (transform.position.x < StackCollumns[i].transform.position.x + StackCollumns[i].renderer.bounds.size.x / 2 &&
                transform.position.x > StackCollumns[i].transform.position.x - StackCollumns[i].renderer.bounds.size.x / 2 &&
                transform.position.y < StackCollumns[i].transform.position.y + StackCollumns[i].renderer.bounds.size.y / 2 &&
                transform.position.y > StackCollumns[i].transform.position.y - StackCollumns[i].renderer.bounds.size.y / 2)
            {
                IEnumerable<KeyValuePair<GameObject, Board.CardData>> StackCollumnCards =
                    Board.Cards.Where(c => c.Value.Collumn == i + 8);
                if (!StackCollumnCards.Any())
                {
                    if (Board.Cards[gameObject].Number == 1)
                    {
                        Board.Cards[gameObject].MoveLocation = new Vector3(StackCollumns[i].transform.position.x, StackCollumns[i].transform.position.y, StackCollumns[i].transform.position.z - Board.Cards[gameObject].Number);
                        Board.Cards[gameObject].Collumn = i + 8;
                        return;
                    }
                    break;
                }
                KeyValuePair<GameObject, Board.CardData> TopCard =
                    StackCollumnCards.OrderBy(c => c.Value.Number).Last();
                if (TopCard.Value.Number == Board.Cards[gameObject].Number - 1 &&
                     TopCard.Value.Type == Board.Cards[gameObject].Type)
                {
                    Board.Cards[gameObject].MoveLocation = TopCard.Key.transform.position;
                    Board.Cards[gameObject].Collumn = i + 8;
                    return;

                }
                break;
            }
        }
        Board.Cards[gameObject].Collumn = Board.Cards[gameObject].PreviousCollumn;
        Board.Cards[gameObject].MoveLocation = Board.Cards[gameObject].PreviousPosition;


    }
    void SetMoveLocation()
    {
        Vector3 a = new Vector3(MousePosition.x - Board.Cards[gameObject].XDiference, MousePosition.y - Board.Cards[gameObject].YDiference, -50);
        Board.Cards[gameObject].MoveLocation = a;
    }

    bool inRectangle()
    {
        if (Board.Cards[gameObject].Collumn < 8)
        {
            if (Board.Cards.Any(c =>
                c.Value.Collumn == Board.Cards[gameObject].Collumn &&
                c.Key.transform.position.y < gameObject.transform.position.y))
            {
                if (MousePosition.x < transform.position.x + renderer.bounds.size.x/2 &&
                    MousePosition.x > transform.position.x - renderer.bounds.size.x/2 &&
                    MousePosition.y < transform.position.y + renderer.bounds.size.y/2 &&
                    MousePosition.y > transform.position.y + renderer.bounds.size.y/3)
                {
                    return true;
                }
            }
            else if (MousePosition.x < transform.position.x + renderer.bounds.size.x / 2 &&
                     MousePosition.x > transform.position.x - renderer.bounds.size.x / 2 &&
                     MousePosition.y < transform.position.y + renderer.bounds.size.y / 2 &&
                     MousePosition.y > transform.position.y - renderer.bounds.size.y / 2)
            {
                return true;
            }
            return false;
        }
        else
        {
            if (MousePosition.x < transform.position.x + renderer.bounds.size.x / 2 &&
                     MousePosition.x > transform.position.x - renderer.bounds.size.x / 2 &&
                     MousePosition.y < transform.position.y + renderer.bounds.size.y / 2 &&
                     MousePosition.y > transform.position.y - renderer.bounds.size.y / 2)
            {
                return true;
            }
            return false;
        }
    }

    private bool CanDrag()
    {
        if (Board.Cards[gameObject].Flipped)
        {
            return false;
        }
        if ((inRectangle() && Board.WhatIsDragged == null) ||
            Board.Cards.Any(
                c =>
                    c.Value.PreviousCollumn == Board.Cards[gameObject].Collumn && c.Value.IsDragged &&
                    c.Value.Number + c.Value.Type == Board.WhatIsDragged &&
                    c.Value.PreviousPosition.y > gameObject.transform.position.y))
        {
            return true;
        }
        return false;
    }

    private void SetValues()
    {
        Board.Cards[gameObject].PreviousPosition = transform.position;
        Board.Cards[gameObject].XDiference = MousePosition.x - transform.position.x;
        Board.Cards[gameObject].YDiference = MousePosition.y - transform.position.y;
        Board.Cards[gameObject].IsDragged = true;
        Board.Cards[gameObject].PreviousCollumn = Board.Cards[gameObject].Collumn;
        Board.Cards[gameObject].Collumn = 100;
    }
    void Update()
    {
        Board.Cards[gameObject].MoveLocation = new Vector3(transform.position.x, transform.position.y, 0);
        bool LeftButtonPressed = Input.GetMouseButton(0);
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (LeftButtonPressed && Board.Cards[gameObject].IsDragged)
        {
            SetMoveLocation();
        }
        else if (LeftButtonPressed && CanDrag())
        {
            SetValues();
            SetMoveLocation();
            Board.WhatIsDragged = Board.Cards[gameObject].Number + Board.Cards[gameObject].Type;
        }
        else if (Board.Cards[gameObject].IsDragged)
        {
            Drop();
            Board.Cards[gameObject].IsDragged = false;
        }
        else
        {
            if (Board.WhatIsDragged == Board.Cards[gameObject].Number + Board.Cards[gameObject].Type)
            {
                Board.WhatIsDragged = null;
            }
        }
        if (Board.Cards[gameObject].Collumn == 0)
        {
            Board.Cards[gameObject].MoveLocation.z = -Board.Cards[gameObject].PositionOnStart;
        }
        Board.Cards[gameObject].MoveLocation.z += (Board.Cards[gameObject].MoveLocation.y - 20);
        transform.position = Board.Cards[gameObject].MoveLocation;

        if (
            Board.Cards.Where(c => c.Value.Collumn == Board.Cards[gameObject].Collumn)
                .OrderByDescending(c => c.Key.transform.position.y)
                .Last()
                .Key == gameObject && Board.WhatIsDragged == null && Board.Cards[gameObject].Collumn != 0)
            Board.Cards[gameObject].Flipped = false;
        CardRenderer.sprite = Board.Cards[gameObject].Flipped ? BackSprite : CardSprite;
    }
}
