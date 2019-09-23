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

    public float distanceThreshold = 4f;
    public bool canHold = true;
    public bool isHolding = false;
    public bool flying = false;

    // Update is called once per frame
    void Update()
    {
        //calculate distance by square magnitude to avoid costly square operation
        distance = (transform.position - gm.hand.transform.position).sqrMagnitude;

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

                //if right mouse button pressed throw the object
                if (Input.GetMouseButtonUp(1))
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
                    GetComponent<Attractor>().doAttract = true;
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
        //checks whether right mousebutton was pressed and picks item up
        if (Input.GetMouseButtonDown(1))
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
            //activate hover effect on snowball
            GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (distance <= distanceThreshold)
        {
            //deactivate hover effect on snowball
            transform.GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "scatter")
        {
            if(other.gameObject.name == "Target"){
                other.GetComponent<Target>().hit.transform.position = transform.position;
            }
            if (flying)
            {
                gm.scatter.Explode(transform.position);
                Destroy(gameObject);
            }
        }
    }
}
