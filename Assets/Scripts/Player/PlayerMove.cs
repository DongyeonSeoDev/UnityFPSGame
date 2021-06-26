using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public bool autoGun = false;

    public float lookSensitivity;
    public float cameraRotationLimit;

    private float currentCameraRotationX = 0f;

    public Camera theCamera;

    [Header("Audio clips")]
    public AudioClip fireSound;

    public GameObject damagePanel;

    public int maxHp = 100;
    private int hp;

    public Text hpText;
    public Image hpBar;

    public Text gameOverScoreText;
    public ParticleSystem particle;
    public GameObject gun;
    private StringBuilder sb = new StringBuilder(11);

    public int def = 0;
    private bool isCollision = false;

    public LayerMask whatIsAttackable;

    public int Hp 
    {
        get { return hp; }
        set 
        {
            if (value > maxHp)
            {
                hp = maxHp;
            }
            else
            {
                hp = value;
            }

            if (hp < 0) hp = 0;

            sb.Remove(0, sb.Length);
            sb.Append("HP: ");
            sb.Append(hp);
            sb.Append('/');
            sb.Append(maxHp);
            hpText.text = sb.ToString();
            hpBar.fillAmount = (float)hp / maxHp;

            if (hp == 0)
            {
                GameOver();
            }
        }
    }

    GameStateManager gameStateManager = null;

    private void Awake()
    {
        gameStateManager = GameStateManager.Instance;

        if (gameStateManager == null)
        {
            Debug.LogError("gameStateManager가 없습니다.");
        }

        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();

        damage = gameStateManager.playerDamage;
        Hp = gameStateManager.playerHP;
        def = gameStateManager.playerDef;
        autoGun = gameStateManager.autoGun;
        GameManager.Instance.GunModeUIChange(autoGun ? 1 : 0);
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

        if (Input.GetButton("Fire1") && autoGun == true)
        {
            Fire(damage);
        }
        else if (Input.GetButtonDown("Fire1") && autoGun == false)
        {
            Fire(damage * 1.5f);
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            autoGun = !autoGun;
            GameManager.Instance.GunModeUIChange(autoGun ? 1 : 0);
        }
        if(transform.position.y <= -50f)
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WALL"))
        {
            isCollision = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isCollision = false;
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * x;
        Vector3 moveVertical = transform.forward * z;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        if (isCollision)
        {
            return;
        }

        myRigidbody.MovePosition(transform.position + velocity);
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxis("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;

        if (isCollision)
        {
            cameraRotationX *= 0.01f;
        }
        
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

            bool rayCastValue = Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out hit, range, whatIsAttackable);

            if (rayCastValue)
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(damage);
                    PoolManager.GetItem<DamageText>().ShowText(damage.ToString(), hit.transform.position, transform.position);
                }
            }

            particle.Play();
        }
    }

    public void Damage(int damage)
    {
        damagePanel.SetActive(true);
        Invoke("EndDamage", 0.2f);

        if (damage - def < 1)
        {
            Hp -= 1;
        }
        else
        {
            Hp -= (damage - def);
        }
    }

    private void GameOver()
    {
        GameManager.Instance.GameOver();
        gun.SetActive(false);
    }

    private void EndDamage()
    {
        damagePanel.SetActive(false);
    }
}
