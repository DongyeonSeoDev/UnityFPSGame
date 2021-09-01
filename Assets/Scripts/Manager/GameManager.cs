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
    [SerializeField] private Text gameOverStageText = null;
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

    private PlayerMove playerMove = null;

    public GameObject enemys = null;
    public GameObject traps = null;

    private bool isSpeedMode = false;

    public float limitTime = 30f;
    private float startTime = 0f;

    public Text bulletUI = null;
    public Image bulletImage = null;
    public Text attackText = null;
    public Text defText = null;

    public CanvasGroup pause = null;
    public Button continueButton = null;
    public Button retryButton = null;
    public Button keyButton = null;
    public Button exitButton = null;

    public GameObject keyPanel = null;
    public Button keyPanelExitButton = null;

    public bool isPause = false;

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
    public Text mazeStateText;
    public Image mazeState;
    public string[] mazeStateTexts;

    public GameObject damageTextObject = null;
    public Transform damageTexts = null;
    public GameObject itemText = null;
    public Transform itemTexts = null;

    private int enemyKillCount = 0;
    private int enemyCount = 0;

    private ClearCheckManager clearCheckManager = null;

    public Text mazeModeText = null;
    public Text mazeModeValue = null;

    public int EnemyKillCount
    {
        get { return enemyKillCount; }
        set
        {
            if (gameStateManager.mazeMode == eMazeMode.ALLKILLENEMY)
            {
                enemyKillCount = value;
                ShowMazeEnemy(enemyCount - enemyKillCount);

                if (enemyKillCount >= enemyCount)
                {
                    clearCheckManager.Clear();
                }
            }
        }
    }

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

        if (gameOverStageText == null)
        {
            Debug.LogError("gameOverStageText가 없습니다.");
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

        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove가 없습니다.");
        }

        if (enemys == null)
        {
            Debug.LogError("enemys가 없습니다.");
        }

        if (traps == null)
        {
            Debug.LogError("traps가 없습니다.");
        }

        clearCheckManager = FindObjectOfType<ClearCheckManager>();

        if (clearCheckManager == null)
        {
            Debug.LogError("clearCheckManager가 없습니다.");
        }

        if (bulletUI == null)
        {
            Debug.LogError("bulletUI가 없습니다.");
        }

        if (bulletImage == null)
        {
            Debug.LogError("bulletImage가 없습니다.");
        }

        if (attackText == null)
        {
            Debug.LogError("attackText가 없습니다.");
        }

        if (defText == null)
        {
            Debug.LogError("defText가 없습니다.");
        }

        if (itemText == null)
        {
            Debug.LogError("itemtext가 없습니다.");
        }

        if (itemTexts == null)
        {
            Debug.LogError("itemTexts가 없습니다.");
        }

        if (mazeState == null)
        {
            Debug.LogError("mazeState가 없습니다.");
        }

        if (continueButton == null)
        {
            Debug.LogError("continueButton이 없습니다.");
        }
        else
        {
            continueButton.onClick.AddListener(() =>
            {
                Pause();
            });
        }

        if (retryButton == null)
        {
            Debug.LogError("retryButton이 없습니다.");
        }
        else
        {
            retryButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;

                gameStateManager.DataClear();
                gameStateManager.Save();

                PoolManager.pool.Clear();
                PoolManager.prefabDictionary.Clear();

                DOTween.KillAll();
                SceneManager.LoadScene("Maze");
            });
        }

        if (exitButton == null)
        {
            Debug.LogError("exitButton이 없습니다.");
        }
        else
        {
            exitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }

        if (keyButton == null)
        {
            Debug.LogError("keyButton이 없습니다.");
        }
        else
        {
            keyButton.onClick.AddListener(() =>
            {
                keyPanel.SetActive(true);
            });
        }

        if (keyPanelExitButton == null)
        {
            Debug.LogError("keyPanelExitButton이 없습니다.");
        }
        else
        {
            keyPanelExitButton.onClick.AddListener(() =>
            {
                keyPanel.SetActive(false);
            });
        }

        PoolManager.CreatePool<DamageText>(damageTextObject, damageTexts, 20);
        PoolManager.CreatePool<ItemText>(itemText, itemTexts, 5);

        gameOverButton[0].onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            PoolManager.pool.Clear();
            PoolManager.prefabDictionary.Clear();

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
        sb.Append(gameStateManager.Stage);
        sb.Append(" 스테이지");

        stageText.text = sb.ToString();
        time = gameStateManager.time;
        timeText.text = TimeDisplay();

        if (gameStateManager.mazeMode == eMazeMode.SPEED)
        {
            traps.SetActive(false);
            enemys.SetActive(false);

            isSpeedMode = true;
            startTime = time;
        }

        enemyCount = enemys.transform.childCount;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ShowMazeState();

        if (gameStateManager.mazeMode == eMazeMode.SPEED)
        {
            mazeModeText.text = "남은 시간";
        }
        else if (gameStateManager.mazeMode == eMazeMode.ALLKILLENEMY)
        {
            mazeModeText.text = "남은 적";
            ShowMazeEnemy(enemyCount - enemyKillCount);
        }
        else
        {
            mazeModeText.text = "탈출하세요!";
            sb.Remove(0, sb.Length);
            sb.Append("최고기록: ");
            sb.Append(gameStateManager.highScoreStage);
            sb.Append("스테이지");
            mazeModeValue.text = sb.ToString();
        }
    }

    private void Update()
    {
        if (!isPlay) return;

        time += Time.deltaTime;

        timeText.text = TimeDisplay();

        if (isSpeedMode)
        {
            ShowMazeTime();

            if (startTime + limitTime <= time)
            {
                playerMove.GameOver();
            }
        }
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
        gameOverTimeText.text = "시간 : " + TimeDisplay();
        gameOverStageText.text = "스테이지 : " + gameStateManager.Stage;

        gameStateManager.DataClear();
        gameStateManager.Save();

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

    public string TimeDisplay(float time = 0)
    {
        time = time == 0 ? this.time : time;

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

    public void BulletCountUI(int bulletCount)
    {
        sb.Remove(0, sb.Length);
        sb.Append("총알 : ");
        sb.Append(bulletCount);
        sb.Append("/30");

        bulletUI.text = sb.ToString();

        bulletImage.fillAmount = (float)bulletCount / 30;
    }

    public void AttackTextUI(int attack)
    {
        sb.Remove(0, sb.Length);
        sb.Append("공격력 : ");
        sb.Append(attack);

        attackText.text = sb.ToString();
    }

    public void DefTextUI(int def)
    {
        sb.Remove(0, sb.Length);
        sb.Append("방어력 : ");
        sb.Append(def);

        defText.text = sb.ToString();
    }

    private void ShowMazeState()
    {
        mazeStateText.text = mazeStateTexts[(int)gameStateManager.mazeMode];

        if (gameStateManager.mazeSize == eMazeSize.LARGE)
        {
            mazeStateText.text += " (대형 미로)";
        }

        mazeStateText.DOColor(new Color(1f, 1f, 1f, 0f), 2f).SetDelay(1f);
        mazeState.DOColor(new Color(1f, 1f, 1f, 0f), 2f).SetDelay(1f).OnComplete(() => {
            mazeState.gameObject.SetActive(false);
            mazeStateText.gameObject.SetActive(false);
        });
    }

    private void ShowMazeTime()
    {
        mazeModeValue.text = TimeDisplay(startTime + limitTime - time);
    }

    private void ShowMazeEnemy(int value)
    {
        mazeModeValue.text = value.ToString();
    }

    public void Pause()
    {
        if (pause.alpha == 1 && isPause == true)
        {
            Time.timeScale = 1f;

            pause.DOFade(0f, 0.5f).OnComplete(() =>
            {
                pause.interactable = false;
                pause.blocksRaycasts = false;

                isPause = false;
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            });
        }
        else if (pause.alpha == 0 && isPause == false)
        {
            pause.DOFade(1f, 0.5f).OnComplete(() =>
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                pause.interactable = true;
                pause.blocksRaycasts = true;

                isPause = true;

                Time.timeScale = 0f;
            });
        }
    }
}
