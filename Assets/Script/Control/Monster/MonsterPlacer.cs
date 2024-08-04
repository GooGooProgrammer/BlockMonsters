using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlacer : MonoBehaviour
{
    //attach to monster
    //a controller for placing monster

    GameObject gridTouching;
    private BoardPlacer boardPlacer;
    private ShadowSpawner shadowSpawner;

        void Start()
    {
        //Problem: Dont get reference by name
        boardPlacer = GameObject.Find("MagicBoard").GetComponent<BoardPlacer>();
        shadowSpawner = GameObject.Find("MagicBoard").GetComponent<ShadowSpawner>();
    }

    public void CheckGridTouching()
    {
        GameObject OldGrid = gridTouching;
        gridTouching = boardPlacer.GetGridTouching(transform.position);

        //Destroy old shawdow when new shadow spawn
        if (OldGrid && OldGrid != gridTouching)
        {
            shadowSpawner.DestroyShadow();
        }

        if (gridTouching && boardPlacer.CheckMonsterCanPlace(gameObject, gridTouching))
        {
            shadowSpawner.SpawnShadow(gameObject, gridTouching);
        }
    }

    public void PlaceMonster()
    {
        if (gridTouching && boardPlacer.CheckMonsterCanPlace(gameObject, gridTouching))
        {
            boardPlacer.PlaceMonsterOnBoard(gameObject, gridTouching);
            GetComponent<BlockMonster>().IsActive=false;
        }
    }
}
