using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//public enum ConnectionTypes
//{
//    Close_Horizontal,
//    Far_Left,
//    Far_Right,
//    Close_Vertical,
//    Far_Up,
//    Far_Down,
//    Num_Connection_Types,
//}

public class CheckConnections : MonoBehaviour
{
    GridGenerator tileGrid;
    GridManager gridManager;
    Connect3UIController uiController;
    public static bool autoConnectionRunning = false;
    // Start is called before the first frame update

    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
        gridManager = GetComponent<GridManager>();
        uiController = FindObjectOfType<Connect3UIController>();
    }

    public bool DoesConnectionExist(int x, int y, TileColor color)
    {
        bool connections = CheckNewTilePositionForMatch(x, y, color);
        return connections;
    }

    public bool IsMoveAvailable()
    {
        if (tileGrid == null)
            tileGrid = GetComponent<GridGenerator>();

        for (int col = 0; col < tileGrid.GridDimensions.y - 3; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x - 3; row++)
            {
                TileColor baseColor = tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor;
                
                TileColor rightColor = tileGrid.tileArray[row + 2, col].GetComponent<Tile>().tileColor;
                TileColor farRightColor = tileGrid.tileArray[row + 3, col].GetComponent<Tile>().tileColor;
                
                TileColor downColor = tileGrid.tileArray[row, col + 2].GetComponent<Tile>().tileColor;
                TileColor farDownColor = tileGrid.tileArray[row, col + 3].GetComponent<Tile>().tileColor;


                if (baseColor == rightColor && baseColor == farRightColor || baseColor == downColor && baseColor == farDownColor)
                    return true;
                    
            }
        }
        return false;
    }

    public void StartConnectionCheckRoutine()
    {
        if (!autoConnectionRunning)
            StartCoroutine(IsConnectionMade());
    }

    IEnumerator IsConnectionMade()
    {
        autoConnectionRunning = true;
        yield return new WaitForSeconds(1.0f);
        for (int col = 0; col < tileGrid.GridDimensions.y; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x; row++)
            {
                TileColor baseColor = tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor;
                if (CheckNewTilePositionForMatch(row, col, baseColor))
                {
                    autoConnectionRunning = false;
                    yield break;
                }
            }
        }
        autoConnectionRunning = false;
    }



    public bool CheckNewTilePositionForMatch(int x, int y, TileColor c)
    {
        if (tileGrid.tileArray[x, y].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            return false;

        bool verticalConnectionFound = false;
        bool horizontalConnectionFound = false;

        //Vertical Connection Found
        ConnectionInfo verticalInfo = ConnectionFound(false, x);
        if (verticalInfo.totalTilesConnected >= 3)
        {
            UpdateGameStats(verticalInfo);

            verticalConnectionFound = true;
            int counter = 0;
            for (int i = verticalInfo.minPosition; i <= verticalInfo.maxPosition; i++)
            {
                if (!tileGrid.tileArray[x, i].GetComponent<Tile>().isMoving)
                {
                    if (tileGrid.tileArray[x,i].GetComponent<Tile>().tileTypes == TileTypes.Bomb_Tile)
                    {
                        gridManager.DestroyBomb(x, i);
                    }
                    CheckIfFozenTilesNearby(x, i);
                    gridManager.RemoveAndAddToTop(x, i, verticalInfo.totalTilesConnected - counter - 1, true, counter, verticalInfo.totalTilesConnected);
                    tileGrid.tileArray[x, i].GetComponent<Tile>().isMoving = true;
                }
                counter++;
            }
        }

        //Horizontal Connection found
        ConnectionInfo horizontalInfo = ConnectionFound(true, y);

        if (horizontalInfo.totalTilesConnected >= 3)
        {
            UpdateGameStats(horizontalInfo);

            horizontalConnectionFound = true;
            for (int i = horizontalInfo.minPosition; i <= horizontalInfo.maxPosition; i++)
            {
                if (!tileGrid.tileArray[i, y].GetComponent<Tile>().isMoving)
                {
                    if (tileGrid.tileArray[i, y].GetComponent<Tile>().tileTypes == TileTypes.Bomb_Tile)
                    {
                        gridManager.DestroyBomb(i, y);
                    }
                    CheckIfFozenTilesNearby(i, y);
                    gridManager.RemoveAndAddToTop(i, y, 0, false, 0, 1);
                    tileGrid.tileArray[i, y].GetComponent<Tile>().isMoving = true;
                }
            }
        }

        return verticalConnectionFound || horizontalConnectionFound;
    }

    ConnectionInfo ConnectionFound(bool xAxis, int axisPosition)
    {
        int tilesConnectedCounter = 1;
        int totalTilesConnected = 1;
        int minPosition = 0;
        int maxPosition = 0;
        if (xAxis)
        {
            for (int i = 0; i < tileGrid.GridDimensions.x; i++)
            {
                tilesConnectedCounter = 1;
                Tile current = tileGrid.tileArray[i, axisPosition].GetComponent<Tile>();
                TileColor currentColor = current.tileColor;

                for (int j = i +1; j < tileGrid.GridDimensions.x; j++)
                {
                    Tile next = tileGrid.tileArray[j, axisPosition].GetComponent<Tile>();
                    TileColor nextColor = next.tileColor;

                    if (current.tileTypes != TileTypes.Frozen_Tile && next.tileTypes != TileTypes.Frozen_Tile &&
                        currentColor == nextColor)
                    {
                        tilesConnectedCounter++;
                        if (tilesConnectedCounter > totalTilesConnected)
                        {
                            totalTilesConnected = tilesConnectedCounter;
                            minPosition = i;
                            maxPosition = j;
                        }
                    }
                    else
                        break;
                }
            }

        }
        else
        {
            for (int i = 0; i < tileGrid.GridDimensions.y; i++)
            {
                tilesConnectedCounter = 1;
                Tile current = tileGrid.tileArray[axisPosition, i].GetComponent<Tile>();
                TileColor currentColor = current.tileColor;

                for (int j = i + 1; j < tileGrid.GridDimensions.y; j++)
                {
                    Tile next = tileGrid.tileArray[axisPosition, j].GetComponent<Tile>();
                    TileColor nextColor = next.tileColor;

                    if (current.tileTypes != TileTypes.Frozen_Tile && next.tileTypes != TileTypes.Frozen_Tile &&
                        currentColor == nextColor)
                    {
                        tilesConnectedCounter++;
                        if (tilesConnectedCounter > totalTilesConnected)
                        {
                            totalTilesConnected = tilesConnectedCounter;
                            minPosition = i;
                            maxPosition = j;
                        }
                    }
                    else
                        break;
                }
            }
        }
        return new ConnectionInfo(xAxis, totalTilesConnected, minPosition, maxPosition);
    }

    void CheckIfFozenTilesNearby(int x, int y)
    {
        if (x - 1 >= 0)
        {
            if (tileGrid.tileArray[x-1, y].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            {
                tileGrid.tileArray[x - 1, y].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
                tileGrid.tileArray[x - 1, y].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        if (x + 1 < tileGrid.GridDimensions.x)
        {
            if (tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            {
                tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
                tileGrid.tileArray[x + 1, y].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        if (y - 1 > 0)
        {
            if (tileGrid.tileArray[x, y -1].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            {
                tileGrid.tileArray[x, y - 1].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
                tileGrid.tileArray[x, y - 1].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }   
        if (y + 1 < tileGrid.GridDimensions.y)
        {
            if (tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
            {
                tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
                tileGrid.tileArray[x, y + 1].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    void UpdateGameStats(ConnectionInfo info)
    {
        Connect3Manager.score += 10 * info.totalTilesConnected;
        Connect3Manager.currentTileDestroyed += info.totalTilesConnected;
        uiController.SetScoreText();
        uiController.SetTilesDestroyedText();
    }
}

public class ConnectionInfo
{
    public ConnectionInfo(bool xAxis, int total, int min, int max)
    {
        onXAxis = xAxis;
        totalTilesConnected = total;
        minPosition = min;
        maxPosition = max;
    }
    public bool onXAxis;
    public int totalTilesConnected;
    public int minPosition;
    public int maxPosition;
}



