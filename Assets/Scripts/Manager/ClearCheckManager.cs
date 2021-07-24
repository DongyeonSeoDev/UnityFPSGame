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
    [SerializeField] private Text stageText;

    /// <summary>
    /// 0: ExitBtn
    /// 1: NextBtn
    /// </summary>
    public Button[] clearButton;

    public AudioSource sound;
    public AudioClip clip;

    private PlayerMove playerMove = null;
    private GameStateManager gameStateManager = null;

    public int largeMazeProbability = 50;
    public int speedModeProbability = 20;
    public int allKillEnemyModeProbability = 30;

    private bool isAllEnemyKillMode = false;

    private bool isGameClear = false;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove�� �����ϴ�.");
        }

        gameStateManager = GameStateManager.Instance;

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager�� �����ϴ�.");
        }

        if (clearButton[0] == null)
        {
            Debug.LogError("exitButton�� �����ϴ�.");
        }

        if (clearButton[1] == null)
        {
            Debug.LogError("clearButton�� �����ϴ�.");
        }

        if (clearCanvasGroup == null)
        {
            Debug.LogError("clearCanvasGroup�� �����ϴ�.");
        }

        if (timeText == null)
        {
            Debug.LogError("timeText�� Text�� �����ϴ�.");
        }

        if (stageText == null)
        {
            Debug.LogError("stageText�� Text�� �����ϴ�.");
        }

        if (sound == null)
        {
            Debug.LogError("sound�� �����ϴ�.");
        }

        if (clip == null)
        {
            Debug.LogError("clip�� �����ϴ�.");
        }

        clearButton[0].onClick.AddListener(() =>
        {
            Application.Quit();
        });

        clearButton[1].onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            PoolManager.pool.Clear();
            PoolManager.prefabDictionary.Clear();

            DOTween.KillAll();

            if (gameStateManager.mazeSize == eMazeSize.LARGE) 
            {
                SceneManager.LoadScene("Maze2");
            }
            else
            {
                SceneManager.LoadScene("Maze");
            }
        });

        if (GameStateManager.Instance.mazeMode == eMazeMode.ALLKILLENEMY)
        {
            isAllEnemyKillMode = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAllEnemyKillMode)
        {
            if (other.CompareTag("PLAYER"))
            {
                Clear();
            }
        }
    }

    public void Clear()
    {
        if (isGameClear) return;

        isGameClear = true;

        GameManager.Instance.isPlay = false;
        timeText.text = "Time: " + GameManager.Instance.TimeDisplay();
        stageText.text = "Score: " + gameStateManager.Stage;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        gameStateManager.playerHP = playerMove.Hp;
        gameStateManager.playerDef = playerMove.def;
        gameStateManager.bulletCount = playerMove.currentBulletCount;
        gameStateManager.playerDamage = playerMove.damage;
        gameStateManager.autoGun = playerMove.autoGun;
        gameStateManager.Stage++;
        gameStateManager.time = GameManager.Instance.time;

        int randomNum = Random.Range(0, 100);

        if (randomNum < speedModeProbability)
        {
            gameStateManager.mazeMode = gameStateManager.mazeMode != eMazeMode.SPEED ? eMazeMode.SPEED : eMazeMode.NORMAL;
        }
        else if (randomNum < allKillEnemyModeProbability + speedModeProbability)
        {
            gameStateManager.mazeMode = gameStateManager.mazeMode != eMazeMode.ALLKILLENEMY ? eMazeMode.ALLKILLENEMY : eMazeMode.NORMAL;
        }
        else
        {
            gameStateManager.mazeMode = eMazeMode.NORMAL;
        }

        randomNum = Random.Range(0, 100);

        if (randomNum < largeMazeProbability)
        {
            gameStateManager.mazeSize = GameStateManager.Instance.mazeSize != eMazeSize.LARGE ? eMazeSize.LARGE : eMazeSize.NORMAL;
        }
        else
        {
            gameStateManager.mazeSize = eMazeSize.NORMAL;
        }

        gameStateManager.Save();

        clearCanvasGroup.blocksRaycasts = true;
        clearCanvasGroup.interactable = true;

        sound.clip = clip;
        sound.loop = false;
        sound.Play();

        if (clearCanvasGroup != null)
        {
            clearCanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                Time.timeScale = 0f;
            });
        }
    }
}
