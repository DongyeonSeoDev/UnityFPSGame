using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int playerHP = 100;
    public int playerDef = 0;
    public float playerDamage = 10f;
    public int stage = 1;

    private static GameStateManager instance = null;

    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameStateManage�� instance�� �����ϴ�.");
            }

            return instance;
        }
    }

    //�� ������
    //�� ü��

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("GameStateManager�� instance�� �̹� �����ϹǷ� " + gameObject.name + "�� �������ϴ�.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        GameStateManager[] obj = FindObjectsOfType<GameStateManager>(); 
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }
}
