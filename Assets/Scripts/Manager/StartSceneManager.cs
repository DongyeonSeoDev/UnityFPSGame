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

    private void Start()
    {
        if (image == null)
        {
            Debug.Log("spriteRenderer가 없습니다.");
        }

        image.DOColor(new Color(0, 0, 0, 0), 2f).OnComplete(() =>
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(() =>
                {
                    image.DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene("MazeTest");
                    });
                });
            }
        });
    }
}
