using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    public bool isDragging;

    private Collider2D objCollider;
    private DragManager dragManager;

    void Start() {
        objCollider = GetComponent<Collider2D>();
        dragManager = FindObjectOfType<DragManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Draggable otherDraggable = other.GetComponent<Draggable>();

        if (otherDraggable != null && dragManager.draggingObject.gameObject == gameObject) {
            ColliderDistance2D colliderDistance2D = other.Distance(objCollider);
            Vector3 diff = new Vector3(colliderDistance2D.normal.x, colliderDistance2D.normal.y) * colliderDistance2D.distance;

            transform.position -= diff;
        }  
    }
}
