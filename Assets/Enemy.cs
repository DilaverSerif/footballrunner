using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Collider baseCollider;
    [SerializeField] private float speed;
    private void Awake()
    {
        baseCollider = GetComponent<Collider>();
        baseCollider.isTrigger = true;
    }
    
    private void Update()
    {
        transform.position += new Vector3(0, 0, -speed * Time.fixedDeltaTime);
    }

}
