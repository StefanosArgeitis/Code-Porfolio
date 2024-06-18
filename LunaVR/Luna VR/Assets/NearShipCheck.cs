using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearShipCheck : MonoBehaviour
{
    public ConvoTrigger convo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            convo.WalkedToShip();
            Debug.Log("at ship");
        }
    }
}
