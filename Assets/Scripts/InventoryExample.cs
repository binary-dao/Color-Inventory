using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class InventoryExample : MonoBehaviour {
    const int UNITS_TO_PIXELS = 110;
    const int MAX_ROWS = 4;
    const int MAX_COLS = 4;

    private static GameObject anchor;
    private GameObject[,] cellArray = new GameObject[MAX_COLS, MAX_ROWS];
    private List<GameObject> interfaceItems = new List<GameObject>();
    private static GameObject newColorItem;

    private static bool isDragging;
    private static bool isCurrentDropFrame;
    private static UserInterfaceItem draggingItem;
    public static CellLogic startDragCell;
    public static CellLogic dropCell;

    private static APIInventory.Callback callback;

    internal static bool IsDragging
    {
        get
        {
            return isDragging;
        }
    }

    // Use this for initialization
    void Start () {
        anchor = GameObject.Find("Anchor");
        GenerateCells();

        GenerateNewColorItem();

        callback = UpdateInventory;
        APIInventory.LoadInventory(callback);
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    //waiting for both InventoryItem stop dragging and 
    internal void LateUpdate()
    {
        if(isCurrentDropFrame)
        {
            if (dropCell)
            {
                DropToCellLogic();
            }
            else
            {
                MoveItemBack();
            }
        }
            
        isCurrentDropFrame = false;
        dropCell = null;
    }

    private void GenerateCells()
    {
        float shiftX = anchor.transform.position.x;
        float shiftY = anchor.transform.position.y;
        for (int x = 0; x < MAX_COLS; x++)
        {
            for (int y = 0; y < MAX_ROWS; y++)
            {
                var cell = (GameObject)Instantiate(Resources.Load("CellPrefab"));
                cell.transform.SetParent(anchor.transform);
                cell.transform.position = new Vector2(shiftX + x * UNITS_TO_PIXELS, shiftY + y * UNITS_TO_PIXELS);
                cell.GetComponent<CellLogic>().x = x;
                cell.GetComponent<CellLogic>().y = y;
                cellArray[x, y] = cell;
            }
        }
        var deleteCell = (GameObject)Instantiate(Resources.Load("CellPrefab"));
        deleteCell.transform.SetParent(anchor.transform);
        deleteCell.transform.position = new Vector2 (anchor.transform.position.x+500, anchor.transform.position.y + 150);
        deleteCell.GetComponent<CellLogic>().isDeleteCell = true;
    }

    private void UpdateInventory(Inventory inventory)
    {
        Debug.Log("InventoryExample UpdateInventory ");

        for (int x = 0; x < MAX_COLS; x++)
        {
            for (int y = 0; y < MAX_ROWS; y++)
            {
                cellArray[x, y].GetComponent<CellLogic>().isOccupied = false;
            }
        }

        for (int i = 0; i<interfaceItems.Count; i++)
        {
            Destroy(interfaceItems[i]);
        }

        interfaceItems = new List<GameObject>();

        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject interfaceItem = (GameObject)Instantiate(Resources.Load("ColorPrefab"));
            ColorInventoryItem inventoryItem = inventory.items[i];
            interfaceItem.transform.SetParent(anchor.transform);
            interfaceItem.transform.position = cellArray[inventoryItem.x, inventoryItem.y].transform.position;
            cellArray[inventoryItem.x, inventoryItem.y].GetComponent<CellLogic>().isOccupied = true;
            interfaceItem.GetComponent<CanvasRenderer>().SetColor(inventoryItem.color);
            interfaceItem.GetComponent<UserInterfaceItem>().cell = cellArray[inventoryItem.x, inventoryItem.y].GetComponent<CellLogic>();
            interfaceItems.Add(interfaceItem);
        }
    }

    private static void GenerateNewColorItem()
    {
        newColorItem = (GameObject)Instantiate(Resources.Load("ColorPrefab"));
        //ColorInventoryItem inventoryItem = inventory.items[i];
        newColorItem.transform.SetParent(anchor.transform);
        newColorItem.transform.position = anchor.transform.position + new Vector3(-195, 150);
        newColorItem.GetComponent<UserInterfaceItem>().isNew = true;
        UpdateColorBall();
    }

    internal static void UpdateColorBall()
    {
        CanvasRenderer canvasRenderer = newColorItem.GetComponent<CanvasRenderer>();
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

    internal static void StartDragging(UserInterfaceItem item)
    {
        draggingItem = item;
        isDragging = true;
    }

    internal static void StopDragging()
    {
        Debug.Log("StopDragging");
        isDragging = false;
        isCurrentDropFrame = true;
    }

    

    private static void DropToCellLogic()
    {
        Debug.Log("DropToCellLogic");
        Debug.Log("draggingItem.isNew " + draggingItem.isNew);
        Debug.Log("dropCell.isDeleteCell " + dropCell.isDeleteCell);
        
        if(draggingItem.isNew)
        {
            if(dropCell.isDeleteCell)
            {
                MoveItemBack();
            }
            else if(dropCell.isOccupied)
            {
                Debug.Log("Can't exchange position with new ");
            }
            else
            {
                ColorInventoryItem item = new ColorInventoryItem();
                item.color = GetColorFromSliders();
                item.x = dropCell.x;
                item.y = dropCell.y;
                APIInventory.AddElement(item, callback);
                Destroy(draggingItem.gameObject);
                GenerateNewColorItem();
            }
        }
        else
        {
            if (dropCell.isDeleteCell)
            {
                APIInventory.DeleteElement(draggingItem.cell.x, draggingItem.cell.y, callback);
                Destroy(draggingItem.gameObject);
            }
            else
            {
                APIInventory.ChangePosition(draggingItem.cell.x, draggingItem.cell.y, dropCell.x, dropCell.y, callback);
                Destroy(draggingItem.gameObject);
            }
        }
    }

    private static void DropItemToCell()
    {
        draggingItem.DropTo(dropCell.transform.position);
    }

    private static void MoveItemBack()
    {
        Debug.Log("MoveItemBack");
        draggingItem.MoveBack();
    }
}
