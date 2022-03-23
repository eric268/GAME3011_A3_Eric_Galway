using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionTypes
{
    Close_Horizontal,
    Far_Left,
    Far_Right,
    Close_Vertical,
    Far_Up,
    Far_Down,
    Num_Connection_Types,
}

public class CheckConnections : MonoBehaviour
{
    GridGenerator tileGrid;
    GridManager gridManager;
    public static bool autoConnectionRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
        gridManager = GetComponent<GridManager>();
    }

    public bool DoesConnectionExist(int x, int y, TileColor color)
    {
        bool connections = CheckNewTilePositionForMatch(x, y, color);
        return connections;
    }

    public bool IsMoveAvailable()
    {
        for (int col = 0; col < tileGrid.GridDimensions.y - 1; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x - 1; row++)
            {
                TileColor baseColor = tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor;
                TileColor rightColor = tileGrid.tileArray[row + 1, col].GetComponent<Tile>().tileColor;
                TileColor downColor = tileGrid.tileArray[row, col + 1].GetComponent<Tile>().tileColor;

                bool connections =  CheckNewTilePositionForMatch(row, col, rightColor);
                bool connections2 = CheckNewTilePositionForMatch(row, col, downColor);
                bool connections3 = CheckNewTilePositionForMatch(row + 1, col, baseColor);
                bool connections4 = CheckNewTilePositionForMatch(row, col + 1, baseColor);

                if (connections || connections2 || connections3 || connections4)
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
        for (int col = 0; col < tileGrid.GridDimensions.y - 1; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x - 1; row++)
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
        bool foundConnection = false;
        HashSet<Vector2> moves = new HashSet<Vector2>();
        TileColor color = c;

        if ((y - 1) >= 0 && (y + 1) < tileGrid.GridDimensions.y)
        {
            TileColor closeUp = tileGrid.tileArray[x, y - 1].GetComponent<Tile>().tileColor;
            TileColor closeDown = tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileColor;
            if (closeUp == color && closeDown == color)
            {
                if (!moves.Contains(new Vector2(x, y - 1)))
                {
                    moves.Add(new Vector2(x, (y - 1)));
                    gridManager.RemoveAndAddToTop(x, (y - 1), 2, true, 0);
                }
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 1, true, 1);
                }
                if (!moves.Contains(new Vector2(x, y + 1)))
                {
                    moves.Add(new Vector2(x, y + 1));
                    gridManager.RemoveAndAddToTop(x, y + 1, 0, true, 2);
                }
                foundConnection = true;
            }
        }
        if ((y - 2) >= 0)
        {
            TileColor farUp = tileGrid.tileArray[x, y - 2].GetComponent<Tile>().tileColor;
            TileColor closeUp = tileGrid.tileArray[x, y - 1].GetComponent<Tile>().tileColor;

            if (closeUp == color && farUp == color)
            {
                if (!moves.Contains(new Vector2(x, (y - 2))))
                {
                    moves.Add(new Vector2(x, (y - 2)));
                    gridManager.RemoveAndAddToTop(x, (y - 2), 2, true, 0);
                }
                if (!moves.Contains(new Vector2(x, (y - 1))))
                {
                    moves.Add(new Vector2(x, (y - 1)));
                    gridManager.RemoveAndAddToTop(x, (y - 1), 1, true, 1);
                }
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 0, true, 2);
                }

                foundConnection = true;
            }
        }
        if ((y + 2) < tileGrid.GridDimensions.y)
        {
            TileColor farDown = tileGrid.tileArray[x, y + 2].GetComponent<Tile>().tileColor;
            TileColor closeDown = tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileColor;

            if (farDown == color && closeDown == color)
            {
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 2, true, 0);
                }
                if (!moves.Contains(new Vector2(x, (y + 1))))
                {
                    moves.Add(new Vector2(x, (y + 1)));
                    gridManager.RemoveAndAddToTop(x, (y + 1), 1, true, 1);
                }

                if (!moves.Contains(new Vector2(x, (y + 2))))
                {
                    moves.Add(new Vector2(x, (y + 2)));
                    gridManager.RemoveAndAddToTop(x, (y + 2), 0, true, 2);
                }

                foundConnection = true;
            }
        }

        if ((x - 1) >= 0 && (x + 1) < tileGrid.GridDimensions.x)
        {
            TileColor closeLeft = tileGrid.tileArray[x - 1, y].GetComponent<Tile>().tileColor;
            TileColor closeRight = tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileColor;
            if (closeLeft == color && color == closeRight)
            {
                if (!moves.Contains(new Vector2(x - 1, y)))
                {
                    moves.Add(new Vector2(x - 1, y));
                    gridManager.RemoveAndAddToTop(x - 1, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x + 1, y)))
                {
                    moves.Add(new Vector2(x + 1, y));
                    gridManager.RemoveAndAddToTop(x + 1, y, 0, false, 0);
                }
                foundConnection = true;
            }
        }
        if ((x - 2) >= 0)
        {
            TileColor closeLeft = tileGrid.tileArray[x - 1, y].GetComponent<Tile>().tileColor;
            TileColor farLeft = tileGrid.tileArray[x - 2, y].GetComponent<Tile>().tileColor;
            if (closeLeft == color && color == farLeft)
            {
                if (!moves.Contains(new Vector2(x-1,y)))
                {
                    moves.Add(new Vector2(x - 1, y));
                    gridManager.RemoveAndAddToTop(x - 1, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x - 2, y)))
                {
                    moves.Add(new Vector2(x - 2, y));
                    gridManager.RemoveAndAddToTop(x - 2, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 0, false, 0);
                }
                foundConnection = true;
            }

        }
        if ((x + 2) < tileGrid.GridDimensions.x)
        {
            TileColor farRight = tileGrid.tileArray[x + 2, y].GetComponent<Tile>().tileColor;
            TileColor closeRight = tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileColor;
            if (closeRight == color && color == farRight)
            {
                if (!moves.Contains(new Vector2(x + 1, y)))
                {
                    moves.Add(new Vector2(x + 1, y));
                    gridManager.RemoveAndAddToTop(x + 1, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x - 2, y)))
                {
                    moves.Add(new Vector2(x + 2, y));
                    gridManager.RemoveAndAddToTop(x + 2, y, 0, false, 0);
                }
                if (!moves.Contains(new Vector2(x, y)))
                {
                    moves.Add(new Vector2(x, y));
                    gridManager.RemoveAndAddToTop(x, y, 0, false, 0);
                }
                foundConnection = true;
            }
        }

        print("Moves: " + moves.Count);

        return foundConnection;
    }
}
