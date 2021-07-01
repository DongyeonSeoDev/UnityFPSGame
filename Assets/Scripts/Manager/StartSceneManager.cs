using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button keyButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject keyPanel;
    [SerializeField] private GameObject gun;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;

    private bool isButtonClick = false;

    private void Start()
    {
        if (image == null)
        {
            Debug.Log("spriteRenderer가 없습니다.");
        }

        startButton.onClick.AddListener(() =>
        {
            if (isButtonClick) return;

            audioSource.clip = sound;
            audioSource.Play();
            image.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() =>
            {
                DOTween.KillAll();

                if (GameStateManager.Instance.mazeSize == eMazeSize.NORMAL)
                {
                    SceneManager.LoadScene("Maze");
                }
                else
                {
                    SceneManager.LoadScene("Maze2");
                }
            });

            isButtonClick = true;
        });

        keyButton.onClick.AddListener(() =>
        {
            keyPanel.SetActive(true);
            gun.SetActive(false);
        });

        exitButton.onClick.AddListener(() =>
        {
            keyPanel.SetActive(false);
            gun.SetActive(true);
        });

        Invoke("soundPlay", 0.5f);

        image.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() =>
        {
            startButton.gameObject.SetActive(true);
            keyButton.gameObject.SetActive(true);
            startButton.transform.DOScale(Vector3.one, 1f);
            keyButton.transform.DOScale(Vector3.one, 1f);
        });
    }

    private void soundPlay()
    {
        audioSource.Play();
    }
}