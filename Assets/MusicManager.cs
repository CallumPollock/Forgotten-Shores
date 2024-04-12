using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> dayMusic = new List<AudioClip>();
    [SerializeField] List<AudioClip> nightMusic = new List<AudioClip>();
    AudioSource musicSource;

    public static Action<AudioClip> onPlayMusicTrack;

    private void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 0.5f;
        WorldTime.OnDayBegin += IsDayOrNight;
        
    }

    private void IsDayOrNight(bool isDay)
    {
        StopAllCoroutines();
        PlayNewRandomTrack(isDay);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    void PlayNewRandomTrack(bool isDay)
    {

        AudioClip clip;
        if (isDay)
            clip = dayMusic[UnityEngine.Random.Range(0, dayMusic.Count)];
        else
            clip = nightMusic[UnityEngine.Random.Range(0, nightMusic.Count)];

        musicSource.clip = clip;
        StartCoroutine(TrackTimer(clip.length, isDay));
        musicSource.Play();
        onPlayMusicTrack?.Invoke(clip);
    }
    
    IEnumerator TrackTimer(float clipLength, bool isDay)
    {
        yield return new WaitForSeconds(clipLength);
        PlayNewRandomTrack(isDay);
    }

}
