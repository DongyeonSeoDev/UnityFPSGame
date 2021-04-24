using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;
    public float fireDelay = 0.1f;
    Rigidbody rigid;

    private AudioSource audioSource;

    private float lastFireTime = 0f;

    [Header("Audio clips")]
    public AudioClip fireSound;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rigid.velocity = transform.forward.normalized * z * speed * Time.deltaTime;
        rigid.rotation = rigid.rotation * Quaternion.Euler(0, x * rotationSpeed * Time.deltaTime, 0);

        if(Input.GetButtonDown("Fire1"))
        {
            if (Time.time - lastFireTime > fireDelay)
            {
                audioSource.clip = fireSound;
                audioSource.Play();
                lastFireTime = Time.time;
            }
        }
    }
}
