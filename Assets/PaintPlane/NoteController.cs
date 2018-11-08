using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteController : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler {
    /// <summary>
    /// 被拖拽出来的image,还是列表上供选择的音符图片
    /// </summary>
    public bool Draged = true;

    public GameObject ParentCanves;

    public GameObject Prefab;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //生成一个图片的实例
        //获取到点击的图片
        //在点击一个音符后,再原本的地方生成一个音符,然后原本的音符可以被拖动
        //print(eventData.pointerEnter.name);
        if (Draged)
        {
            GameObject go = Instantiate(eventData.pointerEnter);
            go.transform.SetParent(ParentCanves.transform);
            var rect = go.transform as RectTransform;
            rect.anchoredPosition = eventData.position;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        var rect = transform as RectTransform;
        if (Draged)
        {
            rect.anchoredPosition = (eventData.position);//要实现这个图片跟随鼠标位置拖动的,需要设置canvas的模式为Screen Space Overlay
        }
        //print(eventData.position);
        //print(Input.mousePosition);
        //print(Camera.main.ViewportToScreenPoint(eventData.position));
        //print(Camera.main.ScreenToWorldPoint(eventData.position));
        //print(Camera.main.ScreenToViewportPoint(eventData.position));
        //print(Camera.main.ViewportToWorldPoint(eventData.position));
        //print(Camera.main.WorldToScreenPoint(eventData.position));
        //print(Camera.main.WorldToViewportPoint(eventData.position));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("OnEndDrag");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
