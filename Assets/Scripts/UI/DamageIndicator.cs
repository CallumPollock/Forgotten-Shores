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

    public void SetColour(Color color)
    {
        text.color = color;
    }

    public void SetText(string text, Color color, float textSize, float lifespan)
    {
        this.text.text = text;
        this.text.color = color;
        this.text.fontSize = textSize;
        StartCoroutine(DestroyOnDelay(lifespan));
    }

    IEnumerator DestroyOnDelay(float lifespan)
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
