using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 700f;

    private int jumpCounter = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D playerRigidbody2D;
    private Animator animator;

    void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && jumpCounter < 2)
        {
            jumpCounter++;
            playerRigidbody2D.linearVelocity = Vector2.zero;
            playerRigidbody2D.AddForce(Vector2.up * jumpForce);
            //TODO:: AUDIO 재생 추가
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidbody2D.linearVelocity.y > 0)
        {
            playerRigidbody2D.linearVelocity *= 0.5f;
        }

        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("YLinearVelocity", playerRigidbody2D.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCounter = 0;
            playerRigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        
        // 오디오 재생
        playerRigidbody2D.linearVelocity = Vector2.zero;
        isDead = true;
    }
}