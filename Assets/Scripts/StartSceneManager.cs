using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MazeTest");
            });
        }
    }
}
