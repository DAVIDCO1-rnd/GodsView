using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    float deltaPosition = 0.3f;
    float deltaAngle = 1.0f;

    void moveCamera()
    {
        //Moving in positive X direction
        if (Input.GetKey("r"))
        {
            transform.position += new Vector3(deltaPosition, 0, 0); 
        }
        //Moving in negative X direction
        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(-deltaPosition, 0, 0); 
        }

        //Moving in positive Y direction
        if (Input.GetKey("f"))
        {
            transform.position += new Vector3(0, deltaPosition, 0); 
        }
        //Moving in negative Y direction
        if (Input.GetKey("s"))
        {
            transform.position += new Vector3(0, -deltaPosition, 0); 
        }

        //Moving in positive Z direction
        if (Input.GetKey("v"))
        {
            transform.position += new Vector3(0, 0, deltaPosition); 
        }
        //Moving in negative Z direction
        if (Input.GetKey("x"))
        {
            transform.position += new Vector3(0, 0, -deltaPosition); 
        } 
    }

    void rotateCamera()
    {
        //Rotating about X axis in positive angle
        if (Input.GetKey("o"))
        {
            transform.Rotate(deltaAngle, 0, 0, Space.Self);
        }
        //Rotating about X axis in negative angle
        if (Input.GetKey("u"))
        {
            transform.Rotate(-deltaAngle, 0, 0, Space.Self);
        }       

        //Rotating about Y axis in positive angle
        if (Input.GetKey("k"))
        {
            transform.Rotate(0, deltaAngle, 0, Space.Self);
        }
        //Rotating about Y axis in negative angle
        if (Input.GetKey("h"))
        {
            transform.Rotate(0, -deltaAngle, 0, Space.Self);
        } 

        //Rotating about Z axis in positive angle
        if (Input.GetKey("n"))
        {
            transform.Rotate(0, 0, deltaAngle, Space.Self);
        }
        //Rotating about Z axis in negative angle
        if (Input.GetKey("y"))
        {
            transform.Rotate(0, 0, -deltaAngle, Space.Self);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveCamera();
        rotateCamera();
    }
}
