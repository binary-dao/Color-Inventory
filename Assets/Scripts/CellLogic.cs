using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellLogic : MonoBehaviour
{
    private RectTransform rectTransform;
    public bool isDeleteCell;
    public bool isOccupied;
    public int x;
    public int y;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && InventoryExample.IsDragging && IsMouseInCell())
        {
            Debug.Log("setting dropCell");
            InventoryExample.dropCell = this;
        }
    }

    private bool IsMouseInCell()
    {
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
        return (rectTransform.rect.Contains(localMousePosition));
    }
}
