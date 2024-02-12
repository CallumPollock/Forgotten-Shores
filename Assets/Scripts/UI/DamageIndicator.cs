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
        if (damage < 0)
            text.color = Color.red;
        else if (damage > 0)
            text.color = Color.green;

        text.text = damage.ToString();
        StartCoroutine(DestroyOnDelay());
    }

    IEnumerator DestroyOnDelay()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}