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
        if (IsMoving || player.transform.position.y > 1.0f)
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }
        crosshair.sizeDelta = new Vector2(currentSize, currentSize);
    }

    private bool IsMoving => System.Math.Abs(Input.GetAxis("Horizontal")) > 0f || System.Math.Abs(Input.GetAxis("Vertical")) > 0f;

    //returns the angle from viewing direction to a given object
    public float AngleTowardsObject(GameObject obj)
    {
        return Vector3.Angle(Camera.main.transform.forward, obj.transform.position - Camera.main.transform.position);
    }
}
