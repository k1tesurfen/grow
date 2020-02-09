using UnityEngine;

public class IntensityAdjustment : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Light>().intensity = 0.46f + GameManager.Scale * 6;
    }
}
