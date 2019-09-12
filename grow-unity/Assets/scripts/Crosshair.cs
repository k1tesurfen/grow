using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshair;
    public Rigidbody player;

    public float restingSize;
    public float maxSize;
    public float speed;
    private float currentSize;

    // Start is called before the first frame update
    void Start()
    {
        crosshair = GetComponent<RectTransform>(); 
    }

    private void Update()
    {
        if (isMoving || player.transform.position.y > 1.0f)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }
        crosshair.sizeDelta = new Vector2(currentSize, currentSize);
    }

    bool isMoving
    {
        get
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
