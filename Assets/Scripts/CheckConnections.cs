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
    // Start is called before the first frame update
    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
    }

    public bool DoesConnectionExist(int x, int y, TileColor color)
    {
        List<ConnectionTypes> connections = CheckNewTilePositionForMatch(x, y, color);

        if (connections.Count > 0)
            return true;

        return false;
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

                List<ConnectionTypes> connections = CheckNewTilePositionForMatch(row, col, rightColor);
                List<ConnectionTypes> connections2 = CheckNewTilePositionForMatch(row, col, downColor);
                List<ConnectionTypes> connections3 = CheckNewTilePositionForMatch(row + 1, col, baseColor);
                List<ConnectionTypes> connections4 = CheckNewTilePositionForMatch(row, col + 1, baseColor);

                if (connections.Count > 0 || connections2.Count > 0 || connections3.Count > 0 || connections4.Count > 0)
                    return true;
            }
        }

        return false;
    }



    public List<ConnectionTypes> CheckNewTilePositionForMatch(int x, int y, TileColor c)
    {
        List<ConnectionTypes> connectionTypes = new List<ConnectionTypes>();
        TileColor color = c;

        if ((x - 1) >= 0 && (x + 1) < tileGrid.GridDimensions.x)
        {
            TileColor closeLeft = tileGrid.tileArray[x - 1, y].GetComponent<Tile>().tileColor;
            TileColor closeRight = tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileColor;
            if (closeLeft == color && color == closeRight)
                connectionTypes.Add(ConnectionTypes.Close_Horizontal);
        }
        if ((x - 2) >= 0)
        {
            TileColor closeLeft = tileGrid.tileArray[x - 1, y].GetComponent<Tile>().tileColor;
            TileColor farLeft = tileGrid.tileArray[x - 2, y].GetComponent<Tile>().tileColor;
            if (closeLeft == color && color == farLeft)
                connectionTypes.Add(ConnectionTypes.Far_Left);
        }
        if ((x + 2) < tileGrid.GridDimensions.x)
        {
            TileColor farRight = tileGrid.tileArray[x + 2, y].GetComponent<Tile>().tileColor;
            TileColor closeRight = tileGrid.tileArray[x + 1, y].GetComponent<Tile>().tileColor;
            if (closeRight == color && color == farRight)
                connectionTypes.Add(ConnectionTypes.Far_Right);
        }
        if ((y - 1) >= 0 && (y + 1) < tileGrid.GridDimensions.y)
        {
            TileColor closeUp = tileGrid.tileArray[x, y - 1].GetComponent<Tile>().tileColor;
            TileColor closeDown = tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileColor;
            if (closeUp == color && closeDown == color)
            {
                connectionTypes.Add(ConnectionTypes.Close_Vertical);
            }
        }
        if ((y - 2) >= 0)
        {
            TileColor farUp = tileGrid.tileArray[x, y - 2].GetComponent<Tile>().tileColor;
            TileColor closeUp = tileGrid.tileArray[x, y - 1].GetComponent<Tile>().tileColor;

            if (closeUp == color && farUp == color)
            {
                connectionTypes.Add(ConnectionTypes.Far_Up);
            }
        }
        if ((y + 2) < tileGrid.GridDimensions.y)
        {
            TileColor farDown = tileGrid.tileArray[x, y + 2].GetComponent<Tile>().tileColor;
            TileColor closeDown = tileGrid.tileArray[x, y + 1].GetComponent<Tile>().tileColor;

            if (farDown == color && closeDown == color)
            {
                connectionTypes.Add(ConnectionTypes.Far_Down);
            }
        }

        return connectionTypes;
    }
}
