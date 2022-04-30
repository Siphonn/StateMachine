using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siphonn.Enemy
{
    public class EnemyStateMachine : StateMachine
    {

        //public BaseState currentState;
        private EnemyStateFactory _states;
        public Rigidbody enemyRb;
        public Transform target;

        [Header("Stats")]
        public float speed = 0.3f;
        public float attackDistance = 0.35f;
        public bool onGround = true;
        public bool facingRight;

        [Header("Attack values")]
        public Transform attackPoint;
        public float attackRadius = 0.5f;
        public bool hitStunned;
        public bool hitWhileStunned;
        public bool knockedDown;
        public bool isGrabbed;

        [Header("Cooldown times")]
        public float stateTransitionTime = 1.5f;
        public float hitRecovery = 0.5f;
        public float knockdownRecovery = 1f;

        void Start()
        {
            enemyRb = GetComponent<Rigidbody>();    

            _states = new EnemyStateFactory(this);
            currentState = _states.Wait();
            currentState.EnterState();
        }

        //void FixedUpdate()
        //{
        //    currentState.UpdateState();
        //}

        public void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }
}
