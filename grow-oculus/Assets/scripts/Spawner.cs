using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager gm;

    public GameObject spawnObject;
    public SphereCollider spawnZone;
    [HideInInspector]
    public float spawnRadius;

    [HideInInspector]
    public GameObject projectile;
    public Logger projectileLogger;

    public bool spawnForQuestionnaire = false;

    //Gets triggered when a Projectile is taken from the spawning zone.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Projectile>())
        {
            if (!other.gameObject.GetComponent<Projectile>().isArmed)
            {
                other.GetComponent<Rigidbody>().useGravity = true;
                other.GetComponent<Projectile>().isArmed = true;
                SpawnProjectile();
            }
        }
    }


    //if there is no Projectile in the spawning area spawn a Projectile.
    //if spawning area is crowded retry after 1s
    public void SpawnProjectile()
    {
        projectile = Instantiate(spawnObject, transform.position, Quaternion.LookRotation(Vector3.down, Vector3.forward), transform);
        projectile.GetComponent<Projectile>().gm = gm;
        projectile.GetComponent<Projectile>().projectileLogger = projectileLogger;
        projectile.GetComponent<Projectile>().grabbable = projectile.GetComponent<OVRGrabbable>();
        projectile.GetComponent<Projectile>().rb = projectile.GetComponent<Rigidbody>();

        if (spawnForQuestionnaire)
        {
            projectile.GetComponent<Projectile>().spawnForQuestionnaire = true;
            projectile.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            projectile.tag = "selectable";
        }
    }

    //for manually adding Projectiles to the spawners
    public void ForceSpawnProjectile()
    {
        projectile = Instantiate(spawnObject, transform.position, Quaternion.LookRotation(Vector3.down, Vector3.forward), transform);
        projectile.GetComponent<Projectile>().gm = gm;
        projectile.GetComponent<Projectile>().projectileLogger = projectileLogger;
        projectile.GetComponent<Projectile>().grabbable = projectile.GetComponent<OVRGrabbable>();
        projectile.GetComponent<Projectile>().rb = projectile.GetComponent<Rigidbody>();

        if (spawnForQuestionnaire)
        {
            projectile.GetComponent<Projectile>().spawnForQuestionnaire = true;
            projectile.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            projectile.tag = "selectable";
        }
    }

    //hide all Projectiles, this spawner has produced. it should be only one Projectile.
    public void HideProjectiles()
    {
        foreach (Transform sb in transform)
        {
            if (sb.gameObject.GetComponent<Projectile>() != null)
            {
                sb.gameObject.GetComponent<Projectile>().HideProjectile();
            }
        }
    }

    //hide all Projectiles, this spawner has produced. it should be only one Projectile.
    public void ClearProjectiles()
    {
        foreach (Transform sb in transform)
        {
            if (sb.gameObject.GetComponent<Projectile>() != null)
            {
                sb.gameObject.GetComponent<Projectile>().DestroyProjectile();
            }
        }
    }

    //Coroutine to execute SpawnProjectile after a set amount of time.
    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnProjectile();
    }

    private void OnDrawGizmos()
    {
        if (spawnZone != null)
        {
            Gizmos.DrawSphere(transform.position, spawnZone.radius);
        }
    }
}
