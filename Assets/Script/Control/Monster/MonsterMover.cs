using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMover : MonoBehaviour
{
    //attach to monster
    //a controller for moving monster

    Vector3 mousePosition;
    Vector3 prevMousePos;
    Vector3 originalPos;
    Vector2 mouseDiff;

    //The Grid which is touching the monster, null if no grid is touching
    private MonsterPlacer monsterPlacer;
    private ShadowSpawner shadowSpawner;

    void Start()
    {
        shadowSpawner = GameObject.Find("MagicBoard").GetComponent<ShadowSpawner>();

        monsterPlacer = transform.GetComponent<MonsterPlacer>();
    }

    //mouse interact starts---


    private void OnMouseDown()
    {
        SetUpForMoveMonseter();
    }

    private void OnMouseDrag()
    {
        MoveMonster();
        RotateMonster();
        monsterPlacer.CheckGridTouching();

    //should be a new class
    //should be a new class

    }

    private void OnMouseUp()
    {
        monsterPlacer.PlaceMonster();
    }

    //mouse interact ends---




    private void SetUpForMoveMonseter()
    {
        //set up information of mouse position and monseter position
        prevMousePos = Input.mousePosition;
        prevMousePos = Camera.main.ScreenToWorldPoint(prevMousePos);

        originalPos = transform.position;
    }

    private void MoveMonster()
    {
        // monster follow the mouse
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mouseDiff = new Vector2(mousePosition.x - prevMousePos.x, mousePosition.y - prevMousePos.y);

        transform.position = new Vector3(
            originalPos.x + mouseDiff.x,
            originalPos.y + mouseDiff.y,
            0
        );
    }

    private void RotateMonster()
    {
        //direction means rotate direction
        int direction;
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = new Vector2(
                    -transform.GetChild(i).localPosition.x,
                    transform.GetChild(i).localPosition.y
                );
            }
            shadowSpawner.DestroyShadow();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            direction = 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            direction = -1;
        }
        else
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2 blockPos = transform.GetChild(i).localPosition;
            transform.GetChild(i).localPosition = new Vector2(
                blockPos.y * direction,
                blockPos.x * -direction
            );
            transform.GetChild(i).localPosition = new Vector2(
                blockPos.y * -direction,
                blockPos.x * direction
            );
        }
        shadowSpawner.DestroyShadow();
        //regenerate the shadow
    }
}
