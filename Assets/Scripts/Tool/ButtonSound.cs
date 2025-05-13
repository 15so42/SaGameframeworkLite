using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEntry.Sound.PlayUISFX(1000);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEntry.Sound.PlayUISFX(1001);
    }
}
