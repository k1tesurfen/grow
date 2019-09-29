using UnityEngine;
public static class Aim
{
    public static Vector3 pos = Vector3.zero;
    public static Vector3 dir = Vector3.forward;

    //shoots ray from pos in direction dir. returns the position of the hit or the zero vector.
    public static Vector3 Hit()
    {
        Ray ray = new Ray(pos, dir);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}