using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap2 : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private MakeMaze makeMaze = null;

    private AudioSource audioSource;

    private void Awake()
    {
        try
        {
            if (particle == null)
            {
                throw new Exception("particle이 없습니다.");
            }

            makeMaze = FindObjectOfType<MakeMaze>();

            if (makeMaze == null)
            {
                throw new Exception("makeMaze가 없습니다.");
            }

            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                throw new Exception("audioSource가 없습니다.");
            }

            int randomNum = UnityEngine.Random.Range(0, makeMaze.enablePosition.Count);
            transform.position = makeMaze.enablePosition[randomNum];
            makeMaze.enablePosition.RemoveAt(randomNum);
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            particle.Play();
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            particle.Stop();
            audioSource.Stop();
        }
    }
}
