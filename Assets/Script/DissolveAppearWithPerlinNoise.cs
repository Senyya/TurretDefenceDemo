using UnityEngine;

// this is the class to attach the dissolve shader on object
public class DissolveAppearWithPerlinNoise : MonoBehaviour
{
    public Shader shader;
    public float dissolveTime = 0.75f;
    private float timmer;
    private Renderer meshRenderer;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    private void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.material.shader = shader;
        meshRenderer.material.SetColor("_DissolveColorA", new Color(12.0f/255,27.0f/255,132.0f/255,1.0f));
        meshRenderer.material.SetTexture("_DissolveMap",GenerateTexture());
        meshRenderer.material.SetFloat("_DissolveThreshold", 1.0f);
    }
    private void Update()
    {
        timmer += Time.deltaTime;
        if (timmer < dissolveTime)
        {
            float num = 1.0f - timmer / dissolveTime;
            meshRenderer.material.SetFloat("_DissolveThreshold", num);
        }
        else
        {
            meshRenderer.material.SetFloat("_DissolveThreshold", 0f);
        }
    }
    public void changeColor(Color newColor)
    {
        meshRenderer.material.SetColor("_Diffuse", newColor);
    }
    // GenerateTexture() is used to create a perlin noise
    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        // now generate the perlin noise map for the texutre

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }
    Color CalculateColor(int x, int y)
    {
        float xCoordinate = (float)x / width * scale;
        float yCoordinate = (float)y / height * scale;
        float sample = Mathf.PerlinNoise(xCoordinate, yCoordinate);
        return new Color(sample, sample, sample);
    }
}
