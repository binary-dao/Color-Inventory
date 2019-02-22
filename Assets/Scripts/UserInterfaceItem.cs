using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInterfaceItem : MonoBehaviour, IPointerDownHandler{
    private bool isDragging;
    private CanvasRenderer canvasRenderer;
    private Vector3 startDragPosition;
    public bool isNew;
    public CellLogic cell;

    // Use this for initialization
    void Start () {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		if(isDragging)
        {
            if(Input.GetMouseButton(0))
            {        
                canvasRenderer.transform.position = Input.mousePosition;
            }
            else
            {
                isDragging = false;
                InventoryExample.StopDragging();
            }
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        startDragPosition = canvasRenderer.transform.position;
        isDragging = true;
        InventoryExample.StartDragging(this);
        canvasRenderer.transform.SetAsLastSibling();
    }

    internal void DropTo(Vector3 position)
    {
        canvasRenderer.transform.position = position;
    }

    internal void MoveBack()
    {
        canvasRenderer.transform.position = startDragPosition;
    }
}
