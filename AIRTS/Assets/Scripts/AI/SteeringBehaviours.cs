using UnityEngine;
using System.Collections;

public class SteeringBehaviours : MonoBehaviour
{
    public static Vector3 Seek(BaseUnit unitData, HexTile target)
    {
        Vector3 temp = new Vector3();

        temp = unitData.transform.position - target.transform.position;
        temp.Normalize();
        temp *= unitData.moveSpeed * Time.deltaTime;

        return temp;
    }

    public static Vector3 Arrive(BaseUnit unitData, HexTile tile, float targetRadius, float slowradius)
    {
        Vector3 direction = tile.transform.position - unitData.transform.position;
        float distance = direction.magnitude;
        float targetSpeed = 0;

        if (distance < targetRadius)
            return Vector3.zero;

        if (distance > slowradius)
            targetSpeed = unitData.moveSpeed;
        else
            targetSpeed = unitData.moveSpeed * (distance / slowradius);

        return (direction * targetSpeed);
    }
}
