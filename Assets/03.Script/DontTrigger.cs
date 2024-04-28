using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DontTrigger : MonoBehaviour , IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.instance.isPossibleCreate = false;
        GameManager.instance.isTriggered = true;
        Invoke(nameof(ReturnDefaultState),1f);
    }
    
    void ReturnDefaultState()
    {
        GameManager.instance.isTriggered = false;
    }
}
