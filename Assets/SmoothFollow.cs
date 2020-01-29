using UnityEngine;

[ExecuteAlways]
public class SmoothFollow : MonoBehaviour
{
    public Transform quadCopter;
    float deltaPosition = 3.0f;
    float deltaAngle = 30.0f;

    float eps = 0.000001f;
    bool trackingMode = false;
    Vector3 prevQuadCopterPosition;
    Quaternion prevQuadCopterRotation;
    float distFromQuadCopter = 2.0f;

    void Start()
    {                
        prevQuadCopterPosition = quadCopter.position;
        prevQuadCopterRotation = quadCopter.rotation;
    }

    void myLookAt(Transform target)
    {
        //I'm using myLookAt instead of transform.LookAt in order to prevent flipping the camera upside down.
        //When using transform.LookAt, if I am in tracking mode and I'm rotating a camera 180 degrees around the 'drone' (elavation), and then I move the camera somewhere, then the transform.LookAt flips the camera back in 180 degrees (I guess it's not the user's intention).
        var relativeUp = target.TransformDirection (Vector3.forward);
        var relativePos = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos,relativeUp);
    }

    void moveCamera()
    {
        //Moving in positive X direction
        if (Input.GetKey("l"))
        {
            transform.position += new Vector3(deltaPosition * Time.deltaTime, 0, 0);
            if (trackingMode)
            {
                myLookAt(quadCopter);
            }
        }
        //Moving in negative X direction
        if (Input.GetKey("j"))
        {
            transform.position += new Vector3(-deltaPosition * Time.deltaTime, 0, 0);
            if (trackingMode)
            {
                myLookAt(quadCopter);
            }
        }

        // //Moving in positive Y direction
        // if (Input.GetKey("d"))
        // {
        //     transform.position += new Vector3(0, deltaPosition * Time.deltaTime, 0);
        //     if (trackingMode)
        //     {
        //         myLookAt(quadCopter);
        //     }            
        // }
        // //Moving in negative Y direction
        // if (Input.GetKey("a"))
        // {
        //     transform.position += new Vector3(0, -deltaPosition * Time.deltaTime, 0);
        //     if (trackingMode)
        //     {
        //         myLookAt(quadCopter);
        //     }            
        // }

        //Moving in positive Z direction
        if (Input.GetKey("i"))
        {
            transform.position += new Vector3(0, 0, deltaPosition * Time.deltaTime);
            if (trackingMode)
            {
                myLookAt(quadCopter);
            }            
        }
        //Moving in negative Z direction
        if (Input.GetKey("m"))
        {
            transform.position += new Vector3(0, 0, -deltaPosition * Time.deltaTime);
            if (trackingMode)
            {
                myLookAt(quadCopter);
            }            
        } 
    }

    void rotateCamera()
    {
        //Rotating about X axis in positive angle
        if (Input.GetKey("w"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, transform.right, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(deltaAngle * Time.deltaTime, 0, 0, Space.Self);
            }            
        }
        //Rotating about X axis in negative angle
        if (Input.GetKey("s"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, -transform.right, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(-deltaAngle * Time.deltaTime, 0, 0, Space.Self);
            }            
            
        }       

        //Rotating about Y axis in positive angle
        if (Input.GetKey("d"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, transform.up, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(0, deltaAngle * Time.deltaTime, 0, Space.Self);
            }            
        }
        //Rotating about Y axis in negative angle
        if (Input.GetKey("a"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, -transform.up, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(0, -deltaAngle * Time.deltaTime, 0, Space.Self);
            }            
        }

        //Rotating about Z axis in positive angle 
        if (Input.GetKey("x"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, transform.forward, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(0, 0, deltaAngle * Time.deltaTime, Space.Self);
            }            
        }        


        //Rotating about Z axis in negative angle 
        if (Input.GetKey("z"))
        {
            if (trackingMode)
            {
                transform.RotateAround(quadCopter.position, -transform.forward, deltaAngle * Time.deltaTime);
            }
            else{
                transform.Rotate(0, 0, -deltaAngle * Time.deltaTime, Space.Self);
            }            
        }
    }

    void trackRotatingQuadCopter()
    {
        Quaternion diffRotation = quadCopter.rotation * Quaternion.Inverse(prevQuadCopterRotation);
        prevQuadCopterRotation = quadCopter.rotation;
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        diffRotation.ToAngleAxis(out angle, out axis);         
        if (trackingMode && angle > eps)
        {
            transform.RotateAround(quadCopter.position, axis, angle);                       
        }
    }

    void trackMovingQuadCopter()
    {
        Vector3 t1 = prevQuadCopterPosition - transform.position;
        Vector3 t2 = quadCopter.position - transform.position;
        Vector3 movement = t2 - t1;
        prevQuadCopterPosition = quadCopter.position;
        if (trackingMode && movement.magnitude > eps)
        {
            transform.position += movement;                       
        }
    }

    void locateCameraCloseToQuadCopter()
    {
        //Vector3 shiftFromDrown = new Vector3(0,0,distFromQuadCopter);
        Vector3 shiftFromDrown = distFromQuadCopter * transform.forward;
        transform.position = quadCopter.position - shiftFromDrown;
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
        //We reach here after the quadCopter's 'Update' function (in which its position or rotation might have changed)
        trackRotatingQuadCopter();    
        trackMovingQuadCopter();
    }
}