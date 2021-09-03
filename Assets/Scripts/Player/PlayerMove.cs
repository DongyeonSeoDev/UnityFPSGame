using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float fireDelay = 0.5f;
    public float continuousFireDelay = 0.2f;

    public GameObject firePosition;
    public float range = 50f;
    public float damage = 10f;

    private Rigidbody myRigidbody;

    private float lastFireTime = 0f;

    public bool autoGun = false;

    public float lookSensitivity;
    public float cameraRotationLimit;

    private float currentCameraRotationX = 0f;

    public Camera theCamera;

    [Header("Audio clips")]
    public AudioClip fireSound;
    public AudioClip switchGunSound;
    public AudioClip reloadSound1;
    public AudioClip reloadSound2;

    public GameObject damagePanel;

    public int maxHp = 100;
    private int hp;

    public Text hpText;
    public Image hpBar;

    public ParticleSystem particle;
    public GameObject gun;
    private StringBuilder sb = new StringBuilder(11);

    public int def = 0;
    private bool isCollision = false;

    private bool isGameOver = false;
    private bool isAttackable = true;

    public GameObject soundObject = null;

    public int currentBulletCount = 30;
    private int maxBulletCount = 30;

    public float reloadDelay = 3.0f;

    private bool isReload = false;

    private GunAnimation gunAnimation = null;
    private GameStateManager gameStateManager = null;

    private GameManager gameManager = null;

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
            sb.Append("Ã¼·Â : ");
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

    private void Awake()
    {
        try
        {
            gameStateManager = GameStateManager.Instance;
            gameManager = GameManager.Instance;

            gunAnimation = FindObjectOfType<GunAnimation>();
            myRigidbody = GetComponent<Rigidbody>();

            damage = gameStateManager.playerDamage;
            Hp = gameStateManager.playerHP;
            currentBulletCount = gameStateManager.bulletCount;
            def = gameStateManager.playerDef;
            autoGun = gameStateManager.autoGun;
            gameManager.GunModeUIChange(autoGun ? 1 : 0);

            gameManager.BulletCountUI(currentBulletCount);
            gameManager.AttackTextUI((int)damage);
            gameManager.DefTextUI(def);

            PoolManager.CreatePool<Sound>(soundObject, transform, 10);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (!gameManager.isPlay) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.Pause();
        }

        if (gameManager.isPause) return;

        CameraRotation();
        CharacterRotation();

        if (transform.position.y <= -50f)
        {
            GameOver();
        }

        if (!isAttackable || isReload) return;

        if (Input.GetButton("Fire1") && autoGun == true)
        {
            Fire(damage, continuousFireDelay);
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            Fire(damage * 2f, fireDelay);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            autoGun = !autoGun;
            gameManager.GunModeUIChange(autoGun ? 1 : 0);

            isAttackable = false;
            PoolManager.GetItem<Sound>().soundPlay(switchGunSound, 1f, 2f);
            Invoke("switchGunModeChange", 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void switchGunModeChange()
    {
        isAttackable = true;
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

        if (velocity == Vector3.zero) gunAnimation.MoveEnd();
        else gunAnimation.MoveStart();

        myRigidbody.MovePosition(transform.position + velocity);
    }

    public void CameraRotation(float xRotation = 0)
    {
        xRotation = xRotation == 0 ? Input.GetAxis("Mouse Y") : xRotation;
        float cameraRotationX = xRotation * lookSensitivity;

        if (isCollision)
        {
            return;
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

    private void Fire(float damage, float delay)
    {
        if (currentBulletCount <= 0) return;

        if (Time.time - lastFireTime > delay)
        {
            PoolManager.GetItem<Sound>().soundPlay(fireSound, 0.5f, 2f);
            
            lastFireTime = Time.time;

            RaycastHit hit;

            int randomNumber = UnityEngine.Random.Range(0, 10);
            damage = damage + randomNumber - 5;

            if (Physics.Raycast(firePosition.transform.position, firePosition.transform.forward, out hit, range))
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.OnDamage(damage);

                    Color color;

                    if (randomNumber <= 2)
                    {
                        color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        color = new Color(1f, 1f * ((float)(10 - randomNumber) / 10), 0f, 1f);
                    }
                    
                    float sizeValue = 0.5f + (0.05f * randomNumber);
                    Vector3 size = new Vector3(sizeValue, sizeValue, sizeValue);
                    PoolManager.GetItem<DamageText>().ShowText(damage.ToString(), hit.point, transform.position, size, color);
                }
            }

            particle.Play();
            currentBulletCount--;
            gameManager.BulletCountUI(currentBulletCount);

            if (autoGun)
            {
                gunAnimation.ContinuousFireStart();
            }
            else
            {
                gunAnimation.FireStart();
            }
        }
    }

    private void Reload()
    {
        if (isReload) return;

        currentBulletCount = maxBulletCount;
        isReload = true;

        PoolManager.GetItem<Sound>().soundPlay(reloadSound1, 1f, 1f);

        Invoke("ReloadSound2Play", 1.3f);
        Invoke("EndReload", reloadDelay);
        gunAnimation.ReloadStart();
    }

    private void ReloadSound2Play()
    {
        PoolManager.GetItem<Sound>().soundPlay(reloadSound2, 1f, 1f);
    }

    private void EndReload()
    {
        isReload = false;

        gameManager.BulletCountUI(currentBulletCount);
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

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        gunAnimation.AnimationEnd();
        gameManager.GameOver();
        gun.SetActive(false);
    }

    private void EndDamage()
    {
        damagePanel.SetActive(false);
    }
}
