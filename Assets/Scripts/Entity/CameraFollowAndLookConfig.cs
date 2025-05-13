using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAndLookConfig : MonoBehaviour
{
    [SerializeField]private Transform lookAtTransform;

    [SerializeField]private Transform followTransform;

    public Transform GetLookAtTransform()
    {
        return lookAtTransform == null ? transform : lookAtTransform;
    }

    public Transform GetFollowTransform()
    {
        return followTransform == null ? transform : followTransform;
    }
    
}
