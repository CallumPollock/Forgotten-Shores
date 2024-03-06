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
        
        StartCoroutine(DestroyOnDelay());
    }

    public void SetColour(Color color)
    {
        text.color = color;
    }

    public void SetText(string text)
    {
        this.text.text = text;
        StartCoroutine(DestroyOnDelay());
    }

    IEnumerator DestroyOnDelay()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
