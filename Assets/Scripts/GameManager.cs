using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
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

    public Text scoreText;

    private int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
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

        instance = this;
        Score = 0;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void clickReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void clickEnd()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
