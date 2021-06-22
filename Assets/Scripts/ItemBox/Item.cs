using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private PlayerMove playerMove = null;
    public AudioSource audioSource = null;

    [SerializeField] private AudioClip[] sound;

    enum ItemEnum
    {
        ATKUP, DEFUP, HPUP
    }

    [SerializeField] private ItemEnum itemCategory;

    public int atkUpValue = 10;
    public int defUpValue = 3;
    public int hpUpValue = 20;

    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        if (playerMove == null) Debug.LogError("PlayerMove�� �����ϴ�.");

        if (audioSource == null) Debug.LogError("audioSource�� �����ϴ�.");

        if (sound[0] == null) Debug.LogError("sound�� 1�� �̻� �־�� �մϴ�.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PLAYER")
        {
            switch (itemCategory)
            {
                case ItemEnum.ATKUP:
                    playerMove.damage += atkUpValue;
                    break;
                case ItemEnum.DEFUP:
                    playerMove.def += defUpValue;
                    break;
                case ItemEnum.HPUP:
                    playerMove.Hp += hpUpValue;
                    break;
            }

            audioSource.clip = sound[Random.Range(0, sound.Length)];
            audioSource.Play();

            gameObject.SetActive(false);
        }
    }
}
