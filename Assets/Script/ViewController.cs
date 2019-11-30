using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to controll the camera
public class ViewController : MonoBehaviour
{
    public float speed = 16.0f;
    public float mouseSpeed = 60.0f;
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(h, 0, 0)*Time.deltaTime* speed, Space.World);
        transform.Translate(new Vector3(0, 0, v) * Time.deltaTime * speed, Space.World);
        Vector3 position = transform.position;
        // make the camera move inside the bound
        position.x = Mathf.Clamp(position.x, -10.0f, 48.0f);
        position.z = Mathf.Clamp(position.z, -16.0f, 48.0f);
        transform.position = position;
        mouseUpdate();
    }

    public void mouseUpdate()
    {
        float mouse = Input.GetAxis("Mouse ScrollWheel");

        // zoom out
        if (mouse < 0)
        {
            if (Camera.main.fieldOfView <= 70)
                Camera.main.fieldOfView += 2;
        }
        // zoom in
        if (mouse > 0)
        {
            if (Camera.main.fieldOfView > 35)
                Camera.main.fieldOfView -= 2;
        }
    }
}
