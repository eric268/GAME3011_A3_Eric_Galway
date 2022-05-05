using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tileSlotPrefab;
    int rowCounter = 0;
    int columnCounter = 0;
    private Vector2 startingPos;
    public float spacing = 50.0f;
    [SerializeField]
    public Vector2Int GridDimensions = new Vector2Int(6, 6);
    public GameObject [,] tileArray;

    [SerializeField]
    private TileTypes m_tileType;

    private void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
        tileArray = new GameObject[GridDimensions.x, GridDimensions.y];
        int numCells = GridDimensions.x * GridDimensions.y;

        while (transform.childCount < numCells)
        {
            //Provides information to tiles
            GameObject newObject = Instantiate(tileSlotPrefab, this.transform);
            newObject.GetComponent<Tile>().tileTypes = m_tileType;
            newObject.name = "Tile x: " + (rowCounter) + " y: " + (columnCounter);
            newObject.GetComponent<Tile>().xGridPos = rowCounter;
            newObject.GetComponent<Tile>().yGridPos = columnCounter;
            tileArray[rowCounter, columnCounter] = newObject;

            //newObject.transform.localPosition = new Vector3((rowCounter - 1) * spacing, (columnCounter - 1) * spacing * -1, 1);
            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((rowCounter) * spacing, (columnCounter) * spacing * -1);

            rowCounter++;
            if (rowCounter >= GridDimensions.x)
            {
                rowCounter = 0;
                columnCounter++;
            }
        }
    }

    public void DestroyGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));
        }
    }
}
