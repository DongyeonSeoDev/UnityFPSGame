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

    public float time = 0f;

    private StringBuilder sb = new StringBuilder(8);

    private GameStateManager gameStateManager = null;

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

    public AudioSource sound;
    public AudioClip clip;

    public Text stageText;

    public GameObject damageTextObject = null;
    public Transform damageTexts = null;

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

        if (gameOverCanvasGroup == null)
        {
            Debug.LogError("gameOverCanvasGroup이 없습니다.");
        }

        if (gameOverButton[0] == null)
        {
            Debug.LogError("ReSetButton이 없습니다.");
        }

        if (gameOverButton[1] == null)
        {
            Debug.LogError("EndButton이 없습니다.");
        }

        if (sound == null)
        {
            Debug.LogError("sound가 없습니다.");
        }

        if (clip == null)
        {
            Debug.LogError("clip이 없습니다.");
        }

        if (stageText == null)
        {
            Debug.LogError("stageText가 없습니다.");
        }

        if (damageTextObject == null)
        {
            Debug.LogError("damageTextObject가 없습니다.");
        }

        if (damageTexts == null)
        {
            Debug.LogError("damageTexts가 없습니다.");
        }

        gameStateManager = FindObjectOfType<GameStateManager>();

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager가 없습니다.");
        }

        PoolManager.CreatePool<DamageText>(damageTextObject, damageTexts, 20);

        gameOverButton[0].onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            PoolManager.pool.Clear();
            PoolManager.prefabDictionary.Clear();

            gameStateManager.Clear();

            DOTween.KillAll();
            SceneManager.LoadScene("Maze");
        });

        gameOverButton[1].onClick.AddListener(() =>
        {
            Application.Quit();
        });

        instance = this;
        isPlay = true;

        sb.Remove(0, sb.Length);
        sb.Append(gameStateManager.stage);
        sb.Append(" 스테이지");

        stageText.text = sb.ToString();
        time = gameStateManager.time;

        timeText.text = TimeDisplay();

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

        sound.clip = clip;
        sound.loop = false;
        sound.Play();

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
