using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] string tt_name;
    [SerializeField] string tt_description;

    void OnMouseEnter()
    {
        ToolTipManager._instance.ToolTip(tt_name, tt_description);
    }
    void OnMouseExit()
    {
        ToolTipManager._instance.Hide();
    }
}
