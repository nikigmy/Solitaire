using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 MousePosition;

    public GameObject[] Collumns;
    public GameObject[] StackCollumns;

    void Drop()
    {
        bool dropped = false;
        for (int i = 0; i < Collumns.Length; i++)
        {
            if (transform.position.x < Collumns[i].transform.position.x + Collumns[i].renderer.bounds.size.x/2 &&
                transform.position.x > Collumns[i].transform.position.x - Collumns[i].renderer.bounds.size.x/2 &&
                transform.position.y < Collumns[i].transform.position.y + Collumns[i].renderer.bounds.size.y/2)
            {
                dropped = true;
                Board.Cards[gameObject].MoveLocation = new Vector3(Collumns[i].transform.position.x,
                    transform.position.y - ((Board.Cards.Count(c => c.Value.Collumn == i + 1))*renderer.bounds.size.y/6),
                    0);
                Board.Cards[gameObject].Collumn = i + 1;
            }
        }
        for (int i = 0; i < StackCollumns.Length; i++)
        {
            if (transform.position.x < StackCollumns[i].transform.position.x + StackCollumns[i].renderer.bounds.size.x / 2 &&
                transform.position.x > StackCollumns[i].transform.position.x - StackCollumns[i].renderer.bounds.size.x / 2 &&
                transform.position.y < StackCollumns[i].transform.position.y + StackCollumns[i].renderer.bounds.size.y / 2 &&
                transform.position.y > StackCollumns[i].transform.position.y - StackCollumns[i].renderer.bounds.size.y / 2)
            {
                dropped = true;
                Board.Cards[gameObject].MoveLocation = StackCollumns[i].transform.position;
                Board.Cards[gameObject].Collumn = i + 7;
            }
        }
        Board.Cards[gameObject].Collumn = Board.Cards[gameObject].PreviousCollumn;
        Board.Cards[gameObject].MoveLocation = Board.Cards[gameObject].PreviousPosition;


    }
    void SetMoveLocation()
    {
        Vector3 a = new Vector3(MousePosition.x - Board.Cards[gameObject].xDiference, MousePosition.y - Board.Cards[gameObject].yDiference, -10);
        Board.Cards[gameObject].MoveLocation = a;
    }

    bool inRectangle()
    {
        if (Board.Cards.Any(c =>
            c.Value.Collumn == Board.Cards[gameObject].Collumn &&
            c.Key.transform.position.y < gameObject.transform.position.y))
        {
            if (MousePosition.x < transform.position.x + renderer.bounds.size.x / 2 &&
                MousePosition.x > transform.position.x - renderer.bounds.size.x / 2 &&
                MousePosition.y < transform.position.y + renderer.bounds.size.y / 2 &&
                MousePosition.y > transform.position.y + renderer.bounds.size.y / 3)
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

    private bool CanDrag()
    {
        if (Board.Cards[gameObject].Flipped)
        {
            return false;
        }
        if ((inRectangle() && Board.WhatIsDragged == null) ||
            Board.Cards.Any(
                c =>
                    c.Value.PreviousCollumn == Board.Cards[gameObject].Collumn && c.Value.isDragged &&
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
        Board.Cards[gameObject].xDiference = MousePosition.x - transform.position.x;
        Board.Cards[gameObject].yDiference = MousePosition.y - transform.position.y;
        Board.Cards[gameObject].isDragged = true;
        Board.Cards[gameObject].PreviousCollumn = Board.Cards[gameObject].Collumn;
        Board.Cards[gameObject].Collumn = 100;
    }
    void Update()
    {
        Board.Cards[gameObject].MoveLocation = new Vector3(transform.position.x, transform.position.y, 0);
        bool LeftButtonPressed = Input.GetMouseButton(0);
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (LeftButtonPressed && Board.Cards[gameObject].isDragged)
        {
            SetMoveLocation();
        }
        else if (LeftButtonPressed && CanDrag())
        {
            SetValues();
            SetMoveLocation();
            Board.WhatIsDragged = Board.Cards[gameObject].Number + Board.Cards[gameObject].Type;
        }
        else if (Board.Cards[gameObject].isDragged)
        {
            Drop();
            Board.Cards[gameObject].isDragged = false;
        }
        else
        {
            if (Board.WhatIsDragged == Board.Cards[gameObject].Number + Board.Cards[gameObject].Type)
            {
                Board.WhatIsDragged = null;
            }
        }
        Board.Cards[gameObject].MoveLocation.z += -Board.Cards[gameObject].MoveLocation.y - 20;
        transform.position = Board.Cards[gameObject].MoveLocation;
    }
}
