using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChanged;

    [SerializeField]
    private float dayLength, startTime;

    private TimeSpan currentTime;
    private float minuteLength => dayLength / 1440;
    private Light2D globalLight;

    [SerializeField] private Gradient gradient;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        currentTime = TimeSpan.FromHours(startTime);
    }

    private void Start()
    {
        
        StartCoroutine(AddMinute());
    }
    private IEnumerator AddMinute()
    {
        currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChanged?.Invoke(this, currentTime);
        globalLight.color = gradient.Evaluate(PercentOfDay(currentTime));
        yield return new WaitForSeconds(minuteLength);
        StartCoroutine(AddMinute());
    }

    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float)timeSpan.TotalMinutes % 1440 / 1440;
    }

    public TimeSpan GetCurrentTime() { return currentTime; }
}
