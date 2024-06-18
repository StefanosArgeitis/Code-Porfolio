using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : MonoBehaviour
{
    public ConvoTrigger convo;
    public Transform raycastOrigin;
    public float raycastDistance = 10f; // Distance of the raycast, which can be modified in the editor.
    public string targetTag = "Scanning"; // Tag of the specific objects to detect.

    private int hitsRequired = 3;
    public bool ScannerWorking;
    public bool HasScannedAll = true;

    private HashSet<Transform> uniqueHitObjects = new HashSet<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => StartScan());
        grabInteractable.deactivated.AddListener(x => StopScan());

    }
    void Update()
    {
        if (ScannerWorking){
            CastRay();
        }
    }

    public void StartScan(){
        Debug.Log("scanning");
        ScannerWorking = true;
        
    }

    /*private void OnDrawGizmos()
    {
        
        // Create a ray for visualization.
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);

        // Set the color for the ray.
        Gizmos.color = Color.red;

        // Draw the ray.
        Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);

        
    }*/

    private void CastRay()
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(targetTag) && uniqueHitObjects.Add(hit.transform))
            {
                Debug.Log("Hit: " + hit.transform.name);

                if (uniqueHitObjects.Count == hitsRequired)
                {
                    Debug.Log("Hit all required targets.");

                    if (HasScannedAll){
                        convo.ScannerTaskDone();
                        HasScannedAll = false;
                    }
                    
                }
            }
        }
    }

    public void StopScan(){
        ScannerWorking = false;
        Debug.Log("StoppedScanning");
    }
}
