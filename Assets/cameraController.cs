using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    float deltaPosition = 0.1f;
    float deltaAngle = 1.0f;

    float eps = 0.000001f;
    // Start is called before the first frame update
    bool trackingMode;
    GameObject drone;
    Vector3 prevDronePosition;
    Quaternion prevDroneRotation;
    float distFromDrone = 5.0f;

    void Start()
    {
        drone = GameObject.FindWithTag("drone");
        trackingMode = false;
        prevDronePosition = drone.transform.position;
        prevDroneRotation = drone.transform.rotation;
    }

    void moveCamera()
    {
        //Moving in positive X direction
        if (Input.GetKey("e"))
        {
            transform.position += new Vector3(deltaPosition, 0, 0);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }
        }
        //Moving in negative X direction
        if (Input.GetKey("q"))
        {
            transform.position += new Vector3(-deltaPosition, 0, 0);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }
        }

        //Moving in positive Y direction
        if (Input.GetKey("d"))
        {
            transform.position += new Vector3(0, deltaPosition, 0);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }            
        }
        //Moving in negative Y direction
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(0, -deltaPosition, 0);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }            
        }

        //Moving in positive Z direction
        if (Input.GetKey("c"))
        {
            transform.position += new Vector3(0, 0, deltaPosition);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }            
        }
        //Moving in negative Z direction
        if (Input.GetKey("z"))
        {
            transform.position += new Vector3(0, 0, -deltaPosition);
            if (trackingMode)
            {
                transform.LookAt(drone.transform);
            }            
        } 
    }

    void rotateCamera()
    {
        //Rotating about X axis in positive angle
        if (Input.GetKey("p"))
        {
            transform.Rotate(deltaAngle, 0, 0, Space.Self);
        }
        //Rotating about X axis in negative angle
        if (Input.GetKey("i"))
        {
            transform.Rotate(-deltaAngle, 0, 0, Space.Self);
        }       

        //Rotating about Y axis in positive angle
        if (Input.GetKey("l"))
        {
            transform.Rotate(0, deltaAngle, 0, Space.Self);
        }
        //Rotating about Y axis in negative angle
        if (Input.GetKey("j"))
        {
            transform.Rotate(0, -deltaAngle, 0, Space.Self);
        }

        //Rotating about Z axis in positive angle 
        if (Input.GetKey("m"))
        {
            transform.Rotate(0, 0, deltaAngle, Space.Self);
        }        


        //Rotating about Z axis in negative angle 
        if (Input.GetKey("b"))
        {
            transform.Rotate(0, 0, -deltaAngle, Space.Self);
        }
    }

    void trackRotatingDrone()
    {
        Quaternion diffRotation = drone.transform.rotation * Quaternion.Inverse(prevDroneRotation);
        prevDroneRotation = drone.transform.rotation;
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        diffRotation.ToAngleAxis(out angle, out axis);         
        if (trackingMode && angle > eps)
        {
            transform.RotateAround(drone.transform.position, axis, angle);                       
        }
    }

    void trackMovingDrone()
    {
        Vector3 t1 = prevDronePosition - transform.position;
        Vector3 t2 = drone.transform.position - transform.position;
        Vector3 movement = t2 - t1;
        prevDronePosition = drone.transform.position;
        if (trackingMode && movement.magnitude > eps)
        {
            transform.position += movement;                       
        }
    }

    void locateCameraCloseToDrone()
    {
        //Vector3 shiftFromDrown = new Vector3(0,0,distFromDrone);
        Vector3 shiftFromDrown = distFromDrone * transform.forward;
        transform.position = drone.transform.position - shiftFromDrown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            trackingMode = !trackingMode;
            if (trackingMode)
                locateCameraCloseToDrone();
        }
        moveCamera();
        rotateCamera();       
    }    

    void LateUpdate()
    {
        trackRotatingDrone();    
        trackMovingDrone();
    }
}
