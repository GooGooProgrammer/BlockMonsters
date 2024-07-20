using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public enum Type
{
    attack,
    defend
}

//BlockMonster Data
public class BlockMonster : MonoBehaviour
{
    [SerializeField] private string monsterName;
    [SerializeField] private Type type;
    [SerializeField] private bool isActive = true;
    void Start()
    {
        BlockDataConstruct();
    }
    void Update()
    {
        if(gameObject.GetComponent<MonsterMover>() && isActive==false)
        Destroy(gameObject.GetComponent<MonsterMover>());
    }
    private void BlockDataConstruct()
    {
        transform.GetChild(0).transform.GetComponent<Collider2D>().isTrigger=false;
    }

        public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
}
