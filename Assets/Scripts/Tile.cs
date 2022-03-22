using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public CheckConnections connectionChecker;
    public GridManager gridManager;
    public TileTypes tileTypes;
    public TileColor tileColor;
    public int xGridPos;
    public int yGridPos;
    public float spacing;
    private float movementSpeed;
    public bool isRunning = false;
    public int bombCount;
    public Vector2 startingPosition;

    private void Start()
    {
        connectionChecker = GetComponent<CheckConnections>();
        gridManager = GetComponentInParent<GridManager>();
        movementSpeed = 50.0f;
        spacing = transform.parent.GetComponent<GridGenerator>().spacing;
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
    }
    

    public void MoveLeft(bool hasConnection)
    {
        
        if (!isRunning)
            StartCoroutine(MoveTileLeft(hasConnection));
    }

    public void MoveRight(bool hasConnection)
    {
        if (!isRunning)
            StartCoroutine(MoveTileRight(hasConnection));
    }

    public void MoveUp(bool hasConnection)
    {
        if (!isRunning)
            StartCoroutine(MoveTileUp(hasConnection));
    }

    public void MoveDown(bool hasConnection)
    {
        if (!isRunning)
            StartCoroutine(MoveTileDown(hasConnection));
    }

    IEnumerator MoveTileLeft(bool hasConnection)
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.x > (startingPosition.x - spacing))
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x - movementSpeed * Time.deltaTime, GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }
        isRunning = false;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x - spacing, startingPosition.y);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        
        if (!hasConnection)
        {
            StartCoroutine(MoveTileRight(true));
        }

    }

    IEnumerator MoveTileRight(bool hasConnection)
    {
        isRunning = true;
        while (GetComponent<RectTransform>().anchoredPosition.x < startingPosition.x + spacing)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x + movementSpeed * Time.deltaTime, GetComponent<RectTransform>().anchoredPosition.y);
            yield return null;
        }
        isRunning = false;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x + spacing, startingPosition.y);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;

        if (!hasConnection)
        {
            StartCoroutine(MoveTileLeft(true));
        }
    }

    IEnumerator MoveTileUp(bool hasConnection)
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

        if (!hasConnection)
        {
            StartCoroutine(MoveTileDown(true));
        }
    }

    IEnumerator MoveTileDown(bool hasConnection)
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

        if (!hasConnection)
        {
            StartCoroutine(MoveTileUp(true));
        }
    }
}
