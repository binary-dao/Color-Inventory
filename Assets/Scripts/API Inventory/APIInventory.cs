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
       /* ColorInventoryItem test = new ColorInventoryItem();
        test.x = 1;
        test.y = 13;
        test.color = Color.red;
        inventory.items.Add(test);*/

        StreamWriter writer = new StreamWriter(FILE_PATH, false);
        string jsonString = JsonUtility.ToJson(inventory);
        writer.WriteLine(jsonString);
        writer.Close();
    }
}