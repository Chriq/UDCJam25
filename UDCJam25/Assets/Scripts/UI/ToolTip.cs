using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] string tt_name;
    [SerializeField] string tt_description;
    [SerializeField] bool active;

    private void Awake()
    {
        active = true;
    }
    public void Disable()
    {
        active = false;
    }

    void OnMouseEnter()
    {
        if (active)
            ToolTipManager._instance.ToolTip(tt_name, tt_description);
    }
    void OnMouseExit()
    {
        ToolTipManager._instance.Hide();
    }
}
