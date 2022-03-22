using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public void MoveLeft(bool reverseMovement)
    {
        
        if (!isRunning)
            StartCoroutine(MoveTileLeft(reverseMovement));
    }

    public void MoveRight(bool reverseMovement)
    {
        if (!isRunning)
            StartCoroutine(MoveTileRight(reverseMovement));
    }

    public void MoveUp(bool reverseMovement)
    {
        if (!isRunning)
            StartCoroutine(MoveTileUp(reverseMovement));
    }

    public void MoveDown(bool reverseMovement)
    {
        if (!isRunning)
            StartCoroutine(MoveTileDown(reverseMovement));
    }

    public void MoveDownLoop(int rows)
    {
        if (!isRunning)
            StartCoroutine(MoveTileLoop(rows));
    }

    IEnumerator MoveTileLeft(bool reverseMovement)
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
        
        if (!reverseMovement)
        {
            StartCoroutine(MoveTileRight(true));
        }
    }

    IEnumerator MoveTileRight(bool reverseMovement)
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
        if (!reverseMovement)
        {
            StartCoroutine(MoveTileLeft(true));
        }
    }

    IEnumerator MoveTileUp(bool reverseMovement)
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

        if (!reverseMovement)
        {
            StartCoroutine(MoveTileDown(true));
        }
    }

    IEnumerator MoveTileDown(bool reverseMovement)
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

        if (!reverseMovement)
        {
            StartCoroutine(MoveTileUp(true));
        }
    }

    IEnumerator MoveTileLoop(int rows)
    {
        isRunning = true;
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        while (GetComponent<RectTransform>().anchoredPosition.y > startingPosition.y - spacing * rows)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y - movementSpeed * Time.deltaTime);
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPosition.x, startingPosition.y - spacing * rows);
        startingPosition = GetComponent<RectTransform>().anchoredPosition;
        
        isRunning = false;
    }

    public void ShiftTileDown()
    {
        if (yGridPos > 0)
        {
            gridManager.tileGrid.tileArray[xGridPos, yGridPos -1].GetComponent<Tile>().yGridPos = yGridPos;
            yGridPos--;
            SwapElements(ref gridManager.tileGrid.tileArray[xGridPos, yGridPos], ref gridManager.tileGrid.tileArray[xGridPos, yGridPos + 1]);
        }
    }

    void SwapElements(ref GameObject g1, ref GameObject g2)
    {
        GameObject temp = g1;
        g1 = g2;
        g2 = temp;
    }
}
