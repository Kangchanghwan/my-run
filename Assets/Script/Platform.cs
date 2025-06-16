using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{   
    public GameObject[] obstacles;
    private bool steped = false;

    void OnEnable()
    {
        foreach (GameObject obstacle in obstacles)
        {
            if (Random.Range(0, 3) == 0)
            {
                obstacle.SetActive(true);
            }
            else
            {
                obstacle.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" && !steped)
        {
            steped = true;
            GameManager.instance.AddScore(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
