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
    /// 1: NextBtn
    /// </summary>
    public Button[] clearButton;

    public AudioSource sound;
    public AudioClip clip;

    private PlayerMove playerMove = null;
    private GameStateManager gameStateManager = null;

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
            clearCanvasGroup.DOKill();

            gameStateManager.playerHP = playerMove.Hp;
            gameStateManager.playerDef = playerMove.def;
            gameStateManager.playerDamage = playerMove.damage;
            gameStateManager.autoGun = playerMove.autoGun;
            gameStateManager.stage++;
            gameStateManager.time = GameManager.Instance.time;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}
