using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class APIInventory : ScriptableObject
{
    //use this signature for callback functions in your user interface
    public delegate void Callback(Inventory inventory);

    //we can change file directly using editors, but now it's for inner API usage only
    private const string FILE_PATH = "Assets/Resources/inventory.json";

    private static Inventory inventory;
    
    //all invokes made to be async for server load for example
    public static void LoadInventory(Callback callback)
    {
        inventory = new Inventory();
        Debug.Log("LoadInventory");
        LoadFromFile();
        callback(inventory);
    }

    private static void LoadFromFile()
    {
        StreamReader reader = new StreamReader(FILE_PATH);
        string jsonString = reader.ReadToEnd();
        Debug.Log(jsonString);
        inventory = JsonUtility.FromJson<Inventory>(jsonString);
        reader.Close();
    }
    

    private static void SaveToFile()
    {
        StreamWriter writer = new StreamWriter(FILE_PATH, false);
        string jsonString = JsonUtility.ToJson(inventory);
        writer.WriteLine(jsonString);
        writer.Close();
    }

    public static void AddElement(ColorInventoryItem item, Callback callback)
    {
        inventory.items.Add(item);
        SaveToFile();
        callback(inventory);
    }

    public static void DeleteElement(int x, int y, Callback callback)
    {
        foreach(var item in inventory.items)
        {
            if(item.x == x && item.y == y)
            {
                inventory.items.Remove(item);
                break;
            }
        }
        SaveToFile();
        callback(inventory);
    }

    //work both for single change of position and for swipe between 2 items
    internal static void ChangePosition(int x1, int y1, int x2, int y2, Callback callback)
    {
        ColorInventoryItem item1 = null;
        ColorInventoryItem item2 = null;
        foreach (var item in inventory.items)
        {
            if (item.x == x1 && item.y == y1)
            {
                item1 = item;
                continue;
            }
            if (item.x == x2 && item.y == y2)
            {
                item2 = item;
                continue;
            }
        }
        if(item1 != null)
        {
            item1.x = x2;
            item1.y = y2;
        }
        if(item2 != null)
        {
            item2.x = x1;
            item2.y = y1;
        }
        SaveToFile();
        callback(inventory);
    }
}