using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMovement : MonoBehaviour
{
    public float speed = 5;
    void Update()
    {
        transform.position += transform.up * speed;
    }
}
