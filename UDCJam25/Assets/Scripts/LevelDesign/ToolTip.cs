using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] string tt_name;
    [SerializeField] string tt_description;

    void TT_Show()
    {
        ToolTipManager._instance.ToolTip(tt_name, tt_description);
    }
    void OnMouseEnter()
    {
        Debug.Log("TT - Mouse Enter");
        Invoke("TT_Show", 2); 
    }
    void OnMouseExit()
    {
        CancelInvoke("TT_Show");
        ToolTipManager._instance.Hide();
    }
}
