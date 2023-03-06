using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorController : MonoBehaviour
{
    //GameObject UI_Highlight;
    GameObject UI_Interactable;
    GameObject UI_Invalid;
    Interactable interact_component;

    private void Awake()
    {
        //UI_Highlight = this.gameObject.transform.GetChild(0).gameObject;
        UI_Interactable = this.gameObject.transform.GetChild(1).gameObject;
        UI_Invalid = this.gameObject.transform.GetChild(2).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Interactable>(out interact_component))
        {
            UI_Interactable.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UI_Interactable.SetActive(false);
        interact_component = null;
    }

    void Update()
    {
        // Move Selector
        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));

        // Click Interaction
        if (Input.GetMouseButtonDown(0))
        {
            // UI Click
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            
            // Scene Interactable
            if (interact_component != null)
            {
                interact_component.Interact();
            }
        }
    }
}
