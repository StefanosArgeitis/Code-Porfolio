using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inv : MonoBehaviour
{
    // List to store inventory items
    public List<InvItem> inventory = new List<InvItem>();

    // Event triggered when the inventory changes
    public static event Action<List<InvItem>> OnInventoryChange;

    // Dictionary to map item data to inventory items
    private Dictionary<ItemData, InvItem> itemDictionary = new Dictionary<ItemData, InvItem>();

    private void OnEnable()
    {
        // Subscribe to events for collecting different types of items
        ItemsBulb.OnBulbCollected += Add;
        LightBulb.OnRemoved += Remove;
        ElectroMagnet.OnRemoved += Remove;
        LightBulbMini.OnLightBulbCollected += Add;
        ElectroMini.OnElectroCollected += Add;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        ItemsBulb.OnBulbCollected -= Add;
        LightBulb.OnRemoved -= Remove;
        ElectroMagnet.OnRemoved -= Remove;
        LightBulbMini.OnLightBulbCollected -= Add;
        ElectroMini.OnElectroCollected -= Add;
    }

    public void Add(ItemData itemData)
    {
        // Check if the item already exists in the inventory
        if (itemDictionary.TryGetValue(itemData, out InvItem item))
        {
            // Increase the stack size of the existing item
            item.AddStack();
            Debug.Log($"{item.itemData.displayName} total stack is now {item.stack_size}");
            OnInventoryChange?.Invoke(inventory);
        }
        else
        {
            // Create a new inventory item and add it to the inventory list and dictionary
            InvItem newItem = new InvItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added {itemData.displayName} for the first time.");
            OnInventoryChange?.Invoke(inventory);
        }
    }

    public void Remove(ItemData itemData)
    {
        // Check if the item exists in the inventory
        if (itemDictionary.TryGetValue(itemData, out InvItem item))
        {
            // Decrease the stack size of the item
            item.RemoveStack();

            // If the stack size reaches 0, remove the item from the inventory list and dictionary
            if (item.stack_size == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }

            OnInventoryChange?.Invoke(inventory);
        }
    }
}
        