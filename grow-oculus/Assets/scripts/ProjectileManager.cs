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
                Destroy(child.gameObject);
            }
        }
    }

}
