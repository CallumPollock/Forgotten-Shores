using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudio : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.2f;
    }

    public void CharacterTyped()
    {
        audioSource.pitch = Random.Range(0.2f, 3f);
        audioSource.PlayOneShot(audioClip);
    }
}
