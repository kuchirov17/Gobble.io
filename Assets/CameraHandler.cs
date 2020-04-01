using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform target;
    private Vector3 startOffset;

    private void Awake()
    {
        startOffset = transform.position - target.position;

    }

    private void Update()
    {
        //transform.position = target.position + startOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + startOffset.z);
    }
}
