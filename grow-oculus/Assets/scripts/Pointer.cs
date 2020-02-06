using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameObject pointIndicator;
    public LineRenderer lineRenderer = null;

    public float defaultLength = 5f;

    [HideInInspector]
    public Vector3 endPos; 

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            //Create Black Hole 
            UpdateLength();
        }
    }

    void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        
        lineRenderer.SetPosition(1, CalculateEnd());
    }

    Vector3 CalculateEnd()
    {
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = DefaultEnd(defaultLength);

        //Manage Raycast Hit
        //TODO: Set BlackHole Indicator to position and rotate it toward camera
        //if accept-button is pressed, 
        if (hit.collider)
        {
            endPosition = hit.point;
        }
        return endPosition;
    }

    RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, LayerMask.GetMask("Default"));

        return hit;
    }

    Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
