using UnityEngine;

namespace Assets.Scripts.Behaviour
{
    public class Moving : MonoBehaviour
    {
        private Rigidbody _body;

        void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameRunningAndStarted)
            {
                _body.velocity = new Vector3(_body.velocity.x, _body.velocity.y, -GameManager.GameSpeed);
            }
        }
    }
}
