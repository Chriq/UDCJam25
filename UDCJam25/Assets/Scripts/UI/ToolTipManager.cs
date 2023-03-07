using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipManager : MonoBehaviour, IPointerClickHandler
{
    public static ToolTipManager _instance;

    TextMeshProUGUI text_name;
    TextMeshProUGUI text_desc;

    // Start is called before the first frame update
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

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ToolTip(string name, string description)
    {
        gameObject.SetActive(true);
        text_name.text = name;
        text_desc.text = description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
