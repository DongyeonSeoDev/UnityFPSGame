using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearCheckManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject clearCanvas;
    [SerializeField] private Text timeText;

    private void Awake()
    {
        if (exitButton == null)
        {
            Debug.LogError("exitButton�� Button�� �����ϴ�.");
        }
        else
        {
            exitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

        if (clearCanvas == null)
        {
            Debug.LogError("clearCanvas�� Canvas�� �����ϴ�.");
        }
        else if (clearCanvas.activeSelf)
        {
            Debug.LogError("clearCanvas�� active�� �����־�� �մϴ�.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText�� Text�� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            clearCanvas.SetActive(true);
            Time.timeScale = 0f;
            GameManager.Instance.isPlay = false;
            timeText.text = "Time: " + GameManager.Instance.TimeDisplay();
        }
    }
}
