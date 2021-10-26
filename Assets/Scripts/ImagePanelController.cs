using UnityEngine;
using UnityEngine.UI;
using System;

public class ImagePanelController : MonoBehaviour
{
    private ImagePanel[] imagePanels;

    void Start()
    {
        imagePanels = GetComponentsInChildren<ImagePanel>();

        Texture2D texture = Resources.Load<Texture2D>("Images/sample");
        if (texture == null)
        {
            throw new ArgumentException("Failed to load image");
        }

        foreach (ImagePanel panel in imagePanels)
        {
            panel.SetImage(texture);
        }
    }

    void Update()
    {

    }
}
