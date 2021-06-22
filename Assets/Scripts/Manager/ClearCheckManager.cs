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
            Debug.LogError("exitButton�� �����ϴ�.");
        }

        if (clearCanvasGroup == null)
        {
            Debug.LogError("clearCanvasGroup�� �����ϴ�.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText�� Text�� �����ϴ�.");
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
