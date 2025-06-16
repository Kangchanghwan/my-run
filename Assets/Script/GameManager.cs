using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUI;

    public AudioClip jumpClip;
    public AudioClip dropDeathClip;
    public AudioClip spikeDeathClip;
    
    public static GameManager instance;
    
    private int score = 0;
    private bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void AddScore(int amount)
    {
        if (isGameOver)
        {
            return;
        }

        score += amount;
        scoreText.text = "Score : " + score;
    }

    public void onPlayerDead()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
    }
    
}
