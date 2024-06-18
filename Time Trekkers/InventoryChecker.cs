using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryChecker : MonoBehaviour
{
    public Inv inventoryScript; // Reference to the Inv script
    public LightBulb comps;
    public ElectroMagnet magnet;
    public Narrator nar;

    public void CheckInventory()
    {
        // Strings representing the target items
        string[] targetStrings = { "Wire", "Bulb", "Cap" };
        string[] targetStringsMagnet = { "Wire", "Iron Core" };

        // Flags to track if all target items are present in the inventory
        bool containsAllStrings = true;
        bool containsAllStringsMag = true;

        // Check if all target items are present in the inventory
        foreach (string targetString in targetStrings)
        {
            bool containsString = false;

            // Iterate through the inventory items
            foreach (InvItem item in inventoryScript.inventory)
            {
                if (item.itemData.displayName == targetString)
                {
                    // The inventory contains the target item
                    containsString = true;
                    break;
                }
            }

            if (!containsString)
            {
                // The inventory does not contain the target item
                containsAllStrings = false;
                break;
            }
        }

        // Check if all target items for the magnet are present in the inventory
        foreach (string targetString in targetStringsMagnet)
        {
            bool containsString = false;

            // Iterate through the inventory items
            foreach (InvItem item in inventoryScript.inventory)
            {
                if (item.itemData.displayName == targetString)
                {
                    // The inventory contains the target item
                    containsString = true;
                    break;
                }
            }

            if (!containsString)
            {
                // The inventory does not contain the target item
                containsAllStringsMag = false;
                break;
            }
        }

        // Set the flag for magnet if all target items are collected
        if (containsAllStringsMag)
        {
            magnet.allMatsCollected = true;
        }

        // Perform actions based on whether all target items are collected
        if (containsAllStrings)
        {
            Debug.Log("The inventory contains all target strings.");

            // Set the flag for components if all target items are collected
            comps.allMatsCollected = true;

            // Play the narration for all light components
            nar.PlayAllLightComps();
        }
        else
        {
            Debug.Log("The inventory does not contain all target strings.");
        }
    }
}
