using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gm;

    public GameObject spawnObject;
    public Collider spawnZone;

    public void Start()
    {
        spawnZone = GetComponent<Collider>();
        Instantiate(spawnObject, transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<OVRGrabbable>() != null && !gm.qm.questionaireMode)
        {
            Instantiate(spawnObject, transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
