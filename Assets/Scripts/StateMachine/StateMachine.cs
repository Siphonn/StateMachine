using UnityEngine;

namespace Siphonn
{
    public class StateMachine : MonoBehaviour
    {
        private StateFactory _states;
        public BaseState currentState;

        void Awake()
        {
            //playerRb = GetComponent<Rigidbody>();
            //playerAnim = GetComponent<Animator>();

            //_states = new StateFactory(this);
            //currentState = _states.Wait();
            //currentState.EnterState();
        }

        private void FixedUpdate()
        {
            currentState.UpdateState();
        }
    }
}
