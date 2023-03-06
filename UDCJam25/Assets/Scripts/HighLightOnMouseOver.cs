using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* https://answers.unity.com/questions/232180/best-way-to-highlight-an-object-on-mouse-over.html */
public class HighLightOnMouseOver : MonoBehaviour
{
    [SerializeField]
    Color baseColor;
    [SerializeField]
    Color highlightColor;

    void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        //baseColor = GetComponent<SpriteRenderer>().material.color;
        //GetComponent<SpriteRenderer>().material.color = highlightColor;
        GetComponent<SpriteRenderer>().color = highlightColor;
    }
    void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        //GetComponent<SpriteRenderer>().material.color = baseColor;
        GetComponent<SpriteRenderer>().color = baseColor;
    }
}
