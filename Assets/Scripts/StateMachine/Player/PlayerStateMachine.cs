using UnityEngine;
using Siphonn.Enemy;

namespace Siphonn.Player
{
    public class PlayerStateMachine : MonoBehaviour
    {

        private PlayerStateFactory _states;
        public BaseState currentState;
        public Rigidbody playerRb;
        public Animator playerAnim;

        [Header("Stats")]
        public float moveSpeed = 0.7f;
        public float jumpForce = 2.5f;

        [Header("INPUTS")]
        public float horzMovement;
        public float vertMovement;
        public float move;
        //public bool lightAttackButton;
        //public bool heavyAttackButton;
        public bool chargeRelease;
        public int lightButtonCount;
        public int heaveButtonCount;
        public bool facingRight = true;
        public bool onGround;
        public bool jumpPressed;
        public bool dashing;
        public bool hitStunned;
        public bool knockedDown;

        [Header("Attack values")]
        public Transform attackPoint;
        public Transform attackCenterPoint;
        public float attackRadius = 0.2f;
        public bool attacking;
        public bool attackFinished;
        public bool attackConnected;
        public bool enemyHitCheck;
        public int maxComboLength = 5;

        [Header("Grab values")]
        public Transform grabPoint;
        public float grabRadius = 0.1f;
        public bool isGrabbing;
        public EnemyStateMachine grabbedEnemy;

        private float prevHorzMove;
        private float prevVertMove;
        private float maxDashInput = 0.75f;
        private float dashInputTime;
        private float dashResetDelay = 0.3f;
        private float dashHorzCount;
        public float dashSpeed = 2f;

        [Header("State Cooldown")]
        public float attackCooldown = 0.25f;
        public float specialAttackCooldown = 1f;
        public float jumpCooldown = 1.5f;
        public float grabCooldown = 0.5f;
        public float idlePoseTime = 30.0f;

        [Header("Recovery times")]
        public float hitRecovery = 0.5f;
        public float knockdownRecovery = 2f;


        void Awake()
        {
            playerRb = GetComponent<Rigidbody>();
            playerAnim = GetComponent<Animator>();

            _states = new PlayerStateFactory(this);
            currentState = _states.Wait();
            currentState.EnterState();
        }

        private void Update()
        {
            PlayerInputs();
            DashCheck();
        }

        private void FixedUpdate()
        {
            currentState.UpdateState();
        }

        /// <summary>
        /// Collision event used to set "onGround" bool
        /// </summary>
        private void OnCollisionEnter(Collision col)
        {
            if (col.transform.tag == "Ground")
            {
                onGround = true;
                lightButtonCount = 0;
                heaveButtonCount = 0;
            }
        }

        private void PlayerInputs()
        {
            /// MOVEMENT
            prevHorzMove = horzMovement;
            prevVertMove = vertMovement;
            horzMovement = Input.GetAxisRaw("Horizontal");
            vertMovement = Input.GetAxisRaw("Vertical");
            move = new Vector2(horzMovement, vertMovement).magnitude;

            /// BUTTONS
            //lightAttackButton = Input.GetButtonDown("Attack");
            //heavyAttackButton = Input.GetButtonDown("Special");
            chargeRelease = Input.GetButtonUp("Attack");

            if (Input.GetButtonDown("Attack")) { lightButtonCount++; }
            if (Input.GetButtonDown("Special")) { heaveButtonCount++; }
            if (Input.GetButtonDown("Jump") && onGround) { jumpPressed = true; }
        }

        private void DashCheck()
        {
            if (!dashing)
            {
                ///Dash left and right
                if (horzMovement >= maxDashInput && prevHorzMove < maxDashInput || horzMovement <= -maxDashInput && prevHorzMove > -maxDashInput)
                {
                    dashInputTime = Time.time;
                    if (dashHorzCount >= 0 && Time.time < dashInputTime + dashResetDelay)
                    {
                        dashHorzCount++;
                    }
                    if (dashHorzCount == 2) //Start dashing
                    {
                        dashing = true;
                        return;
                    }
                }
                if (Time.time > dashInputTime + dashResetDelay && dashHorzCount > 0)//Reset dash counter when time registered dash input
                {
                    dashHorzCount = 0;
                }
            }
            else
            {
                if (horzMovement == 0)
                {
                    dashHorzCount = 0;
                    playerAnim.speed = 1;
                    dashing = false;
                }
            }
        }

        public void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

        /// <summary>
        /// Animation Event.
        /// In AttackState, if enemy check is true. Event that checks for enemies is fired. Then set to false.
        /// </summary>
        public void HitCheckEvent()
        {
            enemyHitCheck = true;
        }

        /// <summary>
        /// Animation Event.
        /// Check for enemies & set them to hit.
        /// </summary>
        public void HitEnemies()
        {
            Collider[] enCol = Physics.OverlapSphere(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy"));

            if (enCol != null)
            {
                foreach (Collider en in enCol)
                {
                    EnemyStateMachine _en = en.GetComponent<EnemyStateMachine>();
                    if (_en.onGround && !_en.knockedDown || !_en.onGround && _en.knockedDown)
                    {
                        _en.hitStunned = true;
                        _en.hitWhileStunned = true;
                        attackConnected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Animation Event, called at the end of an attack animation. 
        /// If the attack button has been pressed. The combo will continue.
        /// If not the combo will end.
        /// </summary>
        public void ComboChecker()
        {
            if (attackConnected)
            {
                attackConnected = false;

                if (lightButtonCount > 0)
                {
                    Debug.Log("Continue Combo");
                    attacking = true;
                }
                else
                {
                    Debug.Log("No attack input. Combo Ended.");
                    playerAnim.SetBool("Attack", false);
                    attackFinished = true;
                }
            }
            else
            {
                Debug.Log("Combo End because no hit connected.");
                playerAnim.SetBool("Attack", false);
                lightButtonCount = 0;
                heaveButtonCount = 0;
                attacking = false;
                attackFinished = true;
            }
        }

        /// <summary>
        /// Animation Event, called on the last combo animation.
        /// </summary>
        public void EndCombo()
        {
            Debug.Log("BIG FINISH!!");
            playerAnim.SetBool("Attack", false);
            attackFinished = true;
        }

        /// <summary>
        /// Animation Event.
        /// Called from final attack "ATTACK 5" animation.
        /// </summary>
        public void KnockBackEnemies()
        {
            Collider[] enCol = Physics.OverlapSphere(attackPoint.position, attackRadius, LayerMask.GetMask("Enemy"));
            if (enCol != null)
            {
                foreach (Collider en in enCol)
                {
                    EnemyStateMachine _en = en.GetComponent<EnemyStateMachine>();
                    if (_en && !_en.knockedDown)
                    {
                        _en.knockedDown = true;
                        Vector3 direction = transform.position - en.transform.position;
                        direction = direction.normalized;
                        Vector2 forceDir = (direction.x > 0) ? new Vector2(-1, 1) : new Vector2(1, 1);
                        _en.enemyRb.AddForce(forceDir * 80, ForceMode.Force);
                    }
                }
            }
        }

        /// <summary>
        /// Animation Event.
        /// Called from special (nuetral) "SPIN KICK" animation.
        /// </summary>
        public void KnockBackSpecialNuetral()
        {
            Collider[] enCol = Physics.OverlapSphere(attackCenterPoint.position, attackRadius * 2.5f, LayerMask.GetMask("Enemy"));

            if (enCol != null)
            {
                foreach (Collider en in enCol)
                {
                    EnemyStateMachine _en = en.GetComponent<EnemyStateMachine>();
                    if (_en && !_en.knockedDown)
                    {
                        _en.knockedDown = true;
                        Vector3 direction = transform.position - en.transform.position;
                        direction = direction.normalized;
                        Vector2 forceDir = (direction.x > 0) ? new Vector2(-1, 1) : new Vector2(1, 1);
                        _en.enemyRb.AddForce(forceDir * 90, ForceMode.Force);
                    }
                }
            }
        }

        /// <summary>
        /// Animation Event.
        /// Called from special (direction) "HADOKEN" animation.
        /// </summary>
        public void KnockBackSpecialDirection()
        {
            Collider[] enCol = Physics.OverlapSphere(attackPoint.position, attackRadius * 1.5f, LayerMask.GetMask("Enemy"));
            if (enCol != null)
            {
                foreach (Collider en in enCol)
                {
                    EnemyStateMachine _en = en.GetComponent<EnemyStateMachine>();
                    if (_en && !_en.knockedDown)
                    {
                        _en.knockedDown = true;
                        Vector3 direction = transform.position - en.transform.position;
                        direction = direction.normalized;
                        Vector2 forceDir = (direction.x > 0) ? new Vector2(-1, 1) : new Vector2(1, 1);
                        _en.enemyRb.AddForce(forceDir * 90, ForceMode.Force);
                    }
                }
            }
        }
    }
}