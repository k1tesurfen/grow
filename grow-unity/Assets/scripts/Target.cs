﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject hit;
    public bool onTarget = false;

    public void Update()
    {
        onTarget = gameObject.GetComponent<Renderer>().isVisible;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hit.transform.position = collision.GetContact(0).point;
    }
}
