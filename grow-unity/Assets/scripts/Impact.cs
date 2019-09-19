using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "scatter")
        {
            Vector3 impactPosition = transform.position;
            
        }
    }
}
