using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public GridGenerator tileGrid;
    CheckConnections connectionChecker;
    Connect3UIController uIController;
    public Sprite blueTileImage, redTileImage, orangeTileImage, greenTileImage, yellowTileImage;
    public Sprite blueBombImage, redBombImage, orangeBombImage, greenBombImage, yellowBombImage;
    public GameObject frozenTile;
    public float chanceOfSpawningBombOnHard = 0.05f;
    public int numFrozenTiles;
    public int numBombsOnHard = 4;
    public int minBombCount = 9;
    public int maxBombCount = 14;
    public static bool tilesMoving = false;

    public List<TileColor> colorList;
    public List<GameObject> bombTileList;

    public bool debugStuff = false;

    // Start is called before the first frame update
    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
        connectionChecker = GetComponent<CheckConnections>();
        numFrozenTiles = tileGrid.GridDimensions.x;
        colorList = new List<TileColor>();
        colorList.Add(TileColor.Blue_Tile);
        colorList.Add(TileColor.Orange_Tile);
        colorList.Add(TileColor.Green_Tile);
        colorList.Add(TileColor.Yellow_Tile);
        colorList.Add(TileColor.Red_Tile);
        uIController = FindObjectOfType<Connect3UIController>();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        PopulateTilesInGrid(Connect3UIController.difficultyType);
    }

    public void PopulateTilesInGrid(DifficultyTypes difficulty)
    {
        switch (difficulty)
        {
            case DifficultyTypes.Easy:
                PopulateTileEasy();
                break;
            case DifficultyTypes.Medium:
                PopulateTileMedium();
                break;
            case DifficultyTypes.Hard:
                PopulateTileHard();
                break;
        }
    }

    void PopulateTileEasy()
    {
        for (int col = tileGrid.GridDimensions.y - 1; col >= 0; col--)
        {
            for (int row = tileGrid.GridDimensions.x - 1; row >= 0; row--)
            {
                List<TileColor> tempList = new List<TileColor>(colorList);
                if (row +1 <= tileGrid.GridDimensions.x - 1)
                {
                    tempList.Remove(tileGrid.tileArray[row + 1, col].GetComponent<Tile>().tileColor);
                }
                if (col + 1 <= tileGrid.GridDimensions.y -1)
                {
                    tempList.Remove(tileGrid.tileArray[row, col+1].GetComponent<Tile>().tileColor);
                }
                int ran = Random.Range(0, tempList.Count);
                TileColor tempTile = tempList[ran];
                tileGrid.tileArray[row, col].GetComponent<Image>().sprite = GetNormalTileSprite(tempTile);
                tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor = tempTile;
                tileGrid.tileArray[row, col].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
                tileGrid.tileArray[row, col].transform.GetChild(0).gameObject.SetActive(false);
                tileGrid.tileArray[row, col].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        while (!connectionChecker.IsMoveAvailable())
        {
            PopulateTileEasy();
        }
        int counter = 0;
    }
    void PopulateTileMedium()
    {
        PopulateTileEasy();
        int ran = Random.Range(2, tileGrid.GridDimensions.y);
        for (int i = 0; i < numFrozenTiles; i++)
        {
            tileGrid.tileArray[i, ran].transform.GetChild(0).gameObject.SetActive(true);
            tileGrid.tileArray[i, ran].transform.GetChild(0).GetComponent<Image>().enabled = true;
            tileGrid.tileArray[i, ran].GetComponent<Tile>().tileTypes = TileTypes.Frozen_Tile;
        }
        while (!connectionChecker.IsMoveAvailable())
        {
            print("Reorganizing tiles");
            connectionChecker.IsMoveAvailable();
            PopulateTileMedium();
        }
    }

    void PopulateTileHard()
    {
        PopulateTileEasy();
        int ran = Random.Range(2, tileGrid.GridDimensions.y - 1);
        for (int i = 0; i < tileGrid.GridDimensions.x; i++)
        {
            if (i % 2 == 0)
                ran -= 1;
            else
                ran += 1;

            int bombCount = Random.Range(minBombCount, maxBombCount + 1);
            tileGrid.tileArray[i, ran].GetComponent<Tile>().bombCount = bombCount;
            GameObject child = tileGrid.tileArray[i, ran].transform.GetChild(1).gameObject;
            child.SetActive(true);
            child.GetComponent<TextMeshProUGUI>().enabled = true;
            child.GetComponent<TextMeshProUGUI>().text = bombCount.ToString();
            tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = GetBombTileSprite(tileGrid.tileArray[i, ran].GetComponent<Tile>().tileColor);
            tileGrid.tileArray[i, ran].GetComponent<Tile>().tileTypes = TileTypes.Bomb_Tile;
            bombTileList.Add(tileGrid.tileArray[i, ran]);
        }

        while (!connectionChecker.IsMoveAvailable())
        {
            PopulateTileHard();
        }
    }
    Sprite GetNormalTileSprite(TileColor color)
    {
        switch (color)
        {
            case TileColor.Blue_Tile:
                return blueTileImage;
            case TileColor.Red_Tile:
                return redTileImage;
            case TileColor.Green_Tile:
                return greenTileImage;
            case TileColor.Orange_Tile:
                return orangeTileImage;
            case TileColor.Yellow_Tile:
                return yellowTileImage;
            default:
                return blueTileImage;
        }
    }

    Sprite GetBombTileSprite(TileColor color)
    {
        switch (color)
        {
            case TileColor.Blue_Tile:
                return blueBombImage;
            case TileColor.Red_Tile:
                return redBombImage;
            case TileColor.Green_Tile:
                return greenBombImage;
            case TileColor.Orange_Tile:
                return orangeBombImage;
            case TileColor.Yellow_Tile:
                return yellowBombImage;

            default:
                return blueBombImage;
        }
    }

    public void RemoveAndAddToTop(int x, int y, int yOffset, bool isVertical, int yMin, int verticalMovementAmount, bool twoVert)
    {
        StartCoroutine(AddToTop(x, y, yOffset, isVertical, yMin, verticalMovementAmount, twoVert));
    }

    IEnumerator AddToTop(int x, int y, int yOffset, bool isVertical, int yMin, int verticalMovementAmount, bool twoVert)
    {
        yield return new WaitForSeconds(0.5f);


        int ran = Random.Range(0, colorList.Count);
        TileColor tempTile = colorList[ran];

        if (Connect3UIController.difficultyType == DifficultyTypes.Hard)
        {
            float randomNum = Random.Range(0.0f, 1.0f);
            if (randomNum <= chanceOfSpawningBombOnHard)
            {
                CreateBombTile(x, y, tempTile, yOffset);
            }
            else
            {
                CreateNormalTile(x, y, tempTile, yOffset);
            }
        }
        else
        {
            CreateNormalTile(x, y, tempTile, yOffset);
        }

        int counter = y;


        //if (twoVert)
        //{
        //    yMin++;
        //    print("Double vert connection");
        //}

        if (!isVertical)
        {
            while (counter > 0)
            {
                tileGrid.tileArray[x, counter].GetComponent<Tile>().ShiftTileDown();
                counter--;
               yield return null;
            }
        }
        else
        {
            while (counter > yMin)
            {
                tileGrid.tileArray[x, counter].GetComponent<Tile>().ShiftTileDown();
                counter--;
                yield return null;
            }
        }

        counter = y;
        while (counter >= 0)
        {
            tileGrid.tileArray[x, counter].GetComponent<Tile>().MoveDownLoop(verticalMovementAmount);
            counter--;
            yield return null;
        }
        connectionChecker.StartConnectionCheckRoutine();
    }

    public bool HasGridStoppedMoving()
    {
        for (int col = 0; col < tileGrid.GridDimensions.y; col++)
        {
            for (int row = 0; row < tileGrid.GridDimensions.x; row++)
            {
                Tile tile = tileGrid.tileArray[row, col].GetComponent<Tile>();
                if (tile.isMoving || tile.isRunning)
                    return false;
            }
        }

        return true;
    }

    public void UpdateBombCount()
    {
        foreach (GameObject bomb in bombTileList)
        {
            int count = int.Parse(bomb.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            count--;
            bomb.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = count.ToString();
        }
    }

    public void DestroyBomb(int x, int y)
    {
        bombTileList.Remove(tileGrid.tileArray[x, y]);
        tileGrid.tileArray[x, y].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
        tileGrid.tileArray[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
    }

    void CreateNormalTile(int x, int y, TileColor color, float offset)
    {
        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = GetNormalTileSprite(color);
        tileGrid.tileArray[x, y].GetComponent<Tile>().tileColor = color;
        tileGrid.tileArray[x, y].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;
        tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition.x, tileGrid.spacing + tileGrid.spacing * offset);
    }
    void CreateBombTile(int x, int y, TileColor color, float offset)
    {
        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = GetBombTileSprite(color);
        tileGrid.tileArray[x, y].GetComponent<Tile>().tileColor = color;
        tileGrid.tileArray[x, y].GetComponent<Tile>().tileTypes = TileTypes.Bomb_Tile;
        tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition.x, tileGrid.spacing + tileGrid.spacing * offset);
        tileGrid.tileArray[x, y].transform.GetChild(1).gameObject.SetActive(true);
        tileGrid.tileArray[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
        tileGrid.tileArray[x, y].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Random.Range(minBombCount, maxBombCount + 1).ToString();
        bombTileList.Add(tileGrid.tileArray[x, y]);
    }
}
