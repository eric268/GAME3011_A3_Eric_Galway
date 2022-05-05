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
    public bool canMakeMove = true;
    public static bool doubleVerticalConnection = false;
    // Start is called before the first frame update

    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
        gridManager = GetComponent<GridManager>();
        uiController = FindObjectOfType<Connect3UIController>();
    }

    public bool DoesConnectionExist(int x, int y, TileColor color)
    {
        bool connections = CheckNewTilePositionForMatch(x, y);
        return connections;
    }

    public bool IsMoveAvailable()
    {
        if (tileGrid == null)
            tileGrid = GetComponent<GridGenerator>();

        for (int col = 0; col < tileGrid.GridDimensions.y - 1; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x - 1; row++)
            {
                if (tileGrid.tileArray[row, col].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile || tileGrid.tileArray[row + 1, col].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
                    continue;

                TileColor baseColor = tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor;
                
            //Move to right
                //Up/Down
                if (col -1 >= 0)
                {
                    Tile u1 = tileGrid.tileArray[row + 1, col - 1].GetComponent<Tile>();
                    Tile d1 = tileGrid.tileArray[row + 1, col + 1].GetComponent<Tile>();

                    if (u1.tileTypes != TileTypes.Frozen_Tile && d1.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (baseColor == u1.tileColor && baseColor == d1.tileColor)
                            return true;
                    }
                }
                //Far Right
                if (row + 3 < tileGrid.GridDimensions.x)
                {
                    Tile r1 = tileGrid.tileArray[row + 2, col].GetComponent<Tile>();
                    Tile r2 = tileGrid.tileArray[row + 3, col].GetComponent<Tile>();

                    if (r1.tileTypes != TileTypes.Frozen_Tile && r2.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (baseColor == r1.tileColor && baseColor == r2.tileColor)
                            return true;
                    }
                }
            //Moved left
                TileColor rightColor = tileGrid.tileArray[row + 1, col].GetComponent<Tile>().tileColor;
                if (col - 1 >= 0)
                {
                    Tile u1 = tileGrid.tileArray[row, col - 1].GetComponent<Tile>();
                    Tile d1 = tileGrid.tileArray[row, col + 1].GetComponent<Tile>();
                    
                    if (u1.tileTypes != TileTypes.Frozen_Tile && d1.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (rightColor == u1.tileColor && rightColor == d1.tileColor)
                            return true;
                    }
                }
                //Far Right
                if (row - 2 >= 0)
                {
                    Tile l1 = tileGrid.tileArray[row - 1, col].GetComponent<Tile>();
                    Tile l2 = tileGrid.tileArray[row - 2, col].GetComponent<Tile>();
                    
                    if (l1.tileTypes != TileTypes.Frozen_Tile && l2.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (rightColor == l1.tileColor && rightColor == l2.tileColor)
                            return true;
                    }
                }
                //Move down
                if (tileGrid.tileArray[row, col + 1].GetComponent<Tile>().tileTypes == TileTypes.Frozen_Tile)
                    continue;
                //FarDown
                if (col + 3 < tileGrid.GridDimensions.y)
                {
                    Tile d1 = tileGrid.tileArray[row, col + 2].GetComponent<Tile>();
                    Tile d2 = tileGrid.tileArray[row, col + 3].GetComponent<Tile>();

                    if (d1.tileTypes != TileTypes.Frozen_Tile && d2.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (baseColor == d1.tileColor && baseColor == d2.tileColor)
                            return true;
                    }
                }
                //Left/Right
                if (row - 1 > 0)
                {
                    Tile l1 = tileGrid.tileArray[row - 1, col + 1].GetComponent<Tile>();
                    Tile r1 = tileGrid.tileArray[row + 1, col + 1].GetComponent<Tile>();

                    if (l1.tileTypes != TileTypes.Frozen_Tile && r1.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (baseColor == l1.tileColor && baseColor == r1.tileColor)
                            return true;
                    }
                }
            //Move Up
                TileColor downColor = tileGrid.tileArray[row, col + 1].GetComponent<Tile>().tileColor;
                //FarUp
                if (col - 2 >= 0)
                {
                    Tile u1 = tileGrid.tileArray[row, col - 1].GetComponent<Tile>();
                    Tile u2 = tileGrid.tileArray[row, col - 2].GetComponent<Tile>();

                    if (u1.tileTypes != TileTypes.Frozen_Tile && u2.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (downColor == u1.tileColor && downColor == u2.tileColor)
                            return true;
                    }
                }
                //left/right
                if (row - 1 >= 0)
                {
                    Tile l1 = tileGrid.tileArray[row - 1, col].GetComponent<Tile>();
                    Tile r1 = tileGrid.tileArray[row + 1, col].GetComponent<Tile>();

                    if (l1.tileTypes != TileTypes.Frozen_Tile && r1.tileTypes != TileTypes.Frozen_Tile)
                    {
                        if (downColor == l1.tileColor && downColor == r1.tileColor)
                            return true;
                    }
                }
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
        canMakeMove = false;
        autoConnectionRunning = true;
        yield return new WaitForSeconds(0.85f);
        for (int col = 0; col < tileGrid.GridDimensions.y; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x; row++)
            {
                Tile tile = tileGrid.tileArray[row, col].GetComponent<Tile>();
                TileColor baseColor = tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor;
                if (!tile.isMoving && !tile.isRunning && CheckNewTilePositionForMatch(row, col))
                {
                    autoConnectionRunning = false;
                    yield break;
                }
            }
        }
        autoConnectionRunning = false;
        if (Connect3Manager.currentTileDestroyed >= Connect3Manager.tilesToWin)
        {
            Connect3Manager.gameWon = true;
            uiController.SetGameOverText(true);
        }
        canMakeMove = true;

        print(IsMoveAvailable());
    }



    public bool CheckNewTilePositionForMatch(int x, int y)
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
                    gridManager.RemoveAndAddToTop(x, i, verticalInfo.totalTilesConnected - counter - 1, true, counter, verticalInfo.totalTilesConnected,false);
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
            int yOffset = 0;
            if (doubleVerticalConnection)
                yOffset = 1;

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
                    gridManager.RemoveAndAddToTop(i, y, -yOffset, false, 0, 1, doubleVerticalConnection);
                    tileGrid.tileArray[i, y].GetComponent<Tile>().isMoving = true;
                }
            }
            doubleVerticalConnection = true;
        }

        if (!horizontalConnectionFound)
            doubleVerticalConnection = false;

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
        if (!Connect3Manager.gameLost)
        {
            Connect3Manager.score += 5 * info.totalTilesConnected * info.totalTilesConnected;
            Connect3Manager.currentTileDestroyed += info.totalTilesConnected;
            uiController.SetScoreText();
            uiController.SetTilesDestroyedText();
        }
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



