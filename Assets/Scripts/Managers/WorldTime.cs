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

    public static Action<bool> OnDayBegin;
    bool isDay;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        //currentTime = TimeSpan.FromHours(startTime);
        SaveLoadJSON.worldLoaded += LoadTime;
    }

    public long GetTicks()
    {
        return currentTime.Ticks;
    }

    private void LoadTime(WorldData worldData)
    {
        currentTime = TimeSpan.FromTicks(worldData.ticks);
        StartCoroutine(AddMinute());
        if (currentTime.Hours >= 22 || currentTime.Hours <= 6)
            isDay = false;

        OnDayBegin?.Invoke(isDay);
    }

    private void CheckDayOrNight()
    {
        if (isDay)
        {
            if (currentTime.Hours >= 22)
            {
                isDay = false;
                OnDayBegin?.Invoke(isDay);
            }

        }
        else
        {
            if (currentTime.Hours >= 6 && currentTime.Hours < 22)
            {
                isDay = true;
                OnDayBegin?.Invoke(isDay);
            }

        }
    }

    public void AddTime(float minutes)
    {
        currentTime += TimeSpan.FromMinutes(minutes);
    }

    private IEnumerator AddMinute()
    {
        currentTime += TimeSpan.FromMinutes(1);
        WorldTimeChanged?.Invoke(this, currentTime);
        CheckDayOrNight();
        globalLight.color = gradient.Evaluate(PercentOfDay(currentTime));
        yield return new WaitForSeconds(minuteLength);
        StartCoroutine(AddMinute());
    }

    private float PercentOfDay(TimeSpan timeSpan)
    {
        return (float)timeSpan.TotalMinutes % 1440 / 1440;
    }

    public void PauseTime()
    {
        StopAllCoroutines();
    }

    public void ResumeTime()
    {
        StartCoroutine(AddMinute());
    }

    public TimeSpan GetCurrentTime() { return currentTime; }
}
