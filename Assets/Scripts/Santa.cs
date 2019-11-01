using UnityEngine;

namespace Assets.Scripts
{
    public class Santa : MonoBehaviour
    {
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            animator.SetFloat("GameSpeed", GameManager.GameSpeed);
        }

    }
}
