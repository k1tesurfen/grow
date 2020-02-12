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

    public LaserStages stage = LaserStages.idle;

    public ParticleSystem ray;

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

    public void HideVisuals()
    {
        lineRenderer.enabled = false;
        ray.Stop();
        ray.transform.position = new Vector3(0, -100, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.currentInteractionMethod == InteractionMethod.magical && isLaserHand && !gm.qm.questionnaireMode)
        {
            if (activateLaser)
            {
                activateLaser = false;
                lineRenderer.enabled = true;
                ray.transform.position = transform.position + (0.1f * transform.forward);
                ray.transform.rotation = transform.rotation;
                ray.Play();
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
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin) && !gm.qm.questionnaireMode)
        {
            //if we grab and the black hole is not set yet
            if (!AdJustParticleSystem.isBlackHoleOpen &&
                stage == LaserStages.setBlackHole)
            {
                lineRenderer.enabled = false;
                ray.transform.position = new Vector3(0, -100, 0);
                ray.Stop();
                SpawnBlackHole();

                //spawn dart in front of player
                gm.pm.spawners[0].ForceSpawnProjectile(gm.mainHand.transform.parent);

                stage = LaserStages.enhanceAndThrow;
            }
        }
    }

    public void SetLaserStage(LaserStages stage)
    {
        this.stage = stage;
    }

    public void SpawnBlackHole()
    {
        Debug.Log("Setting Blackhole");
        gm.blackHole.transform.position = endPos;
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
    idle = 0,
    setBlackHole = 1,
    enhanceAndThrow = 2,
}

