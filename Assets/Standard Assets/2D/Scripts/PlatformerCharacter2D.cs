using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private static int m_SnowflakeCount = 0;
        [SerializeField] private TextMeshProUGUI m_TEXTSnowflakeCount;
        
        private static int m_NumLives = 5;
        [SerializeField] private TextMeshProUGUI m_TEXTNumLives;

        private int m_PlayerHealth = 100;
        private bool m_FlashActive = false;
        private const float k_FlashLength = 0.25f; // MIGHT BE CONST
        private float m_FlashCounter = 0.0f;
        private SpriteRenderer m_PlayerSprite;
        [SerializeField] private Slider m_HealthBar;
        private Collider2D m_SnowballToIgnore;
        private const float k_IgnoreLength = 1.0f;
        private float m_IgnoreCounter = 0.0f;
        private BoxCollider2D m_BoxCollider2D;
        private CircleCollider2D m_CircleCollider2D;
        [SerializeField] private GameObject m_GameOver;
        [SerializeField] private GameObject m_End;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_PlayerSprite = GetComponent<SpriteRenderer>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();

            SetUIText();
        }


        void Update()
        {
            SetUIText();
            m_HealthBar.value = m_PlayerHealth;
            if (m_HealthBar.value <= 0)
            {
                m_GameOver.SetActive(true);
                Time.timeScale = 0.0f;
            }

            if (m_FlashActive)
            {
                if (m_FlashCounter > k_FlashLength * 0.66f)
                {
                    m_PlayerSprite.color = new Color(m_PlayerSprite.color.r, m_PlayerSprite.color.g, m_PlayerSprite.color.b, 0.0f);
                }
                else if (m_FlashCounter > k_FlashLength * 0.33f)
                {
                    m_PlayerSprite.color = new Color(m_PlayerSprite.color.r, m_PlayerSprite.color.g, m_PlayerSprite.color.b, 1.0f);
                }
                else if (m_FlashCounter > 0.0f)
                {
                    m_PlayerSprite.color = new Color(m_PlayerSprite.color.r, m_PlayerSprite.color.g, m_PlayerSprite.color.b, 0.0f);
                }
                else
                {
                    m_PlayerSprite.color = new Color(m_PlayerSprite.color.r, m_PlayerSprite.color.g, m_PlayerSprite.color.b, 1.0f);
                    m_FlashActive = false;
                }

                m_FlashCounter -= Time.deltaTime;
            }

            if (m_SnowballToIgnore != null)
            {
                if (m_IgnoreCounter > 0.0f)
                {
                    Physics2D.IgnoreCollision(m_SnowballToIgnore, m_BoxCollider2D, true);
                    Physics2D.IgnoreCollision(m_SnowballToIgnore, m_CircleCollider2D, true);
                    m_IgnoreCounter -= Time.deltaTime;
                }
                else
                {
                    Physics2D.IgnoreCollision(m_SnowballToIgnore, m_BoxCollider2D, false);
                    Physics2D.IgnoreCollision(m_SnowballToIgnore, m_CircleCollider2D, false);
                }
            }

        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Collectable"))
            {
                m_SnowflakeCount++;
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("Present"))
            {
                m_SnowflakeCount += 10;
            }

            if (other.gameObject.CompareTag("End"))
            {
                m_End.SetActive(true);
            }
        }


        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Snowball"))
            {
                m_SnowballToIgnore = col.collider;
                m_IgnoreCounter = k_IgnoreLength;
                HurtPlayer(10);
            }
        }


        void SetUIText()
        {
            m_TEXTSnowflakeCount.text = m_SnowflakeCount.ToString();
            m_TEXTNumLives.text = m_NumLives.ToString();
        }


        void HurtPlayer(int damageToGive)
        {
            m_PlayerHealth -= damageToGive;

            m_FlashActive = true;
            m_FlashCounter = k_FlashLength;
        }

    }

}
