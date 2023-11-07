using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisMove : MonoBehaviour
{
    public float rotationSpeed = 2.0f; // Adjust the speed as needed

    private Vector3 rotVector = new Vector3(0,1,0);

    void Update()
    {

        // Rotate the object around the global X-axis at a constant speed
        transform.Rotate(rotVector * rotationSpeed * -1 * Time.deltaTime);
    }
}
