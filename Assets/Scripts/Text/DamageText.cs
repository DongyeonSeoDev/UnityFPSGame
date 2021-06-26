using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public float moveY;
    public float moveDuration;
    public float alphaDuration;

    private TextMesh text = null;

    private void Awake()
    {
        text = FindObjectOfType<TextMesh>();

        if (text == null)
        {
            Debug.LogError("text�� �����ϴ�.");
        }
    }

    public void ShowText(string textValue, Vector3 startPosition, Vector3 lookPosition)
    {
        transform.position = startPosition;
        transform.LookAt(lookPosition);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180f, transform.rotation.eulerAngles.z);
        text.text = textValue;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

        transform.DOMoveY(transform.position.y + moveY, moveDuration).OnComplete(() =>
        {
            DOTween.To(() => text.color, x => text.color = x, new Color(text.color.r, text.color.g, text.color.b, 0f), alphaDuration);
        });
    }
}
