using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeController : MonoBehaviour
{
    float deltaPosition = 0.3f;
    float deltaAngle = 1.0f;

    void moveCube()
    {
        //Moving in positive X direction
        if (Input.GetKey("h"))
        {
            transform.position += new Vector3(deltaPosition, 0, 0); 
        }
        //Moving in negative X direction
        if (Input.GetKey("f"))
        {
            transform.position += new Vector3(-deltaPosition, 0, 0); 
        }

        //Moving in positive Z direction
        if (Input.GetKey("y"))
        {
            transform.position += new Vector3(0, 0, deltaPosition); 
        }
        //Moving in negative Z direction
        if (Input.GetKey("g"))
        {
            transform.position += new Vector3(0, 0, -deltaPosition); 
        } 
    }

    void rotateCube()
    {
        //Rotating about X axis in positive angle
        if (Input.GetKey("c"))
        {
            transform.Rotate(deltaAngle, 0, 0, Space.Self);
        }
        //Rotating about X axis in negative angle
        if (Input.GetKey("v"))
        {
            transform.Rotate(-deltaAngle, 0, 0, Space.Self);
        }       

        //Rotating about Y axis in positive angle
        if (Input.GetKey("b"))
        {
            transform.Rotate(0, deltaAngle, 0, Space.Self);
        }
        //Rotating about Y axis in negative angle
        if (Input.GetKey("n"))
        {
            transform.Rotate(0, -deltaAngle, 0, Space.Self);
        } 

        //Rotating about Z axis in positive angle
        if (Input.GetKey("m"))
        {
            transform.Rotate(0, 0, deltaAngle, Space.Self);
        }
        //Rotating about Z axis in negative angle
        if (Input.GetKey(","))
        {
            transform.Rotate(0, 0, -deltaAngle, Space.Self);
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveCube();
        rotateCube();
    }
}
