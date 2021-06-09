using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearSceneManager : MonoBehaviour
{
    public Button exitButton;
    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
