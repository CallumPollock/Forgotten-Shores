using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> musicTracks = new List<AudioClip>();
    AudioSource musicSource;

    public static Action<AudioClip> onPlayMusicTrack;

    private void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 0.5f;
        PlayNewRandomTrack();
        
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    void PlayNewRandomTrack()
    {
        AudioClip clip = musicTracks[UnityEngine.Random.Range(0, musicTracks.Count)];
        musicSource.clip = clip;
        StartCoroutine(TrackTimer(clip.length));
        musicSource.Play();
        onPlayMusicTrack?.Invoke(clip);
    }
    
    IEnumerator TrackTimer(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        PlayNewRandomTrack();
    }

}
