using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsterHeight : MonoBehaviour
{
    public Transform raycastPoint;  // The point from which the ray will be cast.
    public LayerMask groundLayer;  // The layer(s) representing the ground.

    public Transform Holster;  // The object you want to adjust the Y position of.

    // Start is called before the first frame update
    void Start()
    {
        // Cast a ray from the raycastPoint downward to detect the ground.
        if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // Calculate the new Y position for the Holster.
            float newYPosition = hit.point.y + (hit.distance * 0.5f);

            // Set the new Y position for the Holster.
            Vector3 newPosition = Holster.position;
            newPosition.y = newYPosition;
            Holster.position = newPosition;
        }
    }

}
