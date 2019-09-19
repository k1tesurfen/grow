using UnityEngine;
public class Aim{
    public static Vector3 pos = Vector3.zero;
    public static Vector3 dir = Vector3.forward;

    //shoots ray from pos in direction dir. returns the position of the hit or the zero vector.
    public static Vector3 Hit(){
        Ray ray = new Ray(pos, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            return hit.point;
        }
        else return Vector3.zero;
    }
}