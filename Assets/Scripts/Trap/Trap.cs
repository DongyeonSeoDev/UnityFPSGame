using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    private MakeMaze makeMaze = null;

    private readonly int hashIsShow = Animator.StringToHash("IsShow");

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("animator가 없습니다.");
        }

        makeMaze = FindObjectOfType<MakeMaze>();

        if (makeMaze == null)
        {
            Debug.LogError("makeMaze가 없습니다.");
        }

        int randomNum = Random.Range(0, makeMaze.enablePosition.Count);
        transform.position = makeMaze.enablePosition[randomNum];
        makeMaze.enablePosition.RemoveAt(randomNum);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            animator.SetBool(hashIsShow, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            animator.SetBool(hashIsShow, false);
        }
    }
}
