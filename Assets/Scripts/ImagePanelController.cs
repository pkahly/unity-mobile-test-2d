using UnityEngine;
using UnityEngine.UI;
using System;

public class ImagePanelController : MonoBehaviour
{
    private ImagePanel[] imagePanels;
    private int gridSize;

    void Start()
    {
        // Get the Image Panels which will display the image
        imagePanels = GetComponentsInChildren<ImagePanel>();

        // Get the side-length of the grid
        gridSize = (int)Math.Sqrt(imagePanels.Length);

        // Load the image
        Texture2D texture = Resources.Load<Texture2D>("Images/sample");
        if (texture == null)
        {
            throw new ArgumentException("Failed to load image");
        }

        // Get Image sizes
        int imageWidth = texture.width;
        int imageHeight = texture.height;

        // Use Ceil to handle cases where the image is not evenly divisible by the grid size
        int dividedWidth = Mathf.CeilToInt((float)imageWidth / gridSize);
        int dividedHeight = Mathf.CeilToInt((float)imageHeight / gridSize);

        // Create textures for the image panels
        Texture2D[] dividedTextures = new Texture2D[imagePanels.Length];
        for (int i = 0; i < dividedTextures.Length; i++)
        {
            dividedTextures[i] = new Texture2D(dividedWidth, dividedHeight);
        }

        // Divide the pixels into sections
        for (int x = 0; x < imageWidth; x++)
        {
            for (int y = 0; y < imageHeight; y++)
            {
                // Calculate which image panel to use
                int panelX = (x / dividedWidth);
                int panelY = (y / dividedHeight);
                int panelNum = panelX + (panelY * gridSize);

                // Calculate coordinates within the image panel
                int divX = x % dividedWidth;
                int divY = y % dividedHeight;

                // Set the pixel in the appropriate image panel texture
                Color pixel = texture.GetPixel(x, y);
                dividedTextures[panelNum].SetPixel(divX, divY, pixel);
            }
        }

        // Send a portion of the image data to each panel
        for (int i = 0; i < imagePanels.Length; i++)
        {
            dividedTextures[i].Apply();

            imagePanels[i].SetImage(dividedTextures[i]);
        }
    }

    void Update()
    {

    }
}
