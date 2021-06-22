using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ClearCheckManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup clearCanvasGroup;
    [SerializeField] private Text timeText;

    /// <summary>
    /// 0: ExitBtn
    /// </summary>
    public Button[] clearButton;

    private void Awake()
    {
        if (clearButton[0] == null)
        {
            Debug.LogError("exitButton이 없습니다.");
        }

        if (clearCanvasGroup == null)
        {
            Debug.LogError("clearCanvasGroup이 없습니다.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText에 Text가 없습니다.");
        }

        clearButton[0].onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            GameManager.Instance.isPlay = false;
            timeText.text = "Time: " + GameManager.Instance.TimeDisplay();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            clearCanvasGroup.blocksRaycasts = true;
            clearCanvasGroup.interactable = true;

            if (clearCanvasGroup != null)
            {
                clearCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
                {
                    Time.timeScale = 0f;
                });
            }
        }
    }
}
