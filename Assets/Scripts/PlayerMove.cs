using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rigid.velocity = transform.forward.normalized * z * speed * Time.deltaTime;
        rigid.rotation = rigid.rotation * Quaternion.Euler(0, x * rotationSpeed * Time.deltaTime, 0);
    }
}
