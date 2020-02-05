using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject hit;
    public GameObject center;
    public bool onTarget = false;
    private Animator hitAnimator;

    public void Start()
    {
        hitAnimator = hit.transform.Find("Snow-decal").GetComponent<Animator>();
    }

    public void Update()
    {
        //to check every frame if target is visible uncomment next line
        //onTarget = gameObject.GetComponent<Renderer>().isVisible;

        if (hitAnimator.GetCurrentAnimatorStateInfo(0).IsName("finished"))
        {
            hit.transform.position = new Vector3(0f, -1f, 0f);
            hitAnimator.Play("idle", 0, 0f);
        }
    }

    //places the hit indicator onto the hit position and plays its animation. 
    public void RegisterHit(Vector3 pos)
    {
        hitAnimator.Play("hit-fadeaway", 0, 0f);
        hit.transform.position = pos;
        Debug.Log("Distance to target center:\n" + (center.transform.position - pos).magnitude);

    }
}
