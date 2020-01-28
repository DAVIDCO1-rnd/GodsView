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
    GameObject quadCopter;
    Vector3 prevQuadCopterPosition;
    Quaternion prevQuadCopterRotation;
    float distFromQuadCopter = 5.0f;

    void Start()
    {
        quadCopter = GameObject.Find("QuadCopter");
        trackingMode = false;
        prevQuadCopterPosition = quadCopter.transform.position;
        prevQuadCopterRotation = quadCopter.transform.rotation;
    }

    void moveCamera()
    {
        //Moving in positive X direction
        if (Input.GetKey("e"))
        {
            transform.position += new Vector3(deltaPosition, 0, 0);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }
        }
        //Moving in negative X direction
        if (Input.GetKey("q"))
        {
            transform.position += new Vector3(-deltaPosition, 0, 0);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }
        }

        //Moving in positive Y direction
        if (Input.GetKey("d"))
        {
            transform.position += new Vector3(0, deltaPosition, 0);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }            
        }
        //Moving in negative Y direction
        if (Input.GetKey("a"))
        {
            transform.position += new Vector3(0, -deltaPosition, 0);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }            
        }

        //Moving in positive Z direction
        if (Input.GetKey("c"))
        {
            transform.position += new Vector3(0, 0, deltaPosition);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }            
        }
        //Moving in negative Z direction
        if (Input.GetKey("z"))
        {
            transform.position += new Vector3(0, 0, -deltaPosition);
            if (trackingMode)
            {
                transform.LookAt(quadCopter.transform);
            }            
        } 
    }

    void rotateCamera()
    {
        //Rotating about X axis in positive angle
        if (Input.GetKey("p"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, transform.right, deltaAngle);
            }
            else{
                transform.Rotate(deltaAngle, 0, 0, Space.Self);
            }            
        }
        //Rotating about X axis in negative angle
        if (Input.GetKey("i"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, -transform.right, deltaAngle);
            }
            else{
                transform.Rotate(-deltaAngle, 0, 0, Space.Self);
            }            
            
        }       

        //Rotating about Y axis in positive angle
        if (Input.GetKey("l"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, transform.up, deltaAngle);
            }
            else{
                transform.Rotate(0, deltaAngle, 0, Space.Self);
            }            
        }
        //Rotating about Y axis in negative angle
        if (Input.GetKey("j"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, -transform.up, deltaAngle);
            }
            else{
                transform.Rotate(0, -deltaAngle, 0, Space.Self);
            }            
        }

        //Rotating about Z axis in positive angle 
        if (Input.GetKey("m"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, transform.forward, deltaAngle);
            }
            else{
                transform.Rotate(0, 0, deltaAngle, Space.Self);
            }            
        }        


        //Rotating about Z axis in negative angle 
        if (Input.GetKey("b"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.transform.position, -transform.forward, deltaAngle);
            }
            else{
                transform.Rotate(0, 0, -deltaAngle, Space.Self);
            }            
        }
    }

    void trackRotatingQuadCopter()
    {
        Quaternion diffRotation = quadCopter.transform.rotation * Quaternion.Inverse(prevQuadCopterRotation);
        prevQuadCopterRotation = quadCopter.transform.rotation;
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        diffRotation.ToAngleAxis(out angle, out axis);         
        if (trackingMode && angle > eps)
        {
            transform.RotateAround(quadCopter.transform.position, axis, angle);                       
        }
    }

    void trackMovingQuadCopter()
    {
        Vector3 t1 = prevQuadCopterPosition - transform.position;
        Vector3 t2 = quadCopter.transform.position - transform.position;
        Vector3 movement = t2 - t1;
        prevQuadCopterPosition = quadCopter.transform.position;
        if (trackingMode && movement.magnitude > eps)
        {
            transform.position += movement;                       
        }
    }

    void locateCameraCloseToQuadCopter()
    {
        //Vector3 shiftFromDrown = new Vector3(0,0,distFromQuadCopter);
        Vector3 shiftFromDrown = distFromQuadCopter * transform.forward;
        transform.position = quadCopter.transform.position - shiftFromDrown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            trackingMode = !trackingMode;
            if (trackingMode)
                locateCameraCloseToQuadCopter();
        }
        moveCamera();
        rotateCamera();       
    }    

    void LateUpdate()
    {
        //We reach here after the quadCopter's 'Update' function (in which its position might have changed)
        trackRotatingQuadCopter();    
        trackMovingQuadCopter();
    }
}
