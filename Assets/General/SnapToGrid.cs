using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    void Update()
    {
        if (Application.isPlaying) return;
        Vector2 pos = transform.position;

        // Find the bottom-left corner of the tile that the object is on
        float tileCornerX = Mathf.Floor(pos.x);
        float tileCornerY = Mathf.Floor(pos.y);

        // Add the desired offset to snap to
        float snappedX = tileCornerX + 0.5f;
        float snappedY = tileCornerY + 0.5f;

        transform.position = new Vector3(snappedX, snappedY, transform.position.z);
    }
}
