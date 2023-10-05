using UnityEngine;
using System;

public class BallMotion : MonoBehaviour

{

    private Boolean isPaused = false;
    public float acceleration = 9.8f; // Acceleration due to gravity

    //X REFERS TO HORIZONTAL, IT IS Z IN EDITOR !!
  

    public float initial_Y_velocity = 0;

    public float initial_Z_velocity = 0;


    public float MIN_Y = - 10f;

    private Vector3 velocity;

    

    private void Start()
    {
        //velocity = Vector3.zero;
        velocity = new Vector3(0, initial_Y_velocity, initial_Z_velocity);
    }

    private void Update()
    {
        if(isPaused){
            return;
        }

        if(velocity[0] != 0){
            Debug.Log("ENEXPECTED velocity[0]==" + velocity[0]);
            PauseGame();
        }
         if(transform.position[0] != 0){
            Debug.Log("ENEXPECTED position[0]==" + transform.position[0]);
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.R)){
            ResumeGame();
        }


        // Calculate the new velocity based on acceleration and time
        velocity += acceleration * Time.deltaTime * Vector3.down;

        // Update the position based on the new velocity
        transform.position += velocity * Time.deltaTime;


        if(transform.position.y < MIN_Y){
            Destroy(gameObject);
        }
    }

/*     void OnCollisionEnter(Collision collision)
    {
         Debug.Log("COLLISION");
        // Check if the collision involves a specific tag
      
    } */

    private void OnTriggerEnterOld(Collider other)
    {
        Debug.Log("Collision with: " + other.gameObject.name);
        // Perform desired actions when a collision occurs
        velocity = velocity * -0.8f;
    }

    private void OnTriggerEnterOld2(Collider other)
    {
      // Access the other collider's transform
        Transform otherTransform = other.transform;

        // Calculate the angle with the horizon
        float otherAngleWithHorizon = Vector3.Angle(otherTransform.up, Vector3.up) * -1;

        // Do something with the angle
        Debug.Log("[POV Ball] Other's Angle with horizon: " + otherAngleWithHorizon);

        double selfAngleHor = Math.Atan2(velocity[1],velocity[2]) * (180.0 / Math.PI);
        Debug.Log("[POV Ball] Self Angle with horizon: " + selfAngleHor);
        double orgMag = Math.Sqrt (velocity[1] * velocity[1] + velocity[2] * velocity[2]);


        double relAng = selfAngleHor - otherAngleWithHorizon;
        double rflAngRel = 180 -  relAng;
       // double rflAngRelHor = otherAngleWithHorizon + rflAngRel;
       double rflAngRelHor = rflAngRel + otherAngleWithHorizon;
        Debug.Log("rflAngRelHor" + rflAngRelHor);
        
        Vector3 rflVel = new Vector3(0, (float)(orgMag * Math.Sin(rflAngRelHor)),  1 * (float)(orgMag * Math.Cos(rflAngRelHor)));
        velocity = rflVel;
        
    }


    /*
        [prompt]
        a ball with perfect elesticity is traving at veolocity (v1,v2) in a 2d space and hits a hard flat surface tilted in angle alpha1 
        relative to the horizon. in terms of v1, v2, alpha1, at what velocity will the ball be reflected from the surface?  no gravity 
        or additiaonl forces apply.

        A:
        v1' = v1 * cos(α1)
        v2' = -v2 * sin(α1)



        ORIENTATION
        space is notated x,y,z meaning:
        x = 0 (cause it's 2d)
        y = up is positive, down negative
        z = right is positive, left nefative 


    */

     private void OnTriggerEnterOld3(Collider other)
    {
      // Access the other collider's transform
        Transform otherTransform = other.transform;

        // Calculate the angle with the horizon
        float otherAngleWithHorizon = Vector3.Angle(otherTransform.up, Vector3.up) * -1;

        Debug.Log("[POV Ball] Other's Angle with horizon: " + otherAngleWithHorizon);
        
        Debug.Log("[POV Ball] my veloctity(y,z) = ("+ velocity.y  +"," + velocity.z  + ")");


        float alpha = otherAngleWithHorizon;
        float zOut = (float)(velocity.z * Math.Cos(alpha));
        float yOut = (float)(-velocity.y * Math.Sin(alpha));

       Vector3 rflVel = new Vector3(0, yOut, zOut);
       //Vector3 rflVel = new Vector3(0, 10, -1);

       Debug.Log("calculated velocity= [y,z] = " + yOut + "," +  zOut);
        velocity = rflVel;
        
    }

    private void OnTriggerEnter(Collider other)
    {
      // Access the other collider's transform
        Transform otherTransform = other.transform;

        // Calculate the angle with the horizon
        //float otherAngleWithHorizon = Vector3.Angle(otherTransform.up, Vector3.up) * -1;
        float otherAngleWithHorizon = other.transform.rotation.eulerAngles.x % 360;

        Debug.Log("[POV Ball] Other's Angle with horizon: " + otherAngleWithHorizon);  
        
        Debug.Log("[POV Ball] my veloctity(z,y) = ("+ velocity.z  +"," + velocity.y  + ")");

        double orgMag = Math.Sqrt (velocity[1] * velocity[1] + velocity[2] * velocity[2]);


        double selfAngleHor = Math.Atan2(velocity[1],velocity[2]) * (180.0 / Math.PI);
        //double selfAngleHor = Math.Atan2(velocity[2],velocity[1]) * (180.0 / Math.PI);
        //selfAngleHor = 180 - selfAngleHor - 360;
        selfAngleHor = selfAngleHor * -1;
        Debug.Log("[POV Ball] Self Angle with horizon: " + selfAngleHor);

        double relAngWIncPlane = ((selfAngleHor - otherAngleWithHorizon)  + 360 ) % 360 ;
        Debug.Log("[POV Ball] relAngWIncPlane: " + relAngWIncPlane);

        double refSelfAngleWIncPlane = 180 - relAngWIncPlane;
        Debug.Log("[POV Ball] refSelfAngleWIncPlane: " + refSelfAngleWIncPlane);

        double refAngleWHor = ((refSelfAngleWIncPlane + otherAngleWithHorizon) + 360 ) %360;
        Debug.Log("[POV Ball] refAngleWHor: " + refAngleWHor);

        /* float outZ = (float)(orgMag * Math.Cos(refAngleWHor));  
        float outY = (float)(orgMag * Math.Sin(refAngleWHor));  
 */
        float outZ = (float)(orgMag * Math.Cos((180 - refAngleWHor) * (Math.PI / 180)));  
        float outY = (float)(orgMag * Math.Sin((180 - refAngleWHor) * (Math.PI / 180)));  


       Vector3 rflVel = new Vector3(0,  outY, outZ);
       //Vector3 rflVel = new Vector3(0, 10, -1);

       Debug.Log("calculated velocity= [y,z] = " + outY + "," +  outZ);
        velocity = rflVel;
        
    }

    private void PauseGame()
    {
        //Time.timeScale = 0f; // Set time scale to 0 to pause the game
        isPaused = true;
        Debug.Log("Game paused.");
    }

    private void ResumeGame()
    {
        //Time.timeScale = 1f; // Set time scale to 1 to resume the game at normal speed
        isPaused = false;
        Debug.Log("Game resumed.");
    }
}