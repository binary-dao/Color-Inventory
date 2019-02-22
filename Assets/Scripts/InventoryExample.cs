using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class InventoryExample : MonoBehaviour {
    const int UNITS_TO_PIXELS = 110;
    const int MAX_ROWS = 4;
    const int MAX_COLS = 4;

    private GameObject[,] cellArray = new GameObject[MAX_COLS, MAX_ROWS];
    private List<GameObject> interfaceItems = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Debug.Log("InventoryExample Start");
        GenerateCells();

        APIInventory.Callback callback = UpdateInventory;
        APIInventory.LoadInventory(callback);
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    private void GenerateCells()
    {
        GameObject anchor = GameObject.Find("Anchor");
        float shiftX = anchor.transform.position.x;
        float shiftY = anchor.transform.position.y;
        for (int x = 0; x < MAX_COLS; x++)
        {
            for (int y = 0; y < MAX_ROWS; y++)
            {
                var cell = (GameObject)Instantiate(Resources.Load("CellPrefab"));
                cell.transform.SetParent(anchor.transform);
                cell.transform.position = new Vector2(shiftX + x * UNITS_TO_PIXELS, shiftY + y * UNITS_TO_PIXELS);
                /*var cellBehaviour = cell.GetComponent<CellBehaviourScript>();
                cellBehaviour.x = x;
                cellBehaviour.y = y;*/
                cellArray[x, y] = cell;            }
        }
    }

    private void UpdateInventory(Inventory inventory)
    {
        Debug.Log("InventoryExample UpdateInventory ");
        for(int i = 0; i<interfaceItems.Count; i++)
        {
            Destroy(interfaceItems[i]);
        }
        interfaceItems = new List<GameObject>();

        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject interfaceItem = (GameObject)Instantiate(Resources.Load("ColorPrefab"));
            ColorInventoryItem inventoryItem = inventory.items[i];
            interfaceItem.transform.SetParent(cellArray[inventoryItem.x, inventoryItem.y].transform);
            interfaceItem.transform.position = cellArray[inventoryItem.x, inventoryItem.y].transform.position;
            interfaceItem.GetComponent<CanvasRenderer>().SetColor(inventoryItem.color);
        }
    }

    public static void UpdateColorBall()
    {
        GameObject ball = GameObject.Find("TestBall");
        CanvasRenderer canvasRenderer = ball.GetComponent<CanvasRenderer>();
        Color newColor = GetColorFromSliders();
        canvasRenderer.SetColor(newColor);
    }

    private static Color GetColorFromSliders()
    {
        GameObject redSlider = GameObject.Find("SliderRed");
        float red = redSlider.GetComponentInChildren<SliderLogic>().slider.value;
        GameObject sliderGreen = GameObject.Find("SliderGreen");
        float green = sliderGreen.GetComponentInChildren<SliderLogic>().slider.value;
        GameObject sliderBlue = GameObject.Find("SliderBlue");
        float blue = sliderBlue.GetComponentInChildren<SliderLogic>().slider.value;
        return new Color(red, green, blue);
    }
}
