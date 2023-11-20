using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GridController : MonoBehaviour
{
    public Camera mapCamera;
    public GameObject background;
    public GameObject grid;
    public GameObject gridSelector;

    private GameObject[,] gridCells;
    private GridInfo gridInfo;
    private PlayerController playerController;
    private GameObject selectedCell;
    private int selectX;
    private int selectY;
    private AudioSource audioSource;

    //Selection Colours
    private Color deselectColor = new Color(0, 1f, 0, 0.19f);
    private Color selectColor = new Color(0, 0, 1f, 0.19f);

    void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        gridInfo = grid.GetComponent<GridInfo>();
        gridCells = gridInfo.grid;
        for (int i = 0; i < gridInfo.gridSizeX; i++)
        {
            for (int j = 0; j < gridInfo.gridSizeY; j++)
            {
                if (gridCells[i, j] != null)
                {
                    gridCells[i, j].transform.position = grid.transform.position + new Vector3(i * 8, j * 8, 0f);
                }
            }
        }
        mapCamera.orthographicSize = Math.Max(gridInfo.gridSizeX / mapCamera.aspect, gridInfo.gridSizeY) * gridInfo.cellSize / 2;
        mapCamera.transform.position = grid.transform.position + new Vector3(gridInfo.cellSize * (gridInfo.gridSizeX - 1) / 2f, (gridInfo.gridSizeY - 1) / 2f * gridInfo.cellSize + 2, -6);
        background.transform.position = mapCamera.transform.position + new Vector3(0,-mapCamera.orthographicSize-3.5f, 10f);
        background.transform.localScale = new Vector3((mapCamera.orthographicSize * mapCamera.aspect+2f) / 14f, (mapCamera.orthographicSize+6f) / 9f,1f);
        gridSelector.transform.localScale = new Vector3(gridInfo.cellSize, gridInfo.cellSize, 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedCell == null)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame && selectX + 1 < gridInfo.gridSizeX)
            {
                selectX += 1;
            }
            if (Keyboard.current.aKey.wasPressedThisFrame && selectX > 0)
            {
                selectX -= 1;
            }
            if (Keyboard.current.wKey.wasPressedThisFrame && selectY + 1 < gridInfo.gridSizeY)
            {
                selectY += 1;
            }
            if (Keyboard.current.sKey.wasPressedThisFrame && selectY > 0)
            {
                selectY -= 1;
            }
            moveGridSelector();
        }
        else
        {
            if (Keyboard.current.dKey.wasPressedThisFrame && selectX + 1 < gridInfo.gridSizeX && gridCells[selectX + 1, selectY] == null)
            {
                gridCells[selectX, selectY] = null;
                selectX += 1;
                gridCells[selectX, selectY] = selectedCell;
            }
            if (Keyboard.current.aKey.wasPressedThisFrame && selectX > 0 && gridCells[selectX - 1, selectY] == null)
            {
                gridCells[selectX, selectY] = null;
                selectX -= 1;
                gridCells[selectX, selectY] = selectedCell;
            }
            if (Keyboard.current.wKey.wasPressedThisFrame && selectY + 1 < gridInfo.gridSizeY && gridCells[selectX, selectY + 1] == null)
            {
                gridCells[selectX, selectY] = null;
                selectY += 1;
                gridCells[selectX, selectY] = selectedCell;
            }
            if (Keyboard.current.sKey.wasPressedThisFrame && selectY > 0 && gridCells[selectX, selectY - 1] == null)
            {
                gridCells[selectX, selectY] = null;
                selectY -= 1;
                gridCells[selectX, selectY] = selectedCell;
            }
            moveGridSelector();
            moveselectedCell();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && selectedCell == null)
        {
            this.enabled = false;
            playerController.enabled = true;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(playerController.LoadLevel("Menu"));
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            playerController.die();
        }
    }

    void OnEnable()
    {
        gridSelector.SetActive(true); 
        mapCamera.gameObject.SetActive(true);
        resetSelection();
    }

    void OnDisable()
    {
        gridSelector.SetActive(false);
        mapCamera.gameObject.SetActive(false);
    }

    void resetSelection()
    {
        selectX = 0;
        selectY = 0;
        moveGridSelector();
    }

    void moveGridSelector()
    {
        gridSelector.transform.position = grid.transform.position + new Vector3(selectX * 8, selectY * 8 + 2, -1f);
    }

    void moveselectedCell()
    {
        selectedCell.transform.position = grid.transform.position + new Vector3(selectX * 8, selectY * 8,0f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (this.enabled == true && context.performed)
        {
            if (selectedCell == null)
            {
                if (gridCells[selectX, selectY] != null && !gridCells[selectX, selectY].transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    audioSource.Play();
                    selectedCell = gridCells[selectX, selectY];
                    gridSelector.GetComponent<SpriteRenderer>().color = selectColor;
                }
            }
            else
            {
                audioSource.Play();
                selectedCell = null;
                gridSelector.GetComponent<SpriteRenderer>().color = deselectColor;
            }
        }
    }
}