using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaStateAutoRotate : MonoBehaviour
{
    private ChaState chaState;
    public float rotateSpeed = 31;


    private float degree = 0;

    private void Start()
    {
        chaState = GetComponent<ChaState>();
    }

    // Update is called once per frame
    void Update()
    {
        degree += rotateSpeed * Time.deltaTime;
        if(chaState)
            chaState.OrderRotateTo(Quaternion.Euler(0,degree,0));
        
    }
}
