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
                Debug.LogError("GameManager�� instance�� �����ϴ�.");
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

        if (gameOverStageText == null)
        {
            Debug.LogError("gameOverStageText�� �����ϴ�.");
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

        if (sound == null)
        {
            Debug.LogError("sound�� �����ϴ�.");
        }

        if (clip == null)
        {
            Debug.LogError("clip�� �����ϴ�.");
        }

        if (stageText == null)
        {
            Debug.LogError("stageText�� �����ϴ�.");
        }

        if (damageTextObject == null)
        {
            Debug.LogError("damageTextObject�� �����ϴ�.");
        }

        if (damageTexts == null)
        {
            Debug.LogError("damageTexts�� �����ϴ�.");
        }

        gameStateManager = FindObjectOfType<GameStateManager>();

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager�� �����ϴ�.");
        }

        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove�� �����ϴ�.");
        }

        if (enemys == null)
        {
            Debug.LogError("enemys�� �����ϴ�.");
        }

        if (traps == null)
        {
            Debug.LogError("traps�� �����ϴ�.");
        }

        clearCheckManager = FindObjectOfType<ClearCheckManager>();

        if (clearCheckManager == null)
        {
            Debug.LogError("clearCheckManager�� �����ϴ�.");
        }

        if (bulletUI == null)
        {
            Debug.LogError("bulletUI�� �����ϴ�.");
        }

        if (bulletImage == null)
        {
            Debug.LogError("bulletImage�� �����ϴ�.");
        }

        if (attackText == null)
        {
            Debug.LogError("attackText�� �����ϴ�.");
        }

        if (defText == null)
        {
            Debug.LogError("defText�� �����ϴ�.");
        }

        if (itemText == null)
        {
            Debug.LogError("itemtext�� �����ϴ�.");
        }

        if (itemTexts == null)
        {
            Debug.LogError("itemTexts�� �����ϴ�.");
        }

        if (mazeState == null)
        {
            Debug.LogError("mazeState�� �����ϴ�.");
        }

        if (continueButton == null)
        {
            Debug.LogError("continueButton�� �����ϴ�.");
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
            Debug.LogError("retryButton�� �����ϴ�.");
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
            Debug.LogError("exitButton�� �����ϴ�.");
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
            Debug.LogError("keyButton�� �����ϴ�.");
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
            Debug.LogError("keyPanelExitButton�� �����ϴ�.");
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
        sb.Append(" ��������");

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
            mazeModeText.text = "���� �ð�";
        }
        else if (gameStateManager.mazeMode == eMazeMode.ALLKILLENEMY)
        {
            mazeModeText.text = "���� ��";
            ShowMazeEnemy(enemyCount - enemyKillCount);
        }
        else
        {
            mazeModeText.text = "Ż���ϼ���!";
            sb.Remove(0, sb.Length);
            sb.Append("�ְ���: ");
            sb.Append(gameStateManager.highScoreStage);
            sb.Append("��������");
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
        gameOverTimeText.text = "�ð� : " + TimeDisplay();
        gameOverStageText.text = "�������� : " + gameStateManager.Stage;

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
        sb.Append("�Ѿ� : ");
        sb.Append(bulletCount);
        sb.Append("/30");

        bulletUI.text = sb.ToString();

        bulletImage.fillAmount = (float)bulletCount / 30;
    }

    public void AttackTextUI(int attack)
    {
        sb.Remove(0, sb.Length);
        sb.Append("���ݷ� : ");
        sb.Append(attack);

        attackText.text = sb.ToString();
    }

    public void DefTextUI(int def)
    {
        sb.Remove(0, sb.Length);
        sb.Append("���� : ");
        sb.Append(def);

        defText.text = sb.ToString();
    }

    private void ShowMazeState()
    {
        mazeStateText.text = mazeStateTexts[(int)gameStateManager.mazeMode];

        if (gameStateManager.mazeSize == eMazeSize.LARGE)
        {
            mazeStateText.text += " (���� �̷�)";
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
