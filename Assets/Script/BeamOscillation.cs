using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamOscillation : MonoBehaviour
{
    // to set the speed of offset of uv 
    public float UVOffsetChangeSpeed;
    private LineRenderer lineRenderer;
    // Initial UV offset of the beam
    private float initialBeamUVOffset;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialBeamUVOffset = Random.Range(0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        // set the horizontal offset of UV
        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * UVOffsetChangeSpeed + initialBeamUVOffset, 0f));
    }
}
