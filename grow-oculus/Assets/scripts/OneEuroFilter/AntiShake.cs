using UnityEngine;

public class AntiShake : MonoBehaviour
{
    OneEuroFilter<Vector3> posFilter;
    OneEuroFilter<Quaternion> rotFilter;

    Vector3 filteredPos;
    Quaternion filteredRot;

    //@TODO: test for 
    [HideInInspector]
    public float filterFrequency = 120f;

    // Start is called before the first frame update
    void Start()
    {
        posFilter = new OneEuroFilter<Vector3>(filterFrequency, 1f, 0.001f, 1f);
        rotFilter = new OneEuroFilter<Quaternion>(filterFrequency, 1f, 0.001f, 1f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            filteredPos = posFilter.Filter(transform.position);
            filteredRot = rotFilter.Filter(transform.rotation);

            transform.position = filteredPos;
            transform.rotation = filteredRot;
        }
    }
}
