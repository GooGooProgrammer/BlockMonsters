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
    private RectFormer rectFormer;

    private Dictionary<Vector2, GameObject> MonstersOnBoard = new Dictionary<Vector2, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        magicBoard = GetComponent<MagicBoard>();
        shadowSpawner = GetComponent<ShadowSpawner>();
        rectFormer = GetComponent<RectFormer>();
    }

    public GameObject GetGridTouching(Vector3 monsterPos)
    {
        int x = (int)Math.Floor(monsterPos.x - transform.position.x);
        int y = (int)Math.Floor(monsterPos.y - transform.position.y);
        if (x >= 0 && x < MagicBoard.boardSizeX && y >= 0 && y < MagicBoard.boardSizeX)
        {
            return magicBoard.GetGrid(x, y);
        }
        return null;
    }

    public bool CheckMonsterCanPlace(GameObject monster, GameObject Grid)
    {
        Vector3 gridPos = Grid.transform.localPosition;

        for (int i = 0; i < monster.transform.childCount; i++)
        {
            if (i != 0)
            {
                //the position differnce between block[i-1] and block[i]
                Vector3 blockDiff =
                    monster.transform.GetChild(i).localPosition
                    - monster.transform.GetChild(i - 1).localPosition;
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

    public void PlaceMonsterOnBoard(GameObject monster, GameObject Grid)
    {
        shadowSpawner.DestroyShadow();
        monster.transform.position = Grid.transform.position;
        Vector3 gridPos = Grid.transform.localPosition;
        for (int i = 0; i < monster.transform.childCount; i++)
        {
            if (i != 0)
            {
                //the position differnce between block[i-1] and block[i]
                Vector3 blockDiff =
                    monster.transform.GetChild(i).localPosition
                    - monster.transform.GetChild(i - 1).localPosition;
                gridPos = gridPos + blockDiff;
            }

            magicBoard.IsOccuplied[(int)Math.Floor(gridPos.x), (int)Math.Floor(gridPos.y)] = true;
        }

        MonstersOnBoard.Add(Grid.transform.localPosition, monster);

        //Problem: the script below should be in a new class
        List<Vector2> blockRect = rectFormer.MonsterFormRect(Grid);

        if (blockRect != null)
        {

            rectFormer.ClearOccupliedInRect(blockRect);

            GameObject monsterGonnaDelete;

            for (float x = blockRect[2].x; x <= blockRect[0].x; x++)
            {
                for (float y = blockRect[2].y; y <= blockRect[0].y; y++)
                {
                    if(MonstersOnBoard.TryGetValue(new Vector2(x,y), out monsterGonnaDelete))
                    {
                        Destroy(monsterGonnaDelete);
                        MonstersOnBoard.Remove(new Vector2(x,y));
                    }
                }
            }
        }
        //the script ends
    }
}
