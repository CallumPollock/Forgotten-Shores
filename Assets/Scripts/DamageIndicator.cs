using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicator : MonoBehaviour
{

    TextMeshPro text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void SetDamageValue(int damage)
    {
        text.enabled = true;
        text.text = damage.ToString();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        text.enabled = false;
    }
}
