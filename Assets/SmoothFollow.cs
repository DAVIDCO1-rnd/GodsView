using UnityEngine;

[ExecuteAlways]
public class SmoothFollow : MonoBehaviour
{
    public Transform quadCopter;
    float deltaPosition = 3.0f;
    float deltaAngle = 30.0f;

    float eps = 0.000001f;
    bool trackingMode = false; //for some reason (didn't figure out why), if I define trackingMode=true, the drone will not be seen when loading the simulation. You need to press "t" twice in order to see it (switch to non-tracking mode and then back to tracking mode)
    Vector3 prevQuadCopterPosition;
    Quaternion prevQuadCopterRotation;
    float distFromQuadCopter = 2.0f;
    float closestDistanceToQuadCopterInTrackingMode = 1.0f;
    float farthestDistanceToQuadCopterInTrackingMode = 10.0f;

    void Start()
    {
        if (trackingMode)
        {
            locateCameraCloseToQuadCopter();
        }                
        prevQuadCopterPosition = quadCopter.position;
        prevQuadCopterRotation = quadCopter.rotation;
    }

    void moveCamera()
    {
        //Moving in positive X direction (in camera space) - moving right in camera space (this option is NOT available in tracking mode)
        if (Input.GetKey("l") && !trackingMode)
        {
            transform.position += deltaPosition * Time.deltaTime * transform.right;              
        }
        //Moving in negative X direction (in camera space) - moving left in camera space (this option is NOT available in tracking mode)
        if (Input.GetKey("j") && !trackingMode)
        {
            transform.position -= deltaPosition * Time.deltaTime * transform.right;               
        }

        Vector3 movement = quadCopter.position - transform.position;
        //Moving in positive Z direction (in camera space) - moving forward in camera space. Can be used also in tracking mode but it won't let you pass through the drone in tracking mode since you won't track it anymore in this case.
        if (Input.GetKey("i"))
        {            
            if (!trackingMode || (trackingMode && movement.magnitude > closestDistanceToQuadCopterInTrackingMode))
                transform.position += deltaPosition * Time.deltaTime * transform.forward;           
        }
        //Moving in negative Z direction (in camera space) - moving backward in camera space. Can be used also in tracking mode but it won't let you move the camera too far from the drone in tracking mode since the camera might be too far to see (track)
        if (Input.GetKey("k"))
        {
            if (!trackingMode || (trackingMode && movement.magnitude < farthestDistanceToQuadCopterInTrackingMode))
                transform.position -= deltaPosition * Time.deltaTime * transform.forward;          
        } 
    }

    void rotateCamera()
    {        
        if (Input.GetKey("s"))
        {
            if (trackingMode)
            {
                //Rotating counterclockwise around the drone about the axis that is parallel to X axis in camera space (red axis) and passing through the drone
                transform.RotateAround(quadCopter.position, transform.right, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating counterclockwise around itself (the camera) about the axis that is parallel to X axis in camera space (red axis) and passing through itself (the camera)
                transform.Rotate(deltaAngle * Time.deltaTime, 0, 0, Space.Self);
            }            
        }

        if (Input.GetKey("w"))
        {
            if (trackingMode)
            {
                //Rotating clockwise around the drone about the axis that is parallel to X axis in camera space (red axis) and passing through the drone
                transform.RotateAround(quadCopter.position, -transform.right, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating clockwise around itself (the camera) about the axis that is parallel to X axis in camera space (red axis) and passing through itself (the camera)
                transform.Rotate(-deltaAngle * Time.deltaTime, 0, 0, Space.Self);
            }            
            
        }       

        
        if (Input.GetKey("d"))
        {
            if (trackingMode)
            {
                //Rotating counterclockwise around the drone about the axis that is parallel to Y axis in camera space (green axis) and passing through the drone
                transform.RotateAround(quadCopter.position, transform.up, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating counterclockwise around itself (the camera) about the axis that is parallel to Y axis in camera space (green axis) and passing through itself (the camera)
                transform.Rotate(0, deltaAngle * Time.deltaTime, 0, Space.Self);
            }            
        }

        if (Input.GetKey("a"))
        {
            if (trackingMode)
            {
                //Rotating clockwise around the drone about the axis that is parallel to Y axis in camera space (green axis) and passing through the drone
                transform.RotateAround(quadCopter.position, -transform.up, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating clockwise around itself (the camera) about the axis that is parallel to Y axis in camera space (green axis) and passing through itself (the camera)
                transform.Rotate(0, -deltaAngle * Time.deltaTime, 0, Space.Self);
            }            
        }

        if (Input.GetKey("x"))
        {
            if (trackingMode)
            {
                //Rotating counterclockwise around the drone about the axis that is parallel to Z axis in camera space (blue axis) and passing through the drone
                transform.RotateAround(quadCopter.position, transform.forward, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating counterclockwise around itself (the camera) about the axis that is parallel to Z axis in camera space (blue axis) and passing through itself (the camera)
                transform.Rotate(0, 0, deltaAngle * Time.deltaTime, Space.Self);
            }            
        }        

        if (Input.GetKey("z"))
        {
            if (trackingMode)
            {
                //Rotating clockwise around the drone about the axis that is parallel to Z axis in camera space (blue axis) and passing through the drone
                transform.RotateAround(quadCopter.position, -transform.forward, deltaAngle * Time.deltaTime);
            }
            else{
                //Rotating rclockwise around itself (the camera) about the axis that is parallel to Z axis in camera space (blue axis) and passing through itself (the camera)
                transform.Rotate(0, 0, -deltaAngle * Time.deltaTime, Space.Self);
            }            
        }
    }

    void trackRotatingQuadCopter()
    {
        Quaternion diffRotation = quadCopter.rotation * Quaternion.Inverse(prevQuadCopterRotation);
        prevQuadCopterRotation = quadCopter.rotation;
        float droneAngle = 0.0f;
        Vector3 droneAxis = Vector3.zero;
        diffRotation.ToAngleAxis(out droneAngle, out droneAxis); //in the last frame, the drone rotated in angle droneAngle about the axis droneAxis       
        if (trackingMode && droneAngle > eps)
        {
            transform.RotateAround(quadCopter.position, droneAxis, droneAngle); //the camera is rotating in the same angle and about the same axis like the drone did in the last frame (but the drone rotated around itself and the camera is rotating around the drone. That way the camera is always in the same orientation relative to the drone)                      
        }
    }

    void trackMovingQuadCopter()
    {
        Vector3 t1 = prevQuadCopterPosition - transform.position;
        Vector3 t2 = quadCopter.position - transform.position;
        Vector3 movement = t2 - t1; //If the drone moved, then movement holds its movement vector
        prevQuadCopterPosition = quadCopter.position;
        if (trackingMode && movement.magnitude > eps)
        {
            transform.position += movement; //the camera is moving the same movement that the drone did (that way the camera is always in the same position relative to the drone)                     
        }
    }

    void locateCameraCloseToQuadCopter()
    {
        Vector3 shiftFromQuadCopter = distFromQuadCopter * transform.forward;
        transform.position = quadCopter.position - shiftFromQuadCopter;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            trackingMode = !trackingMode;
            if (trackingMode)
                //When the user presses the "t" button for switching from non-tracking mode to tracking mode, the camera is moving (without rotation) to a location such that the drone will be in front of it in distance distFromQuadCopter            
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