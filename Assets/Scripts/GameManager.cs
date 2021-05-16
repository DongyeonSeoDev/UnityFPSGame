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
                Debug.LogError("GameManager�� instance�� �����ϴ�.");
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
            Debug.LogWarning("GameManager�� instance�� �̹� �����ϹǷ� " + gameObject.name + "�� �������ϴ�.");
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
