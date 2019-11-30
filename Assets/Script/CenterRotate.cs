using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRotate : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.transform.Rotate(Vector3.up * 90);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.transform.Rotate(Vector3.up * -90);
        }
    }
}
