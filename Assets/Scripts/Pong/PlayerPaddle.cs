using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float min;
    public float max;

    void Start()
    {
        // Capture the mouse
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        Debug.Log(mouseY);

        transform.position = new Vector2(transform.position.x, Mathf.Clamp(mouseY, min, max));
    }
}
