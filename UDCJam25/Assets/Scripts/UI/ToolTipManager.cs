using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipManager : MonoBehaviour, IPointerClickHandler
{
    public static ToolTipManager _instance;

    [SerializeField] Vector2 screen_offset = new Vector2(20, 15);

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
        text_name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text_desc = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ToolTip(Vector2 sceen_pos, string name, string description)
    {
        this.transform.position = sceen_pos + screen_offset;
        text_name.text = name;
        text_desc.text = description;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        this.transform.position = new Vector2(-100, -100);
    }
}
