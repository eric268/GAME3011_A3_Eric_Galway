using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject canvas;
    private Tile tile;
    private float canvasScale;
    private Vector2 dragDirection;
    private float dragLimit;
    public GameObject selectedGameObject;
    GridGenerator gridGenerator;
    GridManager gridManager;
    private CheckConnections connectionChecker;
    bool validConnection;

    bool mainConnectionsList;
    bool switchedConnectionsList;


    public void OnBeginDrag(PointerEventData eventData)
    {
        dragDirection = Vector2.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragDirection = Vector2.zero;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!gridManager.HasGridStoppedMoving() || tile.tileTypes == TileTypes.Frozen_Tile || !connectionChecker.canMakeMove ||
            Connect3Manager.gameWon == true || Connect3Manager.gameLost == true)
            return;

        dragDirection += eventData.delta / canvasScale;

        if (dragDirection.magnitude > dragLimit)
        {
            if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
            {
                if (dragDirection.x < 0 && tile.xGridPos > 0)
                {
                    if (SwapTileHorizontal(-1))
                    {
                        gridManager.UpdateBombCount();
                    }

                }
                else if (dragDirection.x > 0 && (tile.xGridPos < gridGenerator.GridDimensions.x - 1))
                {
                    if (SwapTileHorizontal(1))
                    {
                        gridManager.UpdateBombCount();
                    }
                }
            }
            else
            {
                if (dragDirection.y < 0 && tile.yGridPos < gridGenerator.GridDimensions.y - 1)
                {
                    if (SwapTileVertical(1))
                    {
                        gridManager.UpdateBombCount();
                    }

                }
                else if (dragDirection.y > 0 && tile.yGridPos > 0)
                {
                    if (SwapTileVertical(-1))
                    {
                        gridManager.UpdateBombCount();
                    }
                }
            }
            dragDirection = Vector2.zero;
        }
    }

    bool SwapTileHorizontal(int x)
    {
        if (gridGenerator.tileArray[tile.xGridPos + x, tile.yGridPos].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            return false;

        TileColor startColor = tile.tileColor;
        TileColor switchColor = gridGenerator.tileArray[tile.xGridPos + x, tile.yGridPos].GetComponent<Tile>().tileColor;
        gridGenerator.tileArray[tile.xGridPos + x, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
        tile.xGridPos += x;
        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos + (x * -1), tile.yGridPos]);

        mainConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);

        switchedConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos + (x * -1), tile.yGridPos, switchColor);

        if (mainConnectionsList || switchedConnectionsList)
        {
            validConnection = true;
        }
        else
        {
            validConnection = false;
        }

        if (x < 0)
        {
            tile.MoveLeft(validConnection);
            gridGenerator.tileArray[tile.xGridPos + (x * -1), tile.yGridPos].GetComponent<Tile>().MoveRight(validConnection);
        }
        else
        {
            tile.MoveRight(validConnection);
            gridGenerator.tileArray[tile.xGridPos + (x * -1), tile.yGridPos].GetComponent<Tile>().MoveLeft(validConnection);
        }
        
        if (!validConnection)
        {
            gridGenerator.tileArray[tile.xGridPos + (x * -1), tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
            tile.xGridPos += (x * -1);
            SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos + x, tile.yGridPos]);
            return false;
        }
        return true;
    }

    bool SwapTileVertical(int y)
    {
        if (gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            return false;

        TileColor startColor = tile.tileColor;
        TileColor switchColor = gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y].GetComponent<Tile>().tileColor;
        gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y].GetComponent<Tile>().yGridPos = tile.yGridPos;
        tile.yGridPos+= y;
        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + (y * -1)]);

        switchedConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos + (y * -1), switchColor);
        mainConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);


        if (mainConnectionsList || switchedConnectionsList)
            validConnection = true;
        else
            validConnection = false;

        if (y > 0)
        {
            tile.MoveDown(validConnection);
            gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + (y * -1)].GetComponent<Tile>().MoveUp(validConnection);
        }
        else
        {
            tile.MoveUp(validConnection);
            gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + (y * -1)].GetComponent<Tile>().MoveDown(validConnection);
        }


        if (!validConnection)
        {
            gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + (y * -1)].GetComponent<Tile>().yGridPos = tile.yGridPos;
            tile.yGridPos += (y * -1);
            SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y]);
            return false;
        }
        return true;

    }

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = GetComponentInParent<GridGenerator>();
        dragLimit = 3.0f;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasScale = canvas.GetComponent<Canvas>().scaleFactor;
        tile = GetComponent<Tile>();
        dragDirection = Vector2.zero;
        connectionChecker = GetComponentInParent<CheckConnections>();
        gridManager = GetComponentInParent<GridManager>();
    }

    void SwapElements(ref GameObject g1, ref GameObject g2)
    {
        GameObject temp = g1;
        g1 = g2;
        g2 = temp;
    }
}
