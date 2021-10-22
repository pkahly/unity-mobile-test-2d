using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour {
    bool isDraggingActive = false;
    Draggable draggingObject;

    void Start() {
        
    }

    void Update() {
        // Release dragged object
        if (isDraggingActive && (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))) {
            Drop();
            return;
        }
        
        // Get Screen Position
        Vector2 screenPos;
        if (Input.GetMouseButton(0)) {
            screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        } else if (Input.touchCount == 1) {
            screenPos = Input.GetTouch(0).position;
        } else {
            return;
        }

        // Convert to world coordinates
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Start or Continue Dragging
        if (isDraggingActive) {
            Drag(worldPos);
        } else {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null) {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if (draggable != null) {
                    draggingObject = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag() {
        isDraggingActive = true;
    }

    void Drag(Vector2 worldPos) {
        draggingObject.transform.position = worldPos;
    }

    void Drop() {
        isDraggingActive = false;
    }
}
