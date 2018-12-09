using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnowball : MonoBehaviour {

    private Rigidbody2D m_Rigidbody2D;

    private float m_MoveSpeed = 2.0f;
    private bool m_MoveRight = false;

    [SerializeField] private Transform m_WallCheck;
    private float m_WallCheckRadius = 0.9f;
    [SerializeField] private LayerMask m_WhatIsWall;
    private bool m_HittingWall = false;

	// Use this for initialization
	void Start ()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        m_HittingWall = Physics2D.OverlapCircle(m_WallCheck.position, m_WallCheckRadius, m_WhatIsWall);
        
        if (m_HittingWall)
        {
            m_MoveRight = !m_MoveRight;
        }

        if (m_MoveRight)
        {
            m_Rigidbody2D.velocity = new Vector2(m_MoveSpeed, m_Rigidbody2D.velocity.y);
        }
        else if (!m_MoveRight)
        {
            m_Rigidbody2D.velocity = new Vector2(-m_MoveSpeed, m_Rigidbody2D.velocity.y);
        }
    }
}
