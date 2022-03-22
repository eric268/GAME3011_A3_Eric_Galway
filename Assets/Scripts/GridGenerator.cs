using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject tileSlotPrefab;
    int rowCounter = 1;
    int columnCounter = 1;
    private Vector2 startingPos;
    public float spacing = 50.0f;
    [SerializeField]
    public Vector2Int GridDimensions = new Vector2Int(6, 6);
    public GameObject [,] tileArray;

    [SerializeField]
    private TileTypes m_tileType;

    private void Awake()
    {
        startingPos = new Vector2(transform.position.x, transform.position.y);
        tileArray = new GameObject[GridDimensions.x, GridDimensions.y];
        int numCells = GridDimensions.x * GridDimensions.y;

        while (transform.childCount < numCells)
        {
            //Provides information to tiles
            GameObject newObject = Instantiate(tileSlotPrefab, this.transform);
            newObject.GetComponent<Tile>().tileTypes = m_tileType;
            newObject.name = "Tile x: " + (rowCounter -1) + " y: " + (columnCounter -1);
            newObject.GetComponent<Tile>().xGridPos = rowCounter -1;
            newObject.GetComponent<Tile>().yGridPos = columnCounter -1;
            tileArray[rowCounter - 1, columnCounter - 1] = newObject;

            //newObject.transform.localPosition = new Vector3((rowCounter - 1) * spacing, (columnCounter - 1) * spacing * -1, 1);
            newObject.GetComponent<RectTransform>().anchoredPosition = new Vector2((rowCounter - 1) * spacing, (columnCounter - 1) * spacing * -1);

            rowCounter++;
            if (rowCounter > GridDimensions.x)
            {
                rowCounter = 1;
                columnCounter++;
            }
        }
    }
}
