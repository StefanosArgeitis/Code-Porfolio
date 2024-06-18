using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFlight : MonoBehaviour
{
    public float flightSpeed = 10.0f;
    public float rotationSpeed = 2.0f;

    void Update()
    {
        // Get input from VR controllers or gaze-based input.
        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Move forward.
        transform.Translate(Vector3.forward * forwardInput * flightSpeed * Time.deltaTime);

        // Rotate
        Vector3 rotation = new Vector3(0, horizontalInput * rotationSpeed, 0);
        transform.Rotate(rotation);
    }
}

