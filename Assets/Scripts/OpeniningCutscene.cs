using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class OpeniningCutscene : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private bool fadeIn = false;
    private float alpha = 1f;

    private void Update()
    {
        if(fadeIn)
        {
            alpha = Mathf.Clamp(alpha - (Time.deltaTime * 0.5f), 0, 1);
        }
        else
        {
            alpha = Mathf.Clamp(alpha + (Time.deltaTime * 3f), 0, 1);
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, alpha);
    }

    [YarnCommand("fade_in")]
    public void FadeIn()
    {
        fadeIn = true;
    }

    [YarnCommand("fade_out")]
    public void FadeOut()
    {
        fadeIn = false;
    }
}
