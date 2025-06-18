using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 700f;

    private int jumpCounter = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D playerRigidbody2D;
    private Animator animator;
    private AudioSource playerAudio;

    void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        // UI 요소 위에서 클릭했는지 확인
        bool isClickingUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButtonDown(0) && !isClickingUI && jumpCounter < 2)
        {
            jumpCounter++;
            playerRigidbody2D.linearVelocity = Vector2.zero;
            playerRigidbody2D.AddForce(Vector2.up * jumpForce);
            playerAudio.PlayOneShot(GameManager.instance.jumpClip);
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
        
        playerRigidbody2D.linearVelocity = Vector2.zero;
        isDead = true;
        GameManager.instance.onPlayerDead();
        playerAudio.PlayOneShot(GameManager.instance.dropDeathClip);
    }
}