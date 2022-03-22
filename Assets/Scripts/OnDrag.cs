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

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Being Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("End Drag");
        dragDirection = Vector2.zero;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (tile.isRunning || tile.tileTypes == TileTypes.Frozen_Tile)
            return;

        dragDirection += eventData.delta / canvasScale;

        if (dragDirection.magnitude > dragLimit)
        {
            bool hasConnection = false;
            if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
            {
                if (dragDirection.x < 0 && tile.xGridPos > 0)
                {
                    TileColor startColor = tile.tileColor;
                    TileColor switchColor = gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().tileColor;
                    gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                    tile.xGridPos--;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos]);
                    hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);

                    if (!hasConnection)
                        hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos + 1, tile.yGridPos, switchColor);

                    tile.MoveLeft(hasConnection);
                    gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().MoveRight(hasConnection);
                    if (!hasConnection)
                    {
                        gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                        tile.xGridPos++;
                        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos]);
                    }

                }
                else if (dragDirection.x > 0 && (tile.xGridPos < gridGenerator.GridDimensions.x - 1))
                {
                    TileColor startColor = tile.tileColor;
                    TileColor switchColor = gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().tileColor;
                    gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                    tile.xGridPos++;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos -1, tile.yGridPos]);
                    
                    hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);

                    if (!hasConnection)
                        hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos -1, tile.yGridPos, switchColor);

                    tile.MoveRight(hasConnection);
                    gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().MoveLeft(hasConnection);

                    if (!hasConnection)
                    {
                        gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                        tile.xGridPos--;
                        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos]);
                    }
                }
            }
            else
            {
                if (dragDirection.y < 0 && tile.yGridPos < gridGenerator.GridDimensions.y - 1 )
                {
                    TileColor startColor = tile.tileColor;
                    TileColor switchColor = gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().tileColor;
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                    tile.yGridPos++;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1]);
                    hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);

                    if (!hasConnection)
                        hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos -1, switchColor);

                    tile.MoveDown(hasConnection);
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().MoveUp(hasConnection);

                    if (!hasConnection)
                    {
                        gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                        tile.yGridPos--;
                        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1]);
                    }
                }
                else if (dragDirection.y > 0 && tile.yGridPos > 0)
                {
                    TileColor startColor = tile.tileColor;
                    TileColor switchColor = gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().tileColor;
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                    tile.yGridPos--;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1]);
                    hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos, startColor);

                    if (!hasConnection)
                        hasConnection = connectionChecker.DoesConnectionExist(tile.xGridPos, tile.yGridPos +1, switchColor);

                    tile.MoveUp(hasConnection);
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().MoveDown(hasConnection);

                    if (!hasConnection)
                    {
                        gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                        tile.yGridPos++;
                        SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1]);
                    }
                }
            }
            dragDirection = Vector2.zero;
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
