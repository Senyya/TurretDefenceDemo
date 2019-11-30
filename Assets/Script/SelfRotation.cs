using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public float rotationSpeed = 1.2f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down * rotationSpeed, Space.World);
    }
}
