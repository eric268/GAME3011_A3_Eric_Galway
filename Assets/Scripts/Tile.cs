using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileTypes tileTypes;
    public TileColor tileColor;
    public int xGridPos;
    public int yGridPos;
    public float spacing;
    private float movementSpeed;
    public bool isRunning = false;

    public Vector2 startingPosition;

    private void Start()
    {
        movementSpeed = 50.0f;
        spacing = transform.parent.GetComponent<TileGridSlotGenerator>().spacing;
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
    }
    

    public void MoveLeft()
    {
        if (!isRunning)
            StartCoroutine(MoveTileLeft());
    }

    public void MoveRight()
    {
        if (!isRunning)
            StartCoroutine(MoveTileRight());
    }

    public void MoveUp()
    {
        if (!isRunning)
            StartCoroutine(MoveTileUp());
    }

    public void MoveDown()
    {
        if (!isRunning)
            StartCoroutine(MoveTileDown());
    }

    IEnumerator MoveTileLeft()
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.x > (startingPosition.x - spacing))
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x - movementSpeed * Time.deltaTime, GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }

        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x - spacing, startingPosition.y);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        isRunning = false;
    }
    IEnumerator MoveTileRight()
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.x < startingPosition.x + spacing)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x + movementSpeed * Time.deltaTime, GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x + spacing, startingPosition.y);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        isRunning = false;
    }

    IEnumerator MoveTileUp()
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.y < startingPosition.y + spacing)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y + movementSpeed * Time.deltaTime);
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x, startingPosition.y + spacing);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        isRunning = false;
    }

    IEnumerator MoveTileDown()
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.y > startingPosition.y - spacing)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y - movementSpeed * Time.deltaTime);
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x, startingPosition.y - spacing);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        isRunning = false;
    }
}
