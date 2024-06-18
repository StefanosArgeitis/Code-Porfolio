using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideTrigger : MonoBehaviour
{
    public ConvoTrigger convo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            convo.OutsideTaskDone();
            Debug.Log("Outside");
        }
    }
}
