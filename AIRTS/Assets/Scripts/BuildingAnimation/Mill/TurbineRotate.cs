using UnityEngine;
using System.Collections;

public class TurbineRotate : MonoBehaviour {

    [Tooltip("The amount of degrees this should rotate per second.")]
    [Range(0, 100)]
    public float RotationSpeed = 5;

    [Tooltip("This is the axis of rotation")]
    public Vector3 Axis;

    [Tooltip("This must be either -1 or 1, any other value will be counted as 1.")]
    [Range(-1, 1)]
    public int Dir = 1;

    private float RotationValue = 0;

    void Start()
    {
        if (Dir == 0) Dir = 1;
    }

    void FixedUpdate()
    {
        RotationValue = Mathf.Lerp(RotationValue, RotationValue - (RotationSpeed * Dir), Time.fixedDeltaTime);
        transform.rotation = Quaternion.AngleAxis(RotationValue, Axis);
        if (Mathf.Abs(RotationValue) > 360) RotationValue %= 360;
    }
}
