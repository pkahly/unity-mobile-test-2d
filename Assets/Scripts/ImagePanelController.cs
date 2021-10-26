using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePanelController : MonoBehaviour
{
    private ImagePanel[] imagePanels;

    void Start()
    {
        imagePanels = GetComponents<ImagePanel>();
    }

    void Update()
    {

    }
}
