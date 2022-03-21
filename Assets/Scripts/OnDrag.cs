using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject canvas;
    private float canvasScale;
    public GameObject selectedGameObject;

   
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Being Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("End Drag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        print("Dragging");
        //if (m_itemGameObject != null)
        //{
        //    m_itemGameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / m_fCanvasScale;
        //}

    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasScale = canvas.GetComponent<Canvas>().scaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
