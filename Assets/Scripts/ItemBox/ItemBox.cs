using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, IDamageable
{
    private float hp = 100f;
    private Color color;

    private Animator boxAnimator = null;
    private BoxCollider boxCollider = null;
    private Renderer boxRenderer = null;

    public GameObject box = null;
    public GameObject destroyBox = null;
    public GameObject portion = null;
    public GameObject effect = null;

    private MakeMaze makeMaze = null;

    private void Awake()
    {
        try
        {
            boxAnimator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
            boxRenderer = box.GetComponent<Renderer>();
            makeMaze = FindObjectOfType<MakeMaze>();

            box.SetActive(true);
            destroyBox.SetActive(false);
            portion.SetActive(false);
            effect.SetActive(true);

            int randomNum = UnityEngine.Random.Range(0, makeMaze.enablePosition.Count);
            transform.position = makeMaze.enablePosition[randomNum];
            makeMaze.enablePosition.RemoveAt(randomNum);
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

            hp = GameStateManager.Instance.Stage * 120;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void OnDamage(float damage)
    {
        hp -= damage;

        color = Color.red;
        ChangeColor();
        color = Color.white;
        Invoke("ChangeColor", 0.1f);

        if (hp < 0)
        {
            box.SetActive(false);
            destroyBox.SetActive(true);
            portion.SetActive(true);
            effect.SetActive(false);
            boxCollider.enabled = false;
            boxAnimator.Play("Bang");
        }
    }

    private void ChangeColor()
    {
        boxRenderer.material.SetColor("_Color", color);
    }
}
