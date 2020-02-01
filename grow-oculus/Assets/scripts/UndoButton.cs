using System.Collections;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public QuestionaireManager qm;

    public Vector3 positionLeft = new Vector3 (-0.977f, -1.25f, 0.035f);
    public Vector3 rotationLeft = new Vector3(0f, 45f, 0f);

    public Vector3 positionRight = new Vector3 (0.977f, -1.25f, 0.035f);
    public Vector3 rotationRight =  new Vector3(0f, -45f, 0f);

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

    public void SetPosition(bool rightHanded)
    {
        if (rightHanded)
        {
            transform.parent.position = positionLeft;
        }
        else
        {
            transform.parent.position = positionRight;
        }
    }

    public void SetRotation(bool rightHanded)
    {
        if (rightHanded)
        {
            transform.parent.rotation = Quaternion.Euler(rotationLeft);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(rotationRight);
        }
    }
}
