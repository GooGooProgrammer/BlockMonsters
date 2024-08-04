using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoard : MonoBehaviour
{
    public static int boardSizeX = 6;
    public static int boardSizeY = 6;

    [SerializeField]
    private GameObject GridPrefabs;
    private GameObject[,] Grids = new GameObject[boardSizeX, boardSizeY];
    private bool[,] isOccuplied = new bool[boardSizeX, boardSizeY];
  

    // Start is called before the first frame update
    void Start()
    {
        SpawnBoard();
    }

    //spawnBoard creates a empty board
    void SpawnBoard()
    {
        //create grids can put them into grids[,]
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                Grids[x, y] = Instantiate(
                    GridPrefabs,
                    transform.TransformPoint(new Vector3(x, y, 0)),
                    Quaternion.Euler(0, 0, 0),
                    this.transform
                );
                isOccuplied[x, y] = false;
            }
        }
    }

    public bool[,] IsOccuplied
    {
        get { return isOccuplied; }
        set { isOccuplied = value; }
    }

    public GameObject GetGrid(int x, int y)
    {
        return Grids[x, y];
    }
}
