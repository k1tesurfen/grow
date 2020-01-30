using System.Collections;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public QuestionaireManager qm;
    private bool bulpOn = false;

    public Material defaultMaterial;
    public Material highlightedMaterial;

    public MeshRenderer lightBulp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("handCollider"))
        {
            SwitchLightBulp();
            qm.RedoRecentQuestion();
            StartCoroutine(JanDelay(1f));
        }
    }

    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        SwitchLightBulp();
    }

    public void SwitchLightBulp()
    {
        if (bulpOn)
        {
            Material[] mats = lightBulp.materials;
            mats[1] = defaultMaterial;
            lightBulp.materials = mats;
            bulpOn = false;
        }
        else
        {
            Material[] mats = lightBulp.materials;
            mats[1] = highlightedMaterial;
            lightBulp.materials = mats;
            bulpOn = true;
        }
    }
}
