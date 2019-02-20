using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory{
    //can be used any InventoryItem inheritor later
    public List<ColorInventoryItem> items = new List<ColorInventoryItem>();

    public Inventory()
    {
    }
}
