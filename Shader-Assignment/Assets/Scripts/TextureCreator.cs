using UnityEngine;


public class TextureCreator : MonoBehaviour
{
    // Add your own pattern types here:
    public enum PatternType { Noise, NoiseWithGray, None, Mandelbrot, UVS, Stripes, cos, experiment, rainbowfunc, rainbow };

    public PatternType patternType;

    const int SIZE = 1024;

    Texture2D texture = null;
    Color[] cols = null;

    public bool rotate = false;

    [Range(0, 1)]
    public float hue = 0;

    [Header("Rotation settings")]
    [Range(0, 360)]
    public int degrees;
    private float decimalDegrees = 0;
    public bool autoRotate;
    public float rotationSpeed = 1;

    [Header("Noise settings")]

    public Color noiseColor = Color.white;
    public float uMultiplier = 1.0f;
    public float vMultiplier = 1.0f;

    public float colorAdder = 0.5f;
    void Start()
    {
        // Create a texture and pass it to the material of this game object's renderer:
        Renderer rend = GetComponent<Renderer>();
        texture = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
        rend.material.mainTexture = texture;
        texture.wrapMode = TextureWrapMode.Clamp;

        Draw();
    }

    /// <summary>
    /// Returns the pixel color for texture coordinate (u,v), for a given pattern.
    /// </summary>
    Color CalculatePixelColor(float u, float v, PatternType pattern)
    {
        // TODO: insert your own pattern creation code here.
        //  See the slides for details.
        switch (pattern)
        {
            case PatternType.Noise: // white noise				
                return Random.value * Color.white;
            case PatternType.NoiseWithGray:
                float noisefloat = Mathf.PerlinNoise(u * uMultiplier, v * vMultiplier);
                if (noisefloat < 0.5f) noisefloat += colorAdder;
                //else noisefloat -= colorAdder;
                Color noise = noisefloat * noiseColor;
                return noise;
            case PatternType.Mandelbrot:
                return Mandelbrot(3 * (u - 0.75f), 3 * (v - 0.5f));
            case PatternType.UVS:
                return new Color(u, v, 0, 1);
            case PatternType.Stripes:
                return Color.white * (Mathf.Floor((u * 10) % 2)) * v;
            case PatternType.cos:
                return new Color(.5f + .5f * Mathf.Cos(2 * Mathf.PI * u), 0, 0, 1);
            case PatternType.rainbowfunc:
                Color col = Color.black;
                if (Myfunction(u, 0) > v)
                {
                    col += Color.red;
                }
                if (Myfunction(u, Mathf.PI / 3 * 2) > v)
                {
                    col += Color.green;
                }
                if (Myfunction(u, Mathf.PI / 3 * 4) > v)
                {
                    col += Color.blue;
                }
                return col;
            case PatternType.rainbow:
                return new Color(Myfunction(hue, 0), Myfunction(hue, Mathf.PI / 3 * 2), Myfunction(hue, Mathf.PI / 3 * 4), 1);
            case PatternType.experiment:
                return Color.white * ((Mathf.Floor(u * 10) + Mathf.Floor(v * 10)) % 2);
            //return Color.white * (.5f + .5f * Mathf.Cos(2 * Mathf.PI * (u / SIZE - 1) + Mathf.PI)) * (.5f + .5f * Mathf.Cos(2 * Mathf.PI * (v / SIZE - 1) + Mathf.PI));
            default:
                return Color.blue;
        }
    }

    private float Myfunction(float u, float offset)
    {
        float y = 0.5f * Mathf.Cos(2 * Mathf.PI * u + offset) + .5f;
        return y;
    }
    /// <summary>
    /// Draws a pattern given by the [pattern] number to the [cols] array, which
    /// should have size [width] * [height].
    /// </summary>
    void DrawPattern(Color[] cols, int width, int height, PatternType pattern)
    {
        for (int index = 0; index < width * height; index++)
        {
            int x = index % width;
            int y = index / width;

            float u = (float)x / (width - 1);
            float v = (float)y / (height - 1);

            if (autoRotate)
            {
                decimalDegrees += Time.deltaTime * rotationSpeed;
                degrees = Mathf.RoundToInt(decimalDegrees);
                if (degrees > 360)
                {
                    decimalDegrees = 0;
                }
            }
            float x1 = u * Mathf.Cos(degrees / Mathf.Rad2Deg) - v * Mathf.Sin(degrees / Mathf.Rad2Deg);
            float y1 = u * Mathf.Sin(degrees / Mathf.Rad2Deg) + v * Mathf.Cos(degrees / Mathf.Rad2Deg);
            // TODO: calculate UV coordinates and pass them to CalculatePixelColor:
            //cols[index] = CalculatePixelColor(x1, y1, pattern);

            float xUsed = u;
            float yUsed = v;
            if (rotate)
            {
                xUsed = x1 + x / 2;
                yUsed = y1 + y / 2;
            }

            cols[index] = CalculatePixelColor(xUsed, yUsed, pattern);
        }
    }

    void Draw()
    {
        if (cols == null)
        {
            cols = texture.GetPixels();
        }
        DrawPattern(cols, SIZE, SIZE, patternType);

        texture.SetPixels(cols);
        texture.Apply();
    }

    // OnValidate is called whenever an inspector value is changed - even in edit mode!
    void OnValidate()
    {
        // To prevent calling Draw code in edit mode,
        // we check whether a texture has been created (in Start)
        if (texture == null) return;
        Draw();
    }

    private void Update()
    {
        // Control + S saves to file:
        if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            var exporter = GetComponent<TextureExporter>();
            if (exporter != null)
            {
                exporter.ExportTexture(texture);
            }
        }

    }

    #region Mandelbrot
    // Used for the Mandelbrot fractal:
    const int maxIterations = 30;
    const float escapeLengthSquared = 4;

    Color Mandelbrot(float cReal, float cImaginary)
    {
        int iteration = 0;

        float zReal = 0;
        float zImaginary = 0;

        while (zReal * zReal + zImaginary * zImaginary < escapeLengthSquared && iteration < maxIterations)
        {
            // Use Mandelbrot's magic iteration formula: z := z^2 + c 
            // (using complex number multiplication & addition - 
            //   see https://mathbitsnotebook.com/Algebra2/ComplexNumbers/CPArithmeticASM.html)
            float newZr = zReal * zReal - zImaginary * zImaginary + cReal;
            zImaginary = 2 * zReal * zImaginary + cImaginary;
            zReal = newZr;
            iteration++;
        }
        // Return a color value based on the number of iterations that were needed to "escape the circle":
        float grad = 1f * iteration / maxIterations; // between 0 and 1
                                                     // TODO: use a nicer gradient
        return new Color(grad, grad, grad);
    }
    #endregion
}
