using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Camera Controller
 * 
 * Inputs Managed:
 *      Mouse ScrollWheel
 *      Camera Lock
 *      Camera Up
 *      Camera Down
 *      Camera Right
 *      Camera Left
 */

public class CameraController : MonoBehaviour
{
    GameObject target;
    Camera cam;

    bool locked = true;
    Vector3 offset = Vector3.back;

    [SerializeField] float min_zoom = 1;
    [SerializeField] float max_zoom = 10;
    [SerializeField] float camSpeed = 10f;
    [SerializeField] float zoomSpeed = 50f;
    [SerializeField] float lerpSpeed = 3f;
    [SerializeField] float borderWidth = 20f;

    private void Awake()
    {
        cam = this.GetComponent<Camera>();
        target = GameManager.Instance.selectedCharacter;
    }

    void Update()
    {
		target = GameManager.Instance.selectedCharacter;

		// Scroll
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, min_zoom, max_zoom);
        }

        // Toggle Lock
        if (Input.GetButtonDown("Camera Lock"))
        {
            locked = !locked;
        }

        // Locked - LERP to selected character
        if (locked)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, lerpSpeed * Time.deltaTime);
        }

        // Unlocked - Free Camera through key or mouse press
        else
        {
            if (Input.mousePosition.y > Screen.height - borderWidth)
            {
                transform.Translate(0, camSpeed * Time.deltaTime, 0);
            }
            if (Input.mousePosition.y < borderWidth)
            {
                transform.Translate(0, -camSpeed * Time.deltaTime, 0);
            }
            if (Input.mousePosition.x > Screen.width - borderWidth)
            {
                transform.Translate(camSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.mousePosition.x < borderWidth)
            {
                transform.Translate(-camSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetButton("Camera Up"))
            {
                transform.Translate(0, camSpeed * Time.deltaTime, 0);
            }
            if (Input.GetButton("Camera Down"))
            {
                transform.Translate(0, -camSpeed * Time.deltaTime, 0);
            }
            if (Input.GetButton("Camera Right"))
            {
                transform.Translate(camSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetButton("Camera Left"))
            {
                transform.Translate(-camSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
