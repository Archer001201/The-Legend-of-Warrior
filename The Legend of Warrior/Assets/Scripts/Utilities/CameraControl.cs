using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;

    private void Awake() {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void Start() {
        GetCameraBound();
    }

    private void GetCameraBound(){
        var obj = GameObject.FindGameObjectWithTag("Bound");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache();
    }
}
