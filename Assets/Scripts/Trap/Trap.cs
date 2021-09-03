using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    private MakeMaze makeMaze = null;

    private readonly int hashIsShow = Animator.StringToHash("IsShow");

    private AudioSource audioSource;

    private void Awake()
    {
        try
        {
            if (animator == null)
            {
                throw new Exception("animator�� �����ϴ�.");
            }

            makeMaze = FindObjectOfType<MakeMaze>();

            if (makeMaze == null)
            {
                throw new Exception("makeMaze�� �����ϴ�.");
            }

            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                throw new Exception("audioSource�� �����ϴ�.");
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
            audioSource.Play();
            animator.SetBool(hashIsShow, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            animator.SetBool(hashIsShow, false);
        }
    }
}
