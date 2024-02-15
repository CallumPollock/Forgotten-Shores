using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    [SerializeField] Slider bodySlider, heightSlider;
    [SerializeField] Transform body;

    public void BodySizeSlider()
    {
        body.localScale = new Vector3(bodySlider.value, body.localScale.y);
    }

    public void HeightSlider()
    {
        body.localScale = new Vector3(body.localScale.x, heightSlider.value);
    }
}
