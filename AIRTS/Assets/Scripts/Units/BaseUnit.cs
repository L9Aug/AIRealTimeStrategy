﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

public class BaseUnit : GameEntity
{
    public enum unitType { civilian, military, sacrifice };
    
    public unitType type;
    public string unitName;
    public float moveSpeed;
    public int visionRadius;
    public StateMachine unitStateMachine;

    public State move;
    public List<AStarInfo<HexTile>> path;

    public ASImplementation<HexTile> aStar;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        unitStateMachine.SMUpdate();
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Vector3 temp = SteeringBehaviours.Arrive(this, path[0].current, 0.1f, 0.1f);
            transform.Translate(temp);

            if (Vector3.Distance(transform.position, path[0].current.transform.position) < 0.2f)
            {
                hexTransform.Position = path[0].current.hexTransform.Position;
                path.RemoveAt(0);
            }
        }
    }

    public void NullAction()
    {

    }
}
