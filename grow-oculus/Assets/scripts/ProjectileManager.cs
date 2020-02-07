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
                child.gameObject.GetComponent<Snowball>().DestroySnowball();
            }
        }
    }

    public void HideProjectiles()
    {
        foreach(Spawner sp in spawners)
        {
            foreach(Transform child in sp.transform)
            {
                child.gameObject.GetComponent<Snowball>().HideSnowball();
            }
        }
    }

    public void Repopulate()
    {
        foreach(Spawner sp in spawners)
        {
            sp.ForceSpawnSnowball();
        }
    }

}
