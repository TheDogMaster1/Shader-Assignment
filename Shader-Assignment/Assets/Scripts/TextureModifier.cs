using System;
using UnityEngine;

public enum ModificationType
{
    Normal, Invert, Blur, Rotate
};


public class TextureModifier : MonoBehaviour
{
    public Color test;
    public Texture2D inputTexture;
    [Tooltip("When true, the texture is modified every time an inspector value is changed")]
    public bool autoRecompute = true;
    public ModificationType patternType = 0;
    [Tooltip("When true, alpha values lower than 1 are shown by blending in the alphaColor")]
    public bool showAlpha = true;
    public Color alphaColor;

    // colors from inputTexture: 
    Color[] inputCols;
    // output texture and its colors:
    Texture2D texture = null;
    Color[] cols = null;

    [Range(0, 360)]
    public int degrees;

    void Start()
    {
        CreateTexture();
        Draw();
    }

    void CreateTexture()
    {
        Debug.Log("(Re)creating texture");
        // Read colors from this array:
        inputCols = inputTexture.GetPixels();

        // Create a texture and pass it to the material of this game object's renderer:
        Renderer rend = GetComponent<Renderer>();
        texture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGBA32, false);
        rend.material.mainTexture = texture;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point; // No interpolation
                                               // Write colors to this array, then call
                                               //  texture.SetPixels(cols) and texture.Apply() to see them:
        cols = texture.GetPixels();
    }

    /// <summary>
    /// Draws a pattern given by the [pattern] type to the [cols] array, which
    /// should have size [width] * [height].
    /// </summary>
    void DrawPattern(Color[] outputColors, Color[] inputColors, int width, int height, ModificationType pattern)
    {
        for (int index = 0; index < width * height; index++)
        {
            // start with transparent black:
            Color output = new Color(0, 0, 0, 0);

            int x = index % width;
            int y = index / height;

            float u = (float)x / (width);
            float v = (float)y / (height);

            float x1 = (u * Mathf.Cos(degrees / Mathf.Rad2Deg) - v * Mathf.Sin(degrees / Mathf.Rad2Deg));
            float y1 = (u * Mathf.Sin(degrees / Mathf.Rad2Deg) + v * Mathf.Cos(degrees / Mathf.Rad2Deg));
            x1 = x1 + x / 2;
            y1 = y1 + y / 2;
            // TODO:
            // Insert the code for your own patterns here:
            //  write to output, read from inputColors.
            //  (See the slides for details)
            switch (pattern)
            {
                case ModificationType.Normal:
                    output = inputColors[index];
                    break;
                case ModificationType.Invert:
                    Color input = inputColors[index];
                    output = new Color(1 - input.r, 1 - input.g, 1 - input.b, 1);
                    break;
                case ModificationType.Blur:
                    // TODO: To see the blur effect, calculate the x and y coordinates
                    //  and pass them to this function:	

                    output = Blur(inputColors, x, y, width, height);
                    break;
                case ModificationType.Rotate:
                    Color newCol = inputTexture.GetPixelBilinear(x1, y1);
                    output = new Color(newCol.r, newCol.g, newCol.b, 1);
                    break;
            }

            if (showAlpha)
            {
                outputColors[index] =
                    output * output.a +
                    alphaColor * (1 - output.a);
            }
            else
            {
                outputColors[index] = output;
            }
        }
    }

    void Draw()
    {
        if (texture.width != inputTexture.width || texture.height != inputTexture.height)
        {
            CreateTexture();
        }
        DrawPattern(cols, inputCols, texture.width, texture.height, patternType);

        texture.SetPixels(cols);
        texture.Apply();
    }

    // OnValidate is called whenever an inspector value is changed - even in edit mode!
    void OnValidate()
    {
        // To prevent calling Draw code in edit mode,
        // we check whether a texture has been created (in Start)
        if (texture == null || !autoRecompute) return;
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

    #region convolution
    // Gaussian blur convolution matrix:
    static int[,] blur =
    {
        {1,2,1},
        {2,4,2},
        {1,2,1}
    };
    // What does this matrix do? Try it below! Can you make the effect work "in both directions"?
    static int[,] basicEdgeDetect =
    {
        {0,0,0},
        {1,-1,0},
        {0,0,0}
    };
    static Color ApplyConvolution(Color[] inputColors, int x, int y, int width, int height, int[,] convolution, float numerator, bool includeAlpha = false)
    {
        Color output = new Color(0, 0, 0, 0);
        // Assumes the convolution matrix is a 3x3 matrix:
        for (int dx = 0; dx < 3; dx++)
        {
            for (int dy = 0; dy < 3; dy++)
            {
                int inX = x + dx - 1; // From x-1 to x+1
                int inY = y + dy - 1; // From y-1 to y+1
                if (inX >= 0 && inX < width && inY >= 0 && inY < height)
                {
                    output += inputColors[inX + inY * width] * convolution[dx, dy];
                } // out of range is implicitly considered to be black
            }
        }
        output /= numerator;
        if (!includeAlpha) output.a = 1;
        return output;
    }

    static Color Blur(Color[] inputColors, int x, int y, int width, int height)
    {
        return ApplyConvolution(inputColors, x, y, width, height, blur, 16, true);
    }
    #endregion
}
