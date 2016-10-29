using UnityEngine;
using System.Collections;

public class TurbineRotate : MonoBehaviour {

    [Range(0, 100)]
    public float RotationSpeed = 5;

    private float RotationValue = 0;

	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        RotationValue = Mathf.Lerp(RotationValue, RotationValue - RotationSpeed, 0.1f);
        transform.rotation = Quaternion.Euler(0, 0, RotationValue);
        if (RotationValue < -180) RotationValue += 360;
    }
}
