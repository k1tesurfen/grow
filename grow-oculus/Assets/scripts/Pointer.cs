using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameManager gm;
    public LineRenderer lineRenderer = null;

    public float defaultLength = 50f;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    protected OVRInput.Controller m_controller;

    public float m_prevFlex;

    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;

    public bool lasering;

    public Vector3 blackHoleOffset = new Vector3(0f, 0f, -0.2f);


    [HideInInspector]
    public Vector3 endPos = new Vector3(0f, -20f, 0f);

    public LayerMask lm;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_controller = gameObject.GetComponent<OVRGrabber>().m_controller;
    }

    public static bool activateLaser = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activateLaser)
        {
            activateLaser = false;
            lineRenderer.enabled = true;
        }
        if (gm.currentInteractionMethod == InteractionMethod.magical)
        {
            UpdateLength();
            float prevFlex = m_prevFlex;
            // Update values from inputs
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);

            CheckForGrabOrRelease(prevFlex);
        }
    }

    void CheckForGrabOrRelease(float prevFlex)
    {
        if (m_prevFlex >= grabBegin)
        {
            if (!AdJustParticleSystem.isBlackHoleOpen && !Physics.CheckSphere(transform.position, 0.14f, LayerMask.GetMask("Projectile")))
            {
                lineRenderer.enabled = false;
                if (Vector3.Distance(endPos, Vector3.zero) > 4f)
                {
                    SpawnBlackHole();
                }
            }
        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd))
        {
            if (!Physics.CheckSphere(transform.position, 0.14f, LayerMask.GetMask("Projectile")))
            {
                lineRenderer.enabled = true;
            }
        }
    }

    public void SpawnBlackHole()
    {
        Debug.Log("Setting Blackhole");
        gm.blackHole.transform.position = endPos + blackHoleOffset;
        gm.blackHole.GetComponent<Attractor>().doAttract = true;
        gm.blackHole.GetComponent<Attractor>().StartVisuals();
        AdJustParticleSystem.Respawn();
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

        if (hit.collider)
        {
            endPosition = hit.point;
            endPos = hit.point;
        }
        return endPosition;
    }

    RaycastHit CreateForwardRaycast()
    {
        RaycastHit hit;

        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, lm);

        return hit;
    }

    Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}

