using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;  // Reference to the prefab for inventory slots
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(5);  // List to store inventory slots

    private void OnEnable()
    {
        // Subscribe to the OnInventoryChange event in Inv script
        Inv.OnInventoryChange += DrawInventory;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnInventoryChange event in Inv script
        Inv.OnInventoryChange -= DrawInventory;
    }

    void ResetInventory()
    {
        // Destroy all existing inventory slots and clear the list
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        inventorySlots = new List<InventorySlot>(5);
    }

    void DrawInventory(List<InvItem> inventory)
    {
        ResetInventory();

        // Create inventory slots based on the capacity
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            CreateInventorySlot();
        }

        // Draw the inventory items in the slots
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].DrawSlot(inventory[i]);
        }
    }

    void CreateInventorySlot()
    {
        // Instantiate a new inventory slot and set it as a child of the InventoryManager
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.SetParent(transform, false);

        // Get the InventorySlot component from the new slot, clear it, and add it to the inventorySlots list
        InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
        newSlotComponent.ClearSlot();
        inventorySlots.Add(newSlotComponent);
    }
}
