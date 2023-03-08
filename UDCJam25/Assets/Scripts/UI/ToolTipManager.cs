using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager _instance;

    TextMeshProUGUI text_name;
    TextMeshProUGUI text_desc;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
        text_name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text_desc = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ToolTip(string name, string description)
    {
        Debug.Log("TT - ToolTip");
        this.gameObject.SetActive(true);
        this.transform.position = Input.mousePosition;
        text_name.text = name;
        text_desc.text = description;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
