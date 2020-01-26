using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gm;

    public GameObject spawnObject;
    public Collider spawnZone;

    public GameObject snowball;

    [Space(20)]
    public bool spawnForQuestionnaire = false;

    public void Start()
    {
        spawnZone = GetComponent<Collider>();
        SpawnSnowball();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<OVRGrabbable>() != null)
        {
            other.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("Checking if there is an empty sphere");
            SpawnSnowball();
        }
    }


    public void SpawnSnowball()
    {
        if (!Physics.CheckSphere(transform.position, 0.062f, LayerMask.GetMask("snowball")))
        {

            Debug.Log("spawning snowball");
            snowball = Instantiate(spawnObject, transform);
            snowball.GetComponent<Snowball>().gm = gm;
            if (spawnForQuestionnaire)
            {
                snowball.tag = "selectable";
            }

        }
        else
        {
            StartCoroutine(ExecuteAfterTime(1f));
        }
    }

    //Coroutine to execute SpawnSnowball after a set amount of time.
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnSnowball();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
