using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerCamera : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField]
    private GameObject _target;
    
    private void Start()
    {
        _offset = transform.position - _target.transform.position;
    }

    void LateUpdate()
    {
        transform.position = _target.transform.position + _offset;
    }
}
