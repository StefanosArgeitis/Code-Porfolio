using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class UFOController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 45.0f;

    [Header("Speed Adjustment")]
    public float minSpeed = 1.0f;
    public float maxSpeed = 10.0f;
    public float speedChangeRate = 2.0f;

    private float currentSpeed = 5.0f;

    private Transform ufoTransform;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject leftControllerObject; // Public GameObject reference for the left controller.
    public GameObject rightControllerObject; // Public GameObject reference for the right controller.

    private void Start()
    {
        ufoTransform = transform;
    }

    private void Update()
    {
        // Move the UFO using the moveDirection vector.
        ufoTransform.Translate(moveDirection * currentSpeed * Time.deltaTime);

        // Rotate with right controller's joystick input.
        Vector2 rotationInput = Vector2.zero;

        if (rightControllerObject && rightControllerObject.TryGetComponent<XRController>(out XRController rightController))
        {
            if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationInput))
            {
                ufoTransform.Rotate(Vector3.up, rotationInput.x * rotationSpeed * Time.deltaTime);
            }
        }

        // Adjust speed with left controller's joystick input.
        if (leftControllerObject && leftControllerObject.TryGetComponent<XRController>(out XRController leftController))
        {
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool thumbstickClicked) && thumbstickClicked)
            {
                if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 thumbstickValue))
                {
                    currentSpeed = Mathf.Clamp(currentSpeed + thumbstickValue.y * speedChangeRate * Time.deltaTime, minSpeed, maxSpeed);
                }
            }
        }
    }
}
