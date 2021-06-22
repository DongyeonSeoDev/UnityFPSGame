using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Text timeText = null;
    [SerializeField] private Text gameOverTimeText = null;
    [SerializeField] private Text gunStateText = null;
    [SerializeField] private string[] gunStateTexts;

    public CanvasGroup gameOverCanvasGroup;
    /// <summary>
    /// 0: RestartBtn
    /// 1: EndBtn
    /// </summary>
    public Button[] gameOverButton;

    private float time = 0f;

    private StringBuilder sb = new StringBuilder(8);

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager�� instance�� �����ϴ�.");
            }

            return instance;
        }
    }

    public bool isPlay;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameManager�� instance�� �̹� �����ϹǷ� " + gameObject.name + "�� �������ϴ�.");
            Destroy(gameObject);
            return;
        }

        if (timeText == null)
        {
            Debug.LogError("timeText�� Text�� �����ϴ�.");
        }

        if (gameOverTimeText == null)
        {
            Debug.LogError("gameOverTimeText�� Text�� �����ϴ�.");
        }

        if (gunStateText == null)
        {
            Debug.LogError("gunStateText�� Text�� �����ϴ�.");
        }

        if (gameOverCanvasGroup == null)
        {
            Debug.LogError("gameOverCanvasGroup�� �����ϴ�.");
        }

        if (gameOverButton[0] == null)
        {
            Debug.LogError("ReSetButton�� �����ϴ�.");
        }

        if (gameOverButton[1] == null)
        {
            Debug.LogError("EndButton�� �����ϴ�.");
        }

        gameOverButton[0].onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            gameOverCanvasGroup.DOKill();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        gameOverButton[1].onClick.AddListener(() =>
        {
            Application.Quit();
        });

        instance = this;
        isPlay = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!isPlay) return;

        time += Time.deltaTime;

        timeText.text = TimeDisplay();
    }

    private string timeCheck(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }

        return time.ToString();
    }

    public void GameOver()
    {
        isPlay = false;
        gameOverTimeText.text = "Time: " + TimeDisplay();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        gameOverCanvasGroup.blocksRaycasts = true;
        gameOverCanvasGroup.interactable = true;

        gameOverCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
        {
            Time.timeScale = 0f;
        });
    }

    public string TimeDisplay()
    {
        int minute = (int)time / 60;
        int second = (int)time - minute * 60;
        int millisecond = (int)((time - (minute * 60 + second)) * 100);

        sb.Remove(0, sb.Length);
        sb.Append(timeCheck(minute));
        sb.Append(':');
        sb.Append(timeCheck(second));
        sb.Append(':');
        sb.Append(timeCheck(millisecond));

        return sb.ToString();
    }

    public void GunModeUIChange(int textNumber)
    {
        gunStateText.text = gunStateTexts[textNumber];
    }
}