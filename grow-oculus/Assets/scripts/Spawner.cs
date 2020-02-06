using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gm;

    public GameObject spawnObject;
    public Collider spawnZone;

    [HideInInspector]
    public GameObject snowball;
    public Logger snowballLogger;

    [Space(20)]
    public bool spawnForQuestionnaire = false;

    public void Start()
    {
        spawnZone = GetComponent<Collider>();
        if (spawnForQuestionnaire)
        {
            SpawnSnowball();
        }
    }

    //Gets triggered when a snowball is taken from the spawning zone.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<OVRGrabbable>() != null)
        {
            other.GetComponent<Rigidbody>().useGravity = true;
            other.GetComponent<Snowball>().armed = true;
            SpawnSnowball();
        }
    }


    //if there is no snowball in the spawning area spawn a snowball.
    //if spawning area is crowded retry after 1s
    public void SpawnSnowball()
    {
        if (!Physics.CheckSphere(transform.position, 0.08f, LayerMask.GetMask("snowball")))
        {
            Debug.Log("spawning snowball");

            snowball = Instantiate(spawnObject, transform);
            snowball.GetComponent<Snowball>().gm = gm;
            snowball.GetComponent<Snowball>().snowballLogger = snowballLogger;
            snowball.GetComponent<Snowball>().grabbable = snowball.GetComponent<OVRGrabbable>();

            if (spawnForQuestionnaire)
            {
                snowball.GetComponent<Snowball>().spawnForQuestionnaire = true;
                snowball.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                snowball.tag = "selectable";
            }
        }
        else
        {
            StartCoroutine(JanDelay(1f));
        }
    }

    //for manually adding snowballs to the spawners
    public void ForceSpawnSnowball()
    {
        if (!Physics.CheckSphere(transform.position, 0.08f, LayerMask.GetMask("snowball")))
        {
            snowball = Instantiate(spawnObject, transform);
            snowball.GetComponent<Snowball>().gm = gm;
            snowball.GetComponent<Snowball>().snowballLogger = snowballLogger;
            snowball.GetComponent<Snowball>().grabbable = snowball.GetComponent<OVRGrabbable>();
        }
    }

    //destroy all snowballs, this spawner has produced. it should be only one snowball.
    //this is the countermove to ForceSpawnObjects()
    public void ClearSnowballs()
    {
        foreach (Transform sb in transform)
        {
            if (sb.gameObject.GetComponent<Snowball>() != null)
            {
                Destroy(sb.gameObject);
            }
        }
    }

    //Coroutine to execute SpawnSnowball after a set amount of time.
    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnSnowball();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
