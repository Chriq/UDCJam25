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

    public float camSpeed = 10f;
    public float zoomSpeed = 100f;
    public float lerpSpeed = 3f;
    public float borderWidth = 20f;

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
            cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        }

        // Toggle Lock
        if (Input.GetButtonDown("Camera Lock"))
        {
            locked = !locked;
        }

        // Locked - LERP to main character
        if (locked)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, lerpSpeed * Time.deltaTime);
        }

        // Unlocked - Free Camera through key or mouse press
        if (!locked)
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
            if (Input.GetButtonDown("Camera Up"))
            {
                transform.Translate(0, camSpeed * Time.deltaTime, 0);
            }
            if (Input.GetButtonDown("Camera Down"))
            {
                transform.Translate(0, -camSpeed * Time.deltaTime, 0);
            }
            if (Input.GetButtonDown("Camera Right"))
            {
                transform.Translate(camSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetButtonDown("Camera Left"))
            {
                transform.Translate(-camSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
