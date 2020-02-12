using System.Collections;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public QuestionaireManager qm;

    public Vector3 positionLeft = new Vector3(-0.88f, -1.28f, -0.19f);
    public Vector3 rotationLeft = new Vector3(0f, 30f, 0f);

    public Vector3 positionRight = new Vector3(0.88f, -1.28f, 0.19f);
    public Vector3 rotationRight = new Vector3(0f, 120f, 0f);

    private bool bulpOn = false;
    public bool isSwitchable = false;

    public Material defaultMaterial;
    public Material highlightedMaterial;

    public MeshRenderer lightBulp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("handCollider"))
        {
            SwitchLightBulp();
            if (isSwitchable)
            {
                qm.RedoRecentQuestion();
            }
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
            transform.parent.localPosition = positionLeft;
        }
        else
        {
            transform.parent.localPosition = positionRight;
        }
    }

    public void SetRotation(bool rightHanded)
    {
        if (rightHanded)
        {
            transform.parent.localRotation = Quaternion.Euler(rotationLeft);
        }
        else
        {
            transform.parent.localRotation = Quaternion.Euler(rotationRight);
        }
    }
}
