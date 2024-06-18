using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can create scriptable objects in the editor
 
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
}
