using System.Collections;
using System.Collections.Generic;
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

    public List<GameObject> spawnedProjectiles;
    public bool spawnForQuestionnaire = false;

    public void Start()
    {
        spawnedProjectiles = new List<GameObject>();
    }

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
        else
        {
            spawnedProjectiles.Add(projectile);
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
        else
        {
            spawnedProjectiles.Add(projectile);
        }
    }

    //for manually adding Projectiles to the spawners
    public void ForceSpawnProjectile(Transform parent)
    {
        projectile = Instantiate(spawnObject, parent.transform);
        projectile.transform.localPosition = new Vector3(0f, 2f, 0.5f);
        projectile.transform.localRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
        projectile.GetComponent<Projectile>().gm = gm;
        projectile.GetComponent<Projectile>().projectileLogger = projectileLogger;
        projectile.GetComponent<Projectile>().grabbable = projectile.GetComponent<OVRGrabbable>();
        projectile.GetComponent<Projectile>().rb = projectile.GetComponent<Rigidbody>();
        projectile.GetComponent<Rigidbody>().useGravity = true;
        projectile.GetComponent<Projectile>().isArmed = true;

        if (!spawnForQuestionnaire)
        {
            projectile.GetComponent<Animator>().Play("ComeCloser");
            spawnedProjectiles.Add(projectile);
        }
    }

    //hide all Projectiles, this spawner has produced. it should be only one Projectile.
    public void HideProjectiles()
    {
        if (!spawnForQuestionnaire)
        {
            foreach (GameObject p in spawnedProjectiles)
            {
                Debug.Log("hiding dart");
                p.GetComponent<Projectile>().HideProjectile();
            }
        }
        else
        {
            foreach (Transform p in transform)
            {
                if (p.gameObject.GetComponent<Projectile>() != null)
                {
                    p.gameObject.GetComponent<Projectile>().HideProjectile();
                }
            }
        }
    }

    //hide all Projectiles, this spawner has produced. it should be only one Projectile.
    public void ClearProjectiles()
    {
        if (!spawnForQuestionnaire)
        {
            foreach (GameObject p in spawnedProjectiles)
            {
                p.GetComponent<Projectile>().DestroyProjectile();
            }
            spawnedProjectiles.Clear();
        }
        else
        {
            foreach (Transform p in transform)
            {
                if (p.gameObject.GetComponent<Projectile>() != null)
                {
                    p.gameObject.GetComponent<Projectile>().DestroyProjectile();
                }
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
