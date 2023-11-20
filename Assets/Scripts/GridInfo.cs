using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    public GameObject[,] grid;
    public GameObject background;
    public GameObject deathBox;
    public int gridSizeX = 3;
    public int gridSizeY = 3;
    public int cellSize = 8;

    // Start is called before the first frame update
    void Awake()
    {
        grid = new GameObject[gridSizeX,gridSizeY];
        foreach (Transform t in transform)
        {
            CellInfo cellInfo = t.gameObject.GetComponent<CellInfo>();
            grid[cellInfo.cellX,cellInfo.cellY] = t.gameObject;
        }
    }

    void Start()
    {
        background.transform.position = this.gameObject.transform.position + new Vector3(cellSize * (gridSizeX-1) / 2f, (gridSizeY-1) / 2f * cellSize + 2, 1);
        background.transform.localScale = new Vector3(gridSizeX * cellSize, gridSizeY * cellSize, 1f);
        deathBox.transform.position = this.gameObject.transform.position + new Vector3(cellSize * gridSizeX / 2, -2, 1);
        deathBox.transform.localScale = new Vector3(gridSizeX * cellSize, 1, 1f);
    }
}
