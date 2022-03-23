using System;
using System.Collections;
using System.Collections.Generic;
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
    private CheckConnections connectionChecker;
    bool validConnection;

    bool mainConnectionsList;
    bool switchedConnectionsList;


    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        dragDirection = Vector2.zero;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (tile.isRunning || tile.tileTypes == TileTypes.Frozen_Tile || CheckConnections.autoConnectionRunning)
            return;

        dragDirection += eventData.delta / canvasScale;

        if (dragDirection.magnitude > dragLimit)
        {
            if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
            {
                if (dragDirection.x < 0 && tile.xGridPos > 0)
                {
                    SwapTileHorizontal(-1);

                }
                else if (dragDirection.x > 0 && (tile.xGridPos < gridGenerator.GridDimensions.x - 1))
                {
                    SwapTileHorizontal(1);
                }
            }
            else
            {
                if (dragDirection.y < 0 && tile.yGridPos < gridGenerator.GridDimensions.y - 1 )
                {
                    SwapTileVertical(1);
                }
                else if (dragDirection.y > 0 && tile.yGridPos > 0)
                {
                    SwapTileVertical(-1);
                }
            }
            dragDirection = Vector2.zero;
        }
    }

    void SwapTileHorizontal(int x)
    {
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
        }
    }

    void SwapTileVertical(int y)
    {
        TileColor startColor = tile.tileColor;
        TileColor switchColor = gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y].GetComponent<Tile>().tileColor;
        gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + y].GetComponent<Tile>().yGridPos = tile.yGridPos;
        tile.yGridPos+= y;
        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + (y * -1)]);
        
        mainConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);
        switchedConnectionsList = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos + (y * -1), switchColor);

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
        }
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
    }

    void SwapElements(ref GameObject g1, ref GameObject g2)
    {
        GameObject temp = g1;
        g1 = g2;
        g2 = temp;
    }
}
