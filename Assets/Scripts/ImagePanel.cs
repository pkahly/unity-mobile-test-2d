using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ImagePanel : MonoBehaviour
{
    public RawImage image;
    public Button button;

    void Awake()
    {
        // Load RawImage
        image = gameObject.GetComponent<RawImage>();
        if (image == null)
        {
            throw new ArgumentException("Failed to find RawImage component");
        }

        // Load Button
        button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            throw new ArgumentException("Failed to find Button component");
        }
    }
}
