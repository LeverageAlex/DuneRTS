using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

/// <summary>
/// This class is responsible for the movement of the camera
/// </summary>
public class CameraMovement : MonoBehaviour {


    //Zoom parameters
    public float maxRadius = 35f;
    public float minRadius = 5f;

    public float scrollSpeed = 0.25f;

    public float speedHorizontal = 10f;
    public float speedVertical = 10f;

    public float radius = 30f;

    private Vector2 positionAngles = new Vector2(30,45);
    private Quaternion rotationGoal;
    private Vector3 origin = new Vector3(0,0,0);

    public Vector3 orbitCenter;

    private Camera attachedCamera;

    // Start is called before the first frame update
    void Start() {
        attachedCamera = this.GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update() {

        // Movement
        if (Input.GetKey(KeyCode.LeftControl)) {
            positionAngles.y += (Input.GetAxis("Mouse X") - Input.GetAxis("Horizontal")) * Time.deltaTime * speedHorizontal;
            positionAngles.x = Mathf.Clamp( positionAngles.x + ((Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical")) * Time.deltaTime * speedVertical), 5f, 85f);
        }

        
        rotationGoal = Quaternion.Euler(-positionAngles.x, positionAngles.y, 0);
        transform.position = orbitCenter + (rotationGoal * new Vector3(0,0,radius));

        // Look at the origin of the map
        attachedCamera.transform.LookAt(orbitCenter, Vector3.up);

        // Zoom
        radius = Mathf.Clamp(radius + (Input.mouseScrollDelta.y * scrollSpeed), minRadius, maxRadius);
    }

    public void resetOrbitFocus(Vector3 orbitPoint) {
        orbitCenter = orbitPoint;
    }

    public void resetOrbitFocus(Vector3 orbitPoint, Vector2 angles) {
        orbitCenter = orbitPoint;
        positionAngles = angles;
    }
}
