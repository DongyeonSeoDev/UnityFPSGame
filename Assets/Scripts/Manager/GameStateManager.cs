using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int playerHP = 100;
    public int playerDef = 0;
    public float playerDamage = 10f;
    public int stage = 1;
    public float time = 0f;
    public bool autoGun = false;

    private int originalPlayerHP;
    public float originalPlayerDamage;

    private static GameStateManager instance = null;

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameStateManage의 instance가 없습니다.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameStateManager의 instance가 이미 존재하므로 " + gameObject.name + "을 지웠습니다.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        GameStateManager[] obj = FindObjectsOfType<GameStateManager>(); 
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);

        originalPlayerHP = playerHP;
        originalPlayerDamage = playerDamage;
    }

    public void Clear()
    {
        playerHP = originalPlayerHP;
        playerDef = 0;
        playerDamage = originalPlayerDamage;
        autoGun = false;
        stage = 1;
        time = 0f;
    }
}
