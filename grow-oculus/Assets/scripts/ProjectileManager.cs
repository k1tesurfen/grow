using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Spawner[] spawners;

    public void ClearProjectiles()
    {
        foreach(Spawner sp in spawners)
        {
            foreach(Transform child in sp.transform)
            {
                child.gameObject.GetComponent<Projectile>().DestroyProjectile();
            }
        }
    }

    public void HideProjectiles()
    {
        foreach(Spawner sp in spawners)
        {
            foreach(Transform child in sp.transform)
            {
                child.gameObject.GetComponent<Projectile>().HideProjectile();
            }
        }
    }

    public void Repopulate()
    {
        foreach(Spawner sp in spawners)
        {
            Debug.Log("====================Force Spawning new Projectiles");
            sp.ForceSpawnProjectile();
        }
    }

}
