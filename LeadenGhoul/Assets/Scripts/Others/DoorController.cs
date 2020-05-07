using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("isOpen", true);
    }
}
