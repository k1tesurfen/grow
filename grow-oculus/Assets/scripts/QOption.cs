using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QOption : MonoBehaviour
{
    public Questionaire q;
    public int value;

    public TextMeshPro optionLabel;
    public SpriteRenderer optionImage;

    public void SetQuestionaire(Questionaire q)
    {
        this.q = q;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("selectable"))
        {
            q.SetSelection(value);
        }
    }
}
