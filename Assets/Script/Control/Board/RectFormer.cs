using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RectFormer : MonoBehaviour
{
    private MagicBoard magicBoard;

    // Start is called before the first frame update
    void Start()
    {
        magicBoard = GetComponent<MagicBoard>();
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
    public List<Vector2> MonsterFormRect(GameObject Grid)
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
        if (corners.Count == 1 || corners.Count == 2) { }
        else if (corners.Count == 4)
        {
            //for 4 corners, if 1 corner minus another 1 corner, one of the x/y must be 0 (cuz straight line)
            if (!CheckStraghtLine(corners) || !CheckInside(corners))
            {
                return null; // return means no rect form
            }
        }
        else
            return null; // return means no rect form

        return corners;
        
    }
    public void ClearOccupliedInRect(List<Vector2> corners)
    {
        for(int x = (int)corners[2].x ; x <= (int)corners[0].x;x++)
        {
            for(int y = (int)corners[2].y ; y <= (int)corners[0].y ;y++)
            {
                magicBoard.IsOccuplied[x,y] = false;
            }
        }
    }

    private void FindCorner(int x, int y, int d)
    {
        //will go to the deepest to find the corner
        int count = 0;
        if (
            DirectionCondition(x, Direction[d, 0], MagicBoard.boardSizeX)
            && magicBoard.IsOccuplied[x + Direction[d, 0], y]
        )
        {
            FindCorner(x + Direction[d, 0], y, d);
            count++;
        }
        if (
            DirectionCondition(y, Direction[d, 1], MagicBoard.boardSizeY)
            && magicBoard.IsOccuplied[x, y + Direction[d, 1]]
        )
        {
            FindCorner(x, y + Direction[d, 1], d);
            count++;
        }

        if (count == 0) //count == 0 means no more corners can explore, means the corner is Grid[x,y]
        {
            corners.Add(new Vector2(x, y));

            if (corners.Count != corners.Distinct().Count()) //delete redundant corners
            {
                corners.RemoveAt(corners.Count - 1);
            }
        }
    }

    private bool CheckStraghtLine(List<Vector2> corners)
    {
        Vector2 cache = corners[3];
        foreach (Vector2 c in corners)
        {
            cache.x -= c.x;
            cache.y -= c.y;
            if (cache.x != 0 && cache.y != 0) // for those not straight line, return false
            {
                return false;
            }
            cache.x = c.x;
            cache.y = c.y;
        }
        return true;
    }

    //Check whether inside is full
    private bool CheckInside(List<Vector2> corners)
    {
        for (int x = (int)corners[2].x; x <= (int)corners[0].x; x++)
        {
            for (int y = (int)corners[2].y; y <= (int)corners[0].y; y++)
            {
                if (!magicBoard.IsOccuplied[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool DirectionCondition(int number, int direction, int boardSize)
    {
        if (direction > 0)
        {
            return number < boardSize - 1;
        }
        else
        {
            return number > 0;
        }
    }
}
