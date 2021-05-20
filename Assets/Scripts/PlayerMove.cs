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

    public GameObject damagePanel;

    public int maxHp = 100;
    private int hp;
    public Text hpText;
    public Image hpBar;

    public GameObject gameOverPanel;
    public Text gameOverScoreText;

    public ParticleSystem particle;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<CapsuleCollider>();

        hp = maxHp;
        hpText.text = "HP: " + hp + '/' + maxHp;
        hpBar.fillAmount = (float)hp / maxHp;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (!GameManager.Instance.isPlay) return;

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

        if(transform.position.y <= -50f)
        {
            GameOver();
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

            particle.Play();

            myCollider.enabled = true;
        }
    }

    public void Damage(int damage)
    {
        damagePanel.SetActive(true);
        Invoke("EndDamage", 0.2f);

        hp -= damage;

        if (hp < 0) hp = 0;

        hpText.text = "HP: " + hp + '/' + maxHp;
        hpBar.fillAmount = (float)hp / maxHp;

        if (hp == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameManager.Instance.isPlay = false;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + GameManager.Instance.Score;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void EndDamage()
    {
        damagePanel.SetActive(false);
    }
}
