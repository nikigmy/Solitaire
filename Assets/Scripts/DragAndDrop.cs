using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 MousePosition;


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
            SetMoveLocation();
            Board.Cards[gameObject].isDragged = false;
            Board.Cards[gameObject].isDragged = false;
            Board.Cards[gameObject].Collumn = Board.Cards[gameObject].PreviousCollumn;
        }
        else
        {
            if (Board.WhatIsDragged == Board.Cards[gameObject].Number + Board.Cards[gameObject].Type)
            {
                Board.WhatIsDragged = null;
            }
        }
        transform.position = Board.Cards[gameObject].MoveLocation;
    }
}
