using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InvItem
{
    public ItemData itemData;  // The data of the item
    public int stack_size;  // The current stack size of the item

    public InvItem(ItemData item)
    {
        itemData = item;
        AddStack();
    }

    public void AddStack()
    {
        stack_size++;  // Increase the stack size by 1
    }

    public void RemoveStack()
    {
        stack_size--;  // Decrease the stack size by 1
    }
}
