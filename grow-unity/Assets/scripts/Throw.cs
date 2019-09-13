using UnityEngine;

public class Throw : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;

    [SerializeField] public Material highlightMaterial;
    [SerializeField] public Material defaultMaterial;

    public float throwforce = 600;
    private Vector3 objectPos;
    private float distance;

    public float distanceThreshold = 2f;
    public bool canHold = true;
    public bool isHolding = false;
    private bool flying = false;

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, gm.hand.transform.position);
        if (distance >= distanceThreshold)
        {
            isHolding = false;
        }
        else
        {
            if (isHolding)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.parent = gm.hand.transform;
                transform.localPosition = gm.hand.transform.localPosition;

                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        GetComponent<Rigidbody>().AddForce((hit.point - transform.position).normalized * throwforce);
                    }
                    else
                    {
                        GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwforce);
                    }
                    flying = true;
                    isHolding = false;
                }
            }
            else
            {
                objectPos = transform.position;
                transform.SetParent(null);
                GetComponent<Rigidbody>().useGravity = true;
                transform.position = objectPos;
            }
        }
    }

    private void OnMouseOver()
    {
        //checks whether left mousebutton was pressed and picks item up
        if (Input.GetMouseButtonUp(1))
        {
            if (distance <= distanceThreshold)
            {
                isHolding = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().detectCollisions = true;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (distance <= distanceThreshold)
        {
            GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (distance <= distanceThreshold)
        {
            transform.GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "scatter")
        {
            if (flying)
            {
                gm.scatter.Explode(transform.position);
                Destroy(transform.gameObject);
            }
        }
    }
}
