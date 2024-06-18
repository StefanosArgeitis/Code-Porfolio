using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;  // Reference to the Image component used to display the item icon
    public TextMeshProUGUI labelTxt;  // Reference to the item label
    public TextMeshProUGUI stackSizeTxt;  // Reference to the item stack size

    public void ClearSlot()
    {
        // Disable the icon, label, and stack size texts to clear the slot
        icon.enabled = false;
        labelTxt.enabled = false;
        stackSizeTxt.enabled = false;
    }

    public void DrawSlot(InvItem item)
    {
        if (item == null)
        {
            ClearSlot();  // If the item is null, clear the slot and return
            return;
        }

        // Enable the icon, label, and stack size texts to show the item details
        icon.enabled = true;
        labelTxt.enabled = true;
        stackSizeTxt.enabled = true;

        // Set the icon sprite to the item's icon sprite
        icon.sprite = item.itemData.icon;

        // Set the label text to the item's display name
        labelTxt.text = item.itemData.displayName;

        // Set the stack size text to the item's stack size converted to a string
        stackSizeTxt.text = item.stack_size.ToString();
    }
}
