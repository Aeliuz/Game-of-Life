using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOfLife : MonoBehaviour
{
    public GameObject cellPrefab;
    public Slider camera_slider;
    public Slider spawn_slider;
    public Camera game_camera;

    Scenehandler scenehandler;

    Cell[,] cells;
    float cellSize = 1f; //Size of our cells
    int numberOfColums, numberOfRows;
    int spawnChancePercentage = 57;
    int last_generation = -1;
    int generation_before_last = -5;
    public int generations = 0;
    public int big_cells = 0;
    bool pause = false;
    public bool stable = false;

    public static GameOfLife Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        spawnChancePercentage = DataHandler.fillRate;
        if (spawnChancePercentage < 20) spawnChancePercentage = 35;
        Debug.Log("spawn chance: " + spawnChancePercentage);
        Camera.main.orthographicSize = DataHandler.cameraSize;
        Debug.Log("camera size: " + camera_slider.value);

        //Lower framerate makes it easier to test and see whats happening.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 12;

        //Calculate our grid depending on size and cellSize
        numberOfColums = (int)Mathf.Floor((Camera.main.orthographicSize * game_camera.aspect * 2) / cellSize);
        numberOfRows = (int)Mathf.Floor(Camera.main.orthographicSize * 2 / cellSize);

        //Initiate our matrix array
        cells = new Cell[numberOfColums, numberOfRows];

        //Create all objects
        //For each row
        for (int y = 0; y < numberOfRows; y++)
        {
            //for each column in each row
            for (int x = 0; x < numberOfColums; x++)
            {
                //Create our game cell objects, multiply by cellSize for correct world placement
                Vector2 newPos = new Vector2(x * cellSize - Camera.main.orthographicSize * Camera.main.aspect + cellSize / 2, y *
                    cellSize - Camera.main.orthographicSize + cellSize / 2);

                var newCell = Instantiate(cellPrefab, newPos, Quaternion.identity);
                newCell.transform.localScale = Vector2.one * cellSize;
                cells[x, y] = newCell.GetComponent<Cell>();

                //Random check to see if it should be alive
                if (Random.Range(0, 100) < DataHandler.fillRate && Scenehandler.custom_start == false && Scenehandler.pattern_start == false)
                {
                    cells[x, y].alive = true;
                }

                if (Scenehandler.custom_start == true)
                {
                    pause = true;
                }

                cells[x, y].CreateCells();
            }
        }

        if (Scenehandler.pattern_start == true)
        {
            cells[numberOfColums / 2 - 3, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 - 4, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 - 5, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 - 6, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 - 4, numberOfRows / 2 + 1].alive = true;
            cells[numberOfColums / 2 - 5, numberOfRows / 2 + 1].alive = true;
            cells[numberOfColums / 2 - 4, numberOfRows / 2 - 1].alive = true;
            cells[numberOfColums / 2 - 5, numberOfRows / 2 - 1].alive = true;

            cells[numberOfColums /2 + 3, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 + 4, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 + 5, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 + 6, numberOfRows / 2].alive = true;
            cells[numberOfColums / 2 + 4, numberOfRows / 2 + 1].alive = true;
            cells[numberOfColums / 2 + 5, numberOfRows / 2 + 1].alive = true;
            cells[numberOfColums / 2 + 4, numberOfRows / 2 - 1].alive = true;
            cells[numberOfColums / 2 + 5, numberOfRows / 2 - 1].alive = true;

            cells[numberOfColums / 2, numberOfRows / 2 + 3].alive = true;
            cells[numberOfColums / 2, numberOfRows / 2 + 6].alive = true;
            cells[numberOfColums / 2 - 1, numberOfRows / 2 + 4].alive = true;
            cells[numberOfColums / 2 - 1, numberOfRows / 2 + 5].alive = true;
            cells[numberOfColums / 2 + 1, numberOfRows / 2 + 4].alive = true;
            cells[numberOfColums / 2 + 1, numberOfRows / 2 + 5].alive = true;

            cells[numberOfColums / 2, numberOfRows / 2 - 3].alive = true;
            cells[numberOfColums / 2, numberOfRows / 2 - 6].alive = true;
            cells[numberOfColums / 2 - 1, numberOfRows / 2 - 4].alive = true;
            cells[numberOfColums / 2 - 1, numberOfRows / 2 - 5].alive = true;
            cells[numberOfColums / 2 + 1, numberOfRows / 2 - 4].alive = true;
            cells[numberOfColums / 2 + 1, numberOfRows / 2 - 5].alive = true;

        }

    }



    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int cellX = Mathf.FloorToInt((mousePosition.x + Camera.main.orthographicSize * Camera.main.aspect) / cellSize);
            int cellY = Mathf.FloorToInt((mousePosition.y + Camera.main.orthographicSize) / cellSize);

            if (cellX >= 0 && cellX < numberOfColums && cellY >= 0 && cellY < numberOfRows)
            {
                cells[cellX, cellY].alive = !cells[cellX, cellY].alive;
                cells[cellX, cellY].next_state_alive = cells[cellX, cellY].alive;
                cells[cellX, cellY].UpdateStatus();
                Debug.Log("cell updated" + "x: " + cellX + "  y: " + cellY);
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause = !pause;
        }

        if (Input.GetKeyDown(KeyCode.M) && Application.targetFrameRate < 20)
        {
            Application.targetFrameRate++;
        }

        if (Input.GetKeyDown(KeyCode.N) && Application.targetFrameRate > 1)
        {
            Application.targetFrameRate--;
        }



        if (!pause)
        {
            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColums; x++)
                {
                    int cell_neighbours = Check_Neighbours(x, y);

                    if (cell_neighbours < 2 || cell_neighbours > 3)
                    {
                        cells[x, y].next_state_alive = false;
                    }
                    else
                    {
                        if (cells[x, y].alive == true)
                        {
                            cells[x, y].next_state_alive = true;
                        }
                        else if (!cells[x, y].alive && cell_neighbours == 3)
                        {
                            cells[x, y].next_state_alive = true;

                        }
                    }
                }
            }

            for (int y = 0; y < numberOfRows; y++)
            {
                for (int x = 0; x < numberOfColums; x++)
                {
                    cells[x, y].UpdateStatus();
                }
            }
        }



        void Check_if_stable()
        {
            if (big_cells == last_generation && big_cells == generation_before_last) 
                stable = true;
            else 
                stable = false;
        }

        Check_if_stable();

        Debug.Log(stable);

        generation_before_last = last_generation;
        last_generation = big_cells;
        big_cells = 0;
        generations++;
    }

    int Check_Neighbours(int cellX, int cellY)
    {
        int neighbours = 0;
        int leftX;
        int belowY;
        int rightX;
        int aboveY;

        if (cellX == 0) leftX = 0;
        else leftX = -1;

        if (cellX == numberOfColums - 1) rightX = 1;
        else rightX = 2;

        if (cellY == 0) belowY = 0;
        else belowY = -1;

        if (cellY == numberOfRows - 1) aboveY = 1;
        else aboveY = 2;

        for (int x = leftX; x < rightX; x++)
        {
            for (int y = belowY; y < aboveY; y++)
            {
                if (cells[cellX + x, cellY + y].alive == true)
                {
                    neighbours++;
                }
            }
        }

        if (cells[cellX, cellY].alive == true)
        {
            neighbours--;
        }

        return neighbours;
    }
}
