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
    TileGridSlotGenerator gridGenerator;



   
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
        if (tile.isRunning)
            return;

        dragDirection += eventData.delta / canvasScale;

        if (dragDirection.magnitude > dragLimit)
        {
            if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
            {
                if (dragDirection.x < 0 && tile.xGridPos > 0)
                {
                    tile.MoveLeft();
                    gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().MoveRight();
                    gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                    tile.xGridPos--;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos]);
                }
                else if (dragDirection.x > 0 && (tile.xGridPos < gridGenerator.GridDimensions.x - 1))
                {
                    tile.MoveRight();
                    gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().MoveLeft();
                    gridGenerator.tileArray[tile.xGridPos + 1, tile.yGridPos].GetComponent<Tile>().xGridPos = tile.xGridPos;
                    tile.xGridPos++;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos - 1, tile.yGridPos]);
                }
            }
            else
            {
                if (dragDirection.y < 0 && tile.yGridPos < gridGenerator.GridDimensions.y - 1 )
                {
                    tile.MoveDown();
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().MoveUp();
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                    tile.yGridPos++;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1]);
                }
                else if (dragDirection.y > 0 && tile.yGridPos > 0)
                {
                    tile.MoveUp();
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().MoveDown();
                    gridGenerator.tileArray[tile.xGridPos, tile.yGridPos - 1].GetComponent<Tile>().yGridPos = tile.yGridPos;
                    tile.yGridPos--;
                    SwapElements(ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos], ref gridGenerator.tileArray[tile.xGridPos, tile.yGridPos + 1]);
                }
            }
            dragDirection = Vector2.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = transform.parent.GetComponent<TileGridSlotGenerator>();
        dragLimit = 3.0f;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasScale = canvas.GetComponent<Canvas>().scaleFactor;
        tile = GetComponent<Tile>();
        dragDirection = Vector2.zero;
    }

    void SwapElements(ref GameObject g1, ref GameObject g2)
    {
        GameObject temp = g1;
        g1 = g2;
        g2 = temp;
    }
}
