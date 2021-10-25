using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        // Capture the mouse
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get Screen Position
        Vector2 screenPos;
        if (Input.touchCount == 1)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else
        {
            screenPos = Input.mousePosition;
        }

        // Convert to world coordinates
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        transform.position = new Vector2(transform.position.x, Mathf.Clamp(worldPos.y, minY, maxY));
    }
}
