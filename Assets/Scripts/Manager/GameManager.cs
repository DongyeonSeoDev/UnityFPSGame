using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] private Text timeText = null;
    [SerializeField] private Text gameOverTimeText = null;
    [SerializeField] private Text gunStateText = null;
    [SerializeField] private string[] gunStateTexts;
    private float time = 0f;

    private StringBuilder sb = new StringBuilder(8);

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager의 instance가 없습니다.");
            }

            return instance;
        }
    }

    public bool isPlay;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameManager의 instance가 이미 존재하므로 " + gameObject.name + "을 지웠습니다.");
            Destroy(gameObject);
            return;
        }

        if (timeText == null)
        {
            Debug.LogError("timeText에 Text가 없습니다.");
        }

        if (gameOverTimeText == null)
        {
            Debug.LogError("gameOverTimeText에 Text가 없습니다.");
        }

        if (gunStateText == null)
        {
            Debug.LogError("gunStateText에 Text가 없습니다.");
        }

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

    public void clickReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void clickEnd()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void GameOver()
    {
        isPlay = false;

        gameOverTimeText.text = "Time: " +TimeDisplay();
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
