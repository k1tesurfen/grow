using UnityEngine;

public class FortressManager : MonoBehaviour
{
    public Animator anim;

    public bool visible = true;

    private bool initialCall = true;

    private void Start()
    {
        anim.Play("idle",0, 0f); 
    }

    private void Update()
    {
    }

    public void HideFortress()
    {
        if (visible && initialCall)
        {
            anim.SetBool("initialCall", true);
            initialCall = false;
            visible = false;
        }
        if (visible && !initialCall)
        {
            anim.SetFloat("Direction", -1);
            visible = false;
        }
    }

    public void ShowFortress()
    {
        if (!visible)
        {
            anim.SetFloat("Direction", 1);
            visible = true;
        }
    }
}
