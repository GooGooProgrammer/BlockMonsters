using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardPlacer : MonoBehaviour
{
    //attach to Board
    //Place monster and edit the board state

    private MagicBoard magicBoard;
    private ShadowSpawner shadowSpawner;

    // Start is called before the first frame update
    void Start()
    {
        magicBoard = GetComponent<MagicBoard>();
        shadowSpawner = GetComponent<ShadowSpawner>();

    }

    public GameObject GetGridTouching(Vector3 MonsterPos)
    {
        int x = (int)Math.Floor(MonsterPos.x - transform.position.x);
        int y = (int)Math.Floor(MonsterPos.y - transform.position.y);
        if (x >= 0 && x < MagicBoard.boardSizeX && y >= 0 && y < MagicBoard.boardSizeX)
        {
            return magicBoard.GetGrid(x, y);
        }
        return null;
    }

    public bool CheckMonsterCanPlace(GameObject Monster, GameObject Grid)
    {
        Vector3 gridPos = Grid.transform.localPosition;

        for (int i = 0; i < Monster.transform.childCount; i++)
        {
            if (i != 0)
            {
                //the position differnce between block[i-1] and block[i]
                Vector3 blockDiff =
                    Monster.transform.GetChild(i).localPosition
                    - Monster.transform.GetChild(i - 1).localPosition;
                gridPos = gridPos + blockDiff;
            }

            // first 4 statements is prevent the gridPos out of array
            // the 5th statement is check whether the gird is occuplied

            if (
                (int)Math.Floor(gridPos.x) > MagicBoard.boardSizeX - 1
                || (int)Math.Floor(gridPos.y) > MagicBoard.boardSizeY - 1
                || (int)Math.Floor(gridPos.x) < 0
                || (int)Math.Floor(gridPos.y) < 0
                || magicBoard.IsOccuplied[(int)Math.Floor(gridPos.x), (int)Math.Floor(gridPos.y)]
            )
            {
                return false;
            }
        }
        return true;
    }

    public void PlaceMonsterOnBoard(GameObject Monster, GameObject Grid)
    {
        shadowSpawner.DestroyShadow();
        Monster.transform.position = Grid.transform.position;
        Vector3 gridPos = Grid.transform.localPosition;
        for (int i = 0; i < Monster.transform.childCount; i++)
        {
            if (i != 0)
            {
                //the position differnce between block[i-1] and block[i]
                Vector3 blockDiff =
                    Monster.transform.GetChild(i).localPosition
                    - Monster.transform.GetChild(i - 1).localPosition;
                gridPos = gridPos + blockDiff;
            }

            magicBoard.IsOccuplied[(int)Math.Floor(gridPos.x), (int)Math.Floor(gridPos.y)] = true;
        }

        MonsterFormRect(Grid);
    }

    //array to store the corners of the monster(s)
    List<Vector2> corners;

    //4 direction for check whether there is a rect
    //Top Right, Bottom Right, Bottom Left, Top Left
    private int[,] Direction =
    {
        { 1, 1 },
        { 1, -1 },
        { -1, -1 },
        { -1, 1 }
    };

    //MonsterFormRect used to check whether the placed monster can form a rect
    public void MonsterFormRect(GameObject Grid)
    {
        corners = new List<Vector2>();

        int gridPosX = (int)Math.Floor(Grid.transform.localPosition.x);
        int gridPosY = (int)Math.Floor(Grid.transform.localPosition.y);

        //To find the 4 corners of rect
        for (int i = 0; i < 4; i++)
        {
            FindCorner(gridPosX, gridPosY, i);
        }
        //if corners num are 1 2 it must be a rect
        //else if 4 then it maybe, other num is impossible
        if (corners.Count == 1 || corners.Count == 2)
        {
            Debug.Log("rectForm");
        }
        else if (corners.Count == 4)
        {
            //for 4 corners, if 1 corner minus another 1 corner, one of the x/y must be 0 (cuz straight line)

            Vector2 cache = corners[3];

            foreach (Vector2 c in corners)
            {
                cache.x -= c.x;
                cache.y -= c.y;
                if (cache.x != 0 && cache.y != 0) // for those not straight line, return
                {
                    return;
                }
                cache.x = c.x;
                cache.y = c.y;
            }

            //Check whether inside is full
            for (int x = (int)corners[2].x; x < (int)corners[0].x; x++)
            {
                for (int y = (int)corners[2].y; y < (int)corners[0].y; y++)
                {
                    if (!magicBoard.IsOccuplied[x, y])
                    {
                        return;
                    }
                }
            }
        }
        else
            return; // return means no rect form

        Debug.Log("rectForm");
    }

    public void FindCorner(int x, int y, int d)
    {
        //will go to the deepest to find the corner
        int count = 0;
        if (
            DirectionCondition(x, Direction[d, 0]) && magicBoard.IsOccuplied[x + Direction[d, 0], y]
        )
        {
            FindCorner(x + Direction[d, 0], y, d);
            count++;
        }
        if (
            DirectionCondition(y, Direction[d, 1]) && magicBoard.IsOccuplied[x, y + Direction[d, 1]]
        )
        {
            FindCorner(x, y + Direction[d, 1], d);
            count++;
        }

        //count == 0 mean the corner is Grid[x,y]

        if (count == 0)
        {
            corners.Add(new Vector2(x, y));

            if (corners.Count != corners.Distinct().Count())
            {
                corners.RemoveAt(corners.Count - 1);
            }
        }
    }

    private bool DirectionCondition(int number, int direction)
    {
        if (direction == 1)
        {
            return number < MagicBoard.boardSizeX - 1; //this assume boardX = boardY
        }
        else
        {
            return number > 0;
        }
    }
}
