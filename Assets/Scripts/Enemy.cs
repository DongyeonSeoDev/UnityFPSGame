using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Renderer boxRenderer;
    private Color color;
    public float hp = 100;

    private void Awake()
    {
        boxRenderer = GetComponent<Renderer>();
    }

    public void OnDamage(float damage)
    {
        hp -= damage;

        color = Color.red;
        ChangeColor();
        color = Color.white;
        Invoke("ChangeColor", 0.1f);

        if (hp <= 0)
        {
            Die();
            Invoke("Spawn", 5f);
        }
    }

    private void ChangeColor()
    {
        boxRenderer.material.SetColor("_Color", color);
    }

    private void Spawn()
    {
        hp = 100;
        gameObject.SetActive(true);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
