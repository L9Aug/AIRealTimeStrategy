using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SM;

public class BaseUnit : MonoBehaviour
{
    public enum unitType { civilian, military, sacrifice };

    public int TeamID;
    public unitType type;
    public HexTransform hexTransform;
    public string unitName;
    public float moveSpeed;
    public float health;
    public int visionRadius;
    public StateMachine unitStateMachine;

    public State move;
    public List<HexTile> path;

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
            Vector3 temp = SteeringBehaviours.Arrive(this, path[0], 0.1f, 0.1f);
            transform.Translate(temp);

            if (Vector3.Distance(transform.position, path[0].transform.position) < 0.2f)
            {
                hexTransform.Position = path[0].hexTransform.Position;
                path.RemoveAt(0);
            }
        }
    }

    public void NullAction()
    {

    }
}
