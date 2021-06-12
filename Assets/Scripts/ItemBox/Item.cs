using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private PlayerMove playerMove = null;

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
        if (playerMove == null) Debug.LogError("PlayerMove가 없습니다.");
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

            gameObject.SetActive(false);
        }
    }
}
