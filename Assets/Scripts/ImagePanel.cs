using UnityEngine;
using UnityEngine.UI;
using System;

public class ImagePanel : MonoBehaviour
{
    private RawImage image;

    void Awake()
    {
        image = gameObject.GetComponent<RawImage>();
        if (image == null)
        {
            throw new ArgumentException("Image Panel is broken");
        }
    }

    public void SetImage(Texture2D texture)
    {
        image.texture = texture;
    }
}
