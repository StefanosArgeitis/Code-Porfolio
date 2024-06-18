using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDropSystem : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField] 
    private GameObject pickUpUI; // the text to show when pick up obj

    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    [SerializeField]
    private Transform pickUpParent; // where we will put the objs

    [SerializeField]
    private GameObject inHandItem; // store what we have picked up

    private RaycastHit hit; // The result of our raycast

    private void Start()
    {
    
    }

    private void Update()
    {
        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);

        if (hit.collider != null) // reset the old raycast, u have selected sth
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
            pickUpUI.SetActive(false);
        }

        if (inHandItem != null) // we don't want to detect anything else
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null && inHandItem == null)
            {
                IPickable pickableItem = hit.collider.GetComponent<IPickable>();
                if (pickableItem != null)
                {
                    inHandItem = pickableItem.PickUp();
                    inHandItem.transform.SetParent(pickUpParent.transform, pickableItem.KeepWorldPosition);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (inHandItem != null)
            {
                inHandItem.transform.SetParent(null); // reset the connection between us and the obj
                inHandItem = null;
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                // make the obj fall to the ground
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }
        }


        // if the condition returns true => we have sth in our raycast hit
        // objects that are selected are highlighted
        if (Physics.Raycast( // to detect the objects
            playerCameraTransform.position, // origin of our raycast
            playerCameraTransform.forward, // direction of the raycast
            out hit, // save info abt what have hit in our raycast hit hit parameter
            hitRange, // max distance
            pickableLayerMask)) // only detect objects that are actually pickable 
        {
            hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
            pickUpUI.SetActive(true);
        }

    }
}
