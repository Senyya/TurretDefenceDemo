using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to controll ice halo's wave
public class WaveController : MonoBehaviour
{
    public float FadeOutTime;       // Color fade speed
    public float ScaleTime;         // Scaling speed
    public Vector3 ScaleSize;       // The size wave will be scaled to
    private Transform currentTransform;        
    private MeshRenderer meshRenderer;
    // Tint color index (refrence) of Shader property 
    int tintColorNumber;
    // Default color of the wave
    private Color defaultColor;
    // Current color of the wave
    private Color currentColor;

    bool isFadeOut;                 // Fading flag

    // Start is called before the first frame update
    void Start()
    {
        currentTransform = GetComponent<Transform>();
        meshRenderer = GetComponent<MeshRenderer>();
        // Get Tint color index (refrence) of shader property 
        tintColorNumber = Shader.PropertyToID("_TintColor");
        // Get and store default color
        defaultColor = meshRenderer.material.GetColor(tintColorNumber);
        ResetWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Scale the wave
        currentTransform.localScale = Vector3.Lerp(currentTransform.localScale, ScaleSize, Time.deltaTime * ScaleTime);

        // Check the fading state 
        if (isFadeOut)
        {
            // Lerp color and update the shader
            currentColor = Color.Lerp(currentColor, new Color(0, 0, 0, -0.1f), Time.deltaTime * FadeOutTime);
            meshRenderer.material.SetColor(tintColorNumber, currentColor);

            // Make sure alpha value is not overshooting 
            if (currentColor.a <= 0f)
            {
                ResetWave();
            }
        }

    }

    // Reduce the wave circle
    void ResetWave()
    {
        // Set scale to zero
        transform.localScale = new Vector3(0f, 0f, 0f);
        isFadeOut = true;

        // Reset default color
        meshRenderer.material.SetColor(tintColorNumber, defaultColor);
        currentColor = defaultColor;
    }
}
