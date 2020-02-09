using UnityEngine;

public class AdJustParticleSystem : MonoBehaviour
{
    private static GameObject instance;
    static float startTime = 0f;

    private static float size = 0f;
    public static float Size
    {
        get
        {
            return size > 0 ? size : 0f;
        }
    }

    static float lifeSpan = 7f;

    public static bool isBlackHoleOpen { get; private set; }

    private static bool isCollapsing = false;

    // Start is called before the first frame update
    void Start()
    {
        isBlackHoleOpen = false;
        instance = gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (size < 0f)
        {
            Collapse();
            return;
        }
        if (Time.time - startTime > lifeSpan)
        {
            isCollapsing = true;
        }
        if (isCollapsing)
        {
            size -= Time.fixedDeltaTime;
            transform.localScale = new Vector3(2, 2, 2);
        }
        if (!(size > 1f || isCollapsing))
        {
            size += Time.fixedDeltaTime / 1f * GameManager.Scale;
            float transformScale = 2f + GameManager.Scale * 3f;
            transform.localScale = new Vector3(transformScale, transformScale, transformScale);
        }
        float scale = 1f + size * 6;
        var main = GetComponent<ParticleSystem>().main;
        main.startSizeX = scale;
        main.startSizeY = scale;
        main.startSizeZ = scale;
    }
    public static void Respawn()
    {
        startTime = Time.time;
        size = 0f;
        isCollapsing = false;
        isBlackHoleOpen = true;
    }

    public static void Collapse()
    {

        isBlackHoleOpen = false;
        instance.transform.parent.parent.position = new Vector3(0, -20f, 0);
    }
}
