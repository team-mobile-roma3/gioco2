using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleCanvas : MonoBehaviour
{
    void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector3 screenSize = Camera.main.ViewportToWorldPoint(Vector3.up + Vector3.right);

        screenSize *= 02;

        float sizeY = screenSize.y / 30;
        float sizeX = screenSize.x / 30;

        rt.localScale = new Vector3(sizeX, sizeY, 1);
    }
}