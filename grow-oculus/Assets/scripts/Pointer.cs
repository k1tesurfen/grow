using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameManager gm;
    public LineRenderer lineRenderer;

    public float defaultLength = 1f;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    protected OVRInput.Controller m_controller;

    public float m_prevFlex;

    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;

    public bool lasering;
    public bool isLaserHand;
    public static bool activateLaser = false;

    public LaserStages stage = LaserStages.setBlackHole;

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


    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.currentInteractionMethod == InteractionMethod.magical && isLaserHand)
        {
            if (activateLaser)
            {
                activateLaser = false;
                lineRenderer.enabled = true;
            }
            UpdateLength();
            float prevFlex = m_prevFlex;
            // Update values from inputs
            m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller);

            CheckForGrabOrRelease(prevFlex);
        }
    }

    void CheckForGrabOrRelease(float prevFlex)
    {
        //Grab Begin
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin))
        {
            //if we grab and the black hole is not set yet
            if (!AdJustParticleSystem.isBlackHoleOpen &&
                stage == LaserStages.setBlackHole)
            {
                lineRenderer.enabled = false;
                if (Vector3.Distance(endPos, Vector3.zero) > 4f)
                {
                    SpawnBlackHole();
                }
                stage = LaserStages.enhanceAndThrow;
            }
        }
        ////Grab End
        //else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd))
        //{
        //    if (!Physics.CheckSphere(transform.position, 0.14f, LayerMask.GetMask("Projectile")))
        //    {
        //        lineRenderer.enabled = true;
        //    }
        //}
    }

    public void SetLaserStage(LaserStages stage)
    {
        this.stage = stage;
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
        lineRenderer.SetPosition(0, transform.position + (0.1f * transform.forward));

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

        Ray ray = new Ray(transform.position + (0.1f * transform.forward), transform.forward);

        Physics.Raycast(ray, out hit, lm);

        return hit;
    }

    Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}

public enum LaserStages
{
    setBlackHole = 0,
    enhanceAndThrow= 1,
}

