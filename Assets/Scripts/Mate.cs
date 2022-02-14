using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mate : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.position += new Vector3(0, 0, speed * Time.fixedDeltaTime);
    }
}
