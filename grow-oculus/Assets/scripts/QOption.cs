using TMPro;
using UnityEngine;

public class QOption : MonoBehaviour
{
    public Questionaire q;
    public int value;

    public TextMeshPro optionLabel;
    public SpriteRenderer optionImage;

    public MeshRenderer signal;
    public Material activeMaterial;
    public Material defaultMaterial;

    public void SetQuestionaire(Questionaire q)
    {
        this.q = q;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("selectable") && !q.answerLock)
    //    {
    //        q.answerLock = true;
    //        Destroy(other.GetComponent<OVRGrabbable>());
    //        q.SetSelection(value, gameObject, other.gameObject);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("selectable") && !q.answerLock)
        {
            if (!other.GetComponent<OVRGrabbable>().isGrabbed)
            {
                q.answerLock = true;
                q.SetSelection(value, gameObject, other.gameObject);
            }
        }
    }

    public void SetActiveMaterial()
    {
        signal.sharedMaterial = activeMaterial;
    }

    public void SetDefaultMaterial()
    {
        signal.sharedMaterial = defaultMaterial;
    }
}
