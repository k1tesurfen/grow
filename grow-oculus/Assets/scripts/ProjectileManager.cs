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
            sp.ClearProjectiles();
        }
    }

    public void HideProjectiles()
    {
        foreach(Spawner sp in spawners)
        {
            sp.HideProjectiles();
        }
    }

    public void Repopulate()
    {
        foreach(Spawner sp in spawners)
        {
            sp.ForceSpawnProjectile();
        }
    }

}
