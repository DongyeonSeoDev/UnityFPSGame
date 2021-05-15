using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float fireDelay = 0.2f;

    public GameObject firePosition;
    public float range = 50f;
    public float damage = 10f;

    private AudioSource audioSource;
    private Rigidbody myRigidbody;

    private float lastFireTime = 0f;

    private bool auto = false;

    public float lookSensitivity;
    public float cameraRotationLimit;

    private float currentCameraRotationX = 0f;

    public Camera theCamera;

    [Header("Audio clips")]
    public AudioClip fireSound;

    private Collider myCollider;

    public CanvasGroup damagePanel;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        CameraRotation();
        CharacterRotation();

        if (Input.GetButton("Fire1") && auto == true)
        {
            Fire(damage);
        }
        else if (Input.GetButtonDown("Fire1") && auto == false)
        {
            Fire(damage * 2);
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            auto = !auto;
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * x;
        Vector3 moveVertical = transform.forward * z;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        myRigidbody.MovePosition(transform.position + velocity);
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxis("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float yRotation = Input.GetAxis("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        transform.Rotate(transform.rotation.normalized * characterRotationY);
    }

    private void Fire(float damage)
    {
        if (Time.time - lastFireTime > fireDelay)
        {
            audioSource.clip = fireSound;
            audioSource.Play();
            lastFireTime = Time.time;

            RaycastHit hit;
            myCollider.enabled = false;
            if (Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out hit, range))
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(damage);
                }
            }
            myCollider.enabled = true;
        }
    }

    public void Damage()
    {
        damagePanel.alpha = 1;
        Invoke("EndDamage", 0.2f);
    }

    private void EndDamage()
    {
        damagePanel.alpha = 0;
    }
}
