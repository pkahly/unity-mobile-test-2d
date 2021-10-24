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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseX = mousePos.x;
        float mouseY = mousePos.y;

        transform.position = new Vector2(Mathf.Clamp(mouseX, minX, maxX), Mathf.Clamp(mouseY, minY, maxY));
    }
}
