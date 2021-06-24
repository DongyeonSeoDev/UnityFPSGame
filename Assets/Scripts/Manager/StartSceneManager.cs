using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;

    private void Awake()
    {
        if (startButton == null)
        {
            Debug.LogError("startButton이 없습니다.");
        }

        if (audioSource == null)
        {
            Debug.LogError("audioSource가 없습니다.");
        }

        if (sound == null)
        {
            Debug.LogError("sound가 없습니다.");
        }
    }

    private void Start()
    {
        if (image == null)
        {
            Debug.Log("spriteRenderer가 없습니다.");
        }

        Invoke("soundPlay", 0.5f);

        image.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() =>
        {
            if (startButton != null)
            {
                startButton.gameObject.SetActive(true);
                startButton.image.DOColor(new Color(0.8f, 0.8f, 0.8f, 1f), 1f);

                startButton.onClick.AddListener(() =>
                {
                    audioSource.clip = sound;
                    audioSource.Play();
                    image.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene("MazeTest");
                    });
                });
            }
        });
    }

    private void soundPlay()
    {
        audioSource.Play();
    }
}