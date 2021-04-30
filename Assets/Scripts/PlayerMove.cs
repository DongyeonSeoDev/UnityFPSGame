using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;
    public float fireDelay = 0.1f;
    Rigidbody rigid;

    public GameObject firePosition;
    public float range = 50f;
    public float damage = 10f;

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
        rigid.velocity = transform.forward.normalized * (z * speed * Time.deltaTime);
        rigid.rotation = rigid.rotation * Quaternion.Euler(0, x * rotationSpeed * Time.deltaTime, 0);

        if(Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (Time.time - lastFireTime > fireDelay)
        {
            audioSource.clip = fireSound;
            audioSource.Play();
            lastFireTime = Time.time;

            RaycastHit hit;
            if (Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out hit, range))
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(damage);
                }
            }
        }
    }
}
