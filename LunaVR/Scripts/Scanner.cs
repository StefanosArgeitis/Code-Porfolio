using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : MonoBehaviour
{
    // Public fields that can be set in the Unity Editor.

    // Trigger for initiating conversations.
    public ConvoTrigger convo;

    // The starting point of the raycast.
    public Transform raycastOrigin;

    // The maximum distance the raycast can travel.
    public float raycastDistance = 10f;

    // Tag that targets must have to be considered during the scan.
    public string targetTag = "Scanning";

    // The number of unique targets required to complete the scanning task.
    private int hitsRequired = 3;

    // Flag indicating whether the scanner is currently scanning.
    public bool ScannerWorking;

    // Flag to ensure that scanning all targets only triggers once.
    public bool HasScannedAll = true;

    // A collection to keep track of scanned objects, ensuring each is only counted once.
    private HashSet<Transform> uniqueHitObjects = new HashSet<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        // Get the XRGrabInteractable component to detect when the object is grabbed or released.
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        // Listen for the 'activated' event to start scanning.
        grabInteractable.activated.AddListener(x => StartScan());

        // Listen for the 'deactivated' event to stop scanning.
        grabInteractable.deactivated.AddListener(x => StopScan());
    }

    // Update is called once per frame
    void Update()
    {
        // If the scanner is working, continuously cast rays to detect targets.
        if (ScannerWorking)
        {
            CastRay();
        }
    }

    // Method to start the scanning process.
    public void StartScan()
    {
        // Log the start of scanning for debugging.
        Debug.Log("scanning");

        // Set the flag to indicate the scanner is active.
        ScannerWorking = true;
    }

    // Method to perform the raycasting and check for target hits.
    private void CastRay()
    {
        // Create a ray starting from the origin in the forward direction.
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);

        // Variable to store information about the raycast hit.
        RaycastHit hit;

        // Perform the raycast and check if it hits an object within the specified distance.
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Check if the hit object has the required tag and has not been hit before.
            if (hit.collider.CompareTag(targetTag) && uniqueHitObjects.Add(hit.transform))
            {
                // Log the name of the hit object for debugging.
                Debug.Log("Hit: " + hit.transform.name);

                // Check if the required number of unique targets have been hit.
                if (uniqueHitObjects.Count == hitsRequired)
                {
                    // Log that all required targets have been hit.
                    Debug.Log("Hit all required targets.");

                    // Check if this is the first time all targets have been scanned.
                    if (HasScannedAll)
                    {
                        // Trigger the completion of the scanning task.
                        convo.ScannerTaskDone();

                        // Prevent this from being triggered again.
                        HasScannedAll = false;
                    }
                }
            }
        }
    }

    // Method to stop the scanning process.
    public void StopScan()
    {
        // Set the flag to indicate the scanner is inactive.
        ScannerWorking = false;

        // Log the stop of scanning for debugging.
        Debug.Log("StoppedScanning");
    }
}
