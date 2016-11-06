using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [Range(1, 10)]
    public float CamMoveSpeed = 5;
    
    float Zoom;

	// Use this for initialization
	void Start () {
        Zoom = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 Move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized; // get key input data.

        if (Input.GetKey(KeyCode.LeftBracket)) // decrease the camrea move speed if '[' is being pressed.
        {
            CamMoveSpeed -= Time.deltaTime * CamMoveSpeed * 0.5f;
        }

        if (Input.GetKey(KeyCode.RightBracket)) // increase the camera move speed if ']' is being pressed.
        {
            CamMoveSpeed += Time.deltaTime * CamMoveSpeed * 0.5f;
        }

        float MoveSpeed = CamMoveSpeed;

        if (Input.GetKey(KeyCode.LeftShift)) // double the movespeed if leftshift is being held.
        {
            MoveSpeed *= 2;
        }

        Zoom -= Input.GetAxis("Mouse ScrollWheel") * MoveSpeed; // zoom the camera out if the mousewheel has been altered.
        Zoom = Mathf.Clamp(Zoom, 10, 100);

        transform.Translate(Move * Time.deltaTime * MoveSpeed, Space.World); // move the camera the desired amount

        transform.position = Vector3.Scale(transform.position, new Vector3(1, 0, 1)) + new Vector3(0, Zoom, 0); // set the camera's 'y' position after the camera has been moved.
	}
}
