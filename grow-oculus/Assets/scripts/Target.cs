using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject hit;
    public GameObject center;
    public bool onTarget = false;
    private Animator hitAnimator;

    private Vector3 defaultPos = new Vector3(0f, -1f, 0f);

    public void Start()
    {
        hitAnimator = hit.transform.Find("Snow-decal").GetComponent<Animator>();
    }

    public void Update()
    {
        //to check every frame if target is visible uncomment next line
        //onTarget = gameObject.GetComponent<Renderer>().isVisible;

        //if (hitAnimator.GetCurrentAnimatorStateInfo(0).IsName("finished"))
        //{
        //    hit.transform.position = new Vector3(0f, -1f, 0f);
        //    hitAnimator.Play("idle", 0, 0f);
        //}
    }

    //places the hit indicator onto the hit position and plays its animation. 
    public void RegisterHit(Vector3 pos)
    {
        hit.transform.position = pos;
        hitAnimator.Play("hit-fadeaway", 0, 0f);
        StartCoroutine(JanDelay(1.1f));
        Debug.Log("Distance to target center:\n" + (center.transform.position - pos).magnitude);
    }

    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        hit.transform.position = defaultPos;
        hitAnimator.Play("idle", 0, 0f);

    }
}
