using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void soundPlay(AudioClip clip, float volume, float playTime)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Invoke("endSound", playTime);
    }

    private void endSound()
    {
        gameObject.SetActive(false);
    }
}
