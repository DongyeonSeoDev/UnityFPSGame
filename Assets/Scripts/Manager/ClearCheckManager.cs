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
            Debug.LogError("exitButton에 Button이 없습니다.");
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
            Debug.LogError("clearCanvas에 Canvas가 없습니다.");
        }
        else if (clearCanvas.activeSelf)
        {
            Debug.LogError("clearCanvas의 active는 꺼져있어야 합니다.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText에 Text가 없습니다.");
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
