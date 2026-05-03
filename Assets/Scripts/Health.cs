using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    CameraEffects cameraEffects;
    
    public float invncibilityTimer;

    public float knockOutTimer;
    SpriteRenderer spriteRenderer;
    
    GameStates gameState;
    private Score score;

    void Start()
    {
        cameraEffects = GameObject.FindGameObjectWithTag("CameraShaker").GetComponent<CameraEffects>();
        gameState = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStates>();
        score = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Score>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (health <= 0 && !CompareTag("Base"))
        {
            Destroy(gameObject);
            score.Add(10);
        } else if (health <= 0 && CompareTag("Base"))
        {
            gameState.GameOver();
        }

        if (invncibilityTimer > 0)
        {
            invncibilityTimer -= Time.deltaTime;
        }

        if (knockOutTimer > 0)
        {
            knockOutTimer -= Time.deltaTime;
        }

        if (spriteRenderer.color.b < 255)
        {
            spriteRenderer.color += new Color32(0, 5, 5, 0) /*BY SPEED MULTIPLY*/;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }

    public void Damage(float amount)
    {
        health -= amount;
        invncibilityTimer = 0.1f;
        spriteRenderer.color = new Color32(255, 0, 0, 255);
        
        if (CompareTag("Base"))
        {
            cameraEffects.ShakeCamera(0.1f, 4f, true, true);
            cameraEffects.Vignette(0.5f, Color.red, 0.5f);
            invncibilityTimer = 1f;
            score.Subtract(10);
        }

        if (CompareTag("Enemy"))
        {
            knockOutTimer = 3f;
        }
    }
}
