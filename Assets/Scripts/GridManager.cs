using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour
{
    public GridGenerator tileGrid;
    public DifficultyTypes difficultyType;

    [Header("Tile Types")]
    public GameObject blueTile, redTile, orangeTile, greenTile, yellowTile;
    public GameObject blueBomb, redBomb, orangeBomb, greenBomb, yellowBomb;
    public GameObject blueFrozen, redFrozen, orangeFrozen, greenFrozen, yellowFrozen;
    public GameObject frozenTile;

    public int numFrozenTiles;
    public int numBombsOnHard = 4;
    public int minBombCount = 9;
    public int maxBombCount = 14;

    public List<TileColor> normalTypeList;
    public List<TileColor> bombTypeList;

    // Start is called before the first frame update
    void Start()
    {
        tileGrid = GetComponent<GridGenerator>();
        numFrozenTiles = tileGrid.GridDimensions.x;
        normalTypeList = new List<TileColor>();
        normalTypeList.Add(TileColor.Blue_Tile);
        normalTypeList.Add(TileColor.Orange_Tile);
        normalTypeList.Add(TileColor.Green_Tile);
        normalTypeList.Add(TileColor.Yellow_Tile);
        normalTypeList.Add(TileColor.Red_Tile);
        PopulateTilesInGrid(difficultyType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateTilesInGrid(DifficultyTypes difficulty)
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
                List<TileColor> tempList = new List<TileColor>(normalTypeList);
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
                switch (tempTile)
                {
                    case TileColor.Blue_Tile:
                        tileGrid.tileArray[row, col].GetComponent<Image>().sprite = blueTile.GetComponent<Image>().sprite;
                        break;
                    case TileColor.Red_Tile:
                        tileGrid.tileArray[row, col].GetComponent<Image>().sprite = redTile.GetComponent<Image>().sprite;
                        break;
                    case TileColor.Green_Tile:
                        tileGrid.tileArray[row, col].GetComponent<Image>().sprite = greenTile.GetComponent<Image>().sprite;
                        break;
                    case TileColor.Orange_Tile:
                        tileGrid.tileArray[row, col].GetComponent<Image>().sprite = orangeTile.GetComponent<Image>().sprite;
                        break;
                    case TileColor.Yellow_Tile:
                        tileGrid.tileArray[row, col].GetComponent<Image>().sprite = yellowTile.GetComponent<Image>().sprite;
                        break;

                }
                tileGrid.tileArray[row, col].GetComponent<Tile>().tileColor = tempTile;
                tileGrid.tileArray[row, col].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;

            }
        }
    }
    void PopulateTileMedium()
    {
        PopulateTileEasy();
        int ran = Random.Range(2, tileGrid.GridDimensions.y);
        for (int i = 0; i < numFrozenTiles; i++)
        {
            tileGrid.tileArray[i, ran].transform.GetChild(0).gameObject.SetActive(true);
            tileGrid.tileArray[i, ran].GetComponent<Tile>().tileTypes = TileTypes.Frozen_Tile;
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
            child.GetComponent<TextMeshProUGUI>().text = bombCount.ToString();

            switch (tileGrid.tileArray[i, ran].GetComponent<Tile>().tileColor)
            {
                case TileColor.Blue_Tile:
                    tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = blueBomb.GetComponent<Image>().sprite;
                    break;
                case TileColor.Red_Tile:
                    tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = redBomb.GetComponent<Image>().sprite;
                    break;
                case TileColor.Green_Tile:
                    tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = greenBomb.GetComponent<Image>().sprite;
                    break;
                case TileColor.Orange_Tile:
                    tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = orangeBomb.GetComponent<Image>().sprite;
                    break;
                case TileColor.Yellow_Tile:
                    tileGrid.tileArray[i, ran].GetComponent<Image>().sprite = yellowBomb.GetComponent<Image>().sprite;
                    break;

            }
            tileGrid.tileArray[i, ran].GetComponent<Tile>().tileTypes = TileTypes.Bomb_Tile;
        }
    }

    public void RemoveAndAddToTop(int x, int y, int yOffset, bool isVertical, int yMin)
    {
        StartCoroutine(AddToTop(x, y, yOffset, isVertical, yMin));
    }

    IEnumerator AddToTop(int x, int y, int yOffset, bool isVertical, int yMin)
    {
        yield return new WaitForSeconds(1.0f);

        //int ran = Random.Range(0, normalTypeList.Count);
        //TileColor tempTile = normalTypeList[ran];
        //switch (tempTile)
        //{
        //    case TileColor.Blue_Tile:
        //        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = blueTile.GetComponent<Image>().sprite;
        //        break;
        //    case TileColor.Red_Tile:
        //        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = redTile.GetComponent<Image>().sprite;
        //        break;
        //    case TileColor.Green_Tile:
        //        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = greenTile.GetComponent<Image>().sprite;
        //        break;
        //    case TileColor.Orange_Tile:
        //        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = orangeTile.GetComponent<Image>().sprite;
        //        break;
        //    case TileColor.Yellow_Tile:
        //        tileGrid.tileArray[x, y].GetComponent<Image>().sprite = yellowTile.GetComponent<Image>().sprite;
        //        break;
        //}

        //tileGrid.tileArray[x, y].GetComponent<Tile>().tileColor = tempTile;
        //tileGrid.tileArray[x, y].GetComponent<Tile>().tileTypes = TileTypes.Normal_Tile;

        tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition = new Vector2(tileGrid.tileArray[x, y].GetComponent<RectTransform>().anchoredPosition.x, tileGrid.spacing + tileGrid.spacing * yOffset);

        GameObject temp = tileGrid.tileArray[x, y];

        int counter = y;

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
            if (isVertical)
            {
                tileGrid.tileArray[x, counter].GetComponent<Tile>().MoveDownLoop(3);
            }
            else
            {
                tileGrid.tileArray[x, counter].GetComponent<Tile>().MoveDownLoop(1);
            }
            counter--;
            yield return null;
        }

        //tileGrid.tileArray[x, yOffset].GetComponent<Tile>().MoveDownLoop(1 + yOffset);
    }
}
