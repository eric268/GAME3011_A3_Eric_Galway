using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateTiles : MonoBehaviour
{
    TileGridSlotGenerator tileGrid;
    public DifficultyTypes difficultyType;
    // Start is called before the first frame update
    void Start()
    {
        tileGrid = GetComponent<TileGridSlotGenerator>();
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
                PopulateTileEasy();
                break;
            case DifficultyTypes.Hard:
                PopulateTileEasy();
                break;

        }
    }

    void PopulateTileEasy()
    {

    }
    void PopulateTileMedium()
    {

    }
    void PopulateTileHard()
    {

    }
}
