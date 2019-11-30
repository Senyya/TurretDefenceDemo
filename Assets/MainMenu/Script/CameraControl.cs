using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // the speed of camera movement
    public float movespeed = 10.0f;
    // define the bound of the camera movement
    public float limitx;
    // define an angle that the camera can see the whole landscape
    float orientX = 28.861f;
    float orientY = 0.0f;


    // Update is called once per frame
    void Update()
    {
        keyboardUpdate();
        mouseUpdate();
    }
    void keyboardUpdate()
    {
        // move left
        if (Input.GetKey("q"))
        {
            this.transform.Translate(-Time.deltaTime * movespeed, 0.0f, 0.0f);
        }
        // move right
        if (Input.GetKey("e"))
        {
            this.transform.Translate(Time.deltaTime * movespeed, 0.0f, 0.0f);
        }

        Vector3 position = transform.position;
        // make the camera move inside the bound
        position.x = Mathf.Clamp(position.x, -limitx, limitx);
        transform.position = position;
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
        if ( mouse > 0)
        {
            if (Camera.main.fieldOfView > 35)
                Camera.main.fieldOfView -= 2;
        }
    }
}
