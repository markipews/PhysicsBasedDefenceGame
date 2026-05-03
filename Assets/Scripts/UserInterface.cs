using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI difficultyMultiplierText;
    TextMeshProUGUI healthText;
    TextMeshProUGUI turretPriceText;
    
    Score score;

    private Spawner enemySpawner;
    Health baseHealth;

    private Buy buyScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        difficultyMultiplierText = GameObject.FindGameObjectWithTag("DifficultyMultiplier").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<TextMeshProUGUI>();
        turretPriceText = GameObject.FindGameObjectWithTag("TurretPriceUI").GetComponent<TextMeshProUGUI>();
        
        
        score = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Score>();
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<Spawner>();
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<Health>();
        
        buyScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Buy>();
    }

    // Update is called once per frame
    void Update()
    {

        if (score.score >= buyScript.basicTurretPrice)
        {
            scoreText.color = new Color(0.7f, 1f, 0.7f);
            scoreText.text = "Purchase Ready. Press B! " + score.score.ToString("F2") +  " $";
        }
        else
        {
            scoreText.color = new Color(1f, 0.7f, 0.7f);  
            scoreText.text = score.score.ToString("F2") +  " $";            
        }

        turretPriceText.text = "TURRET PRICE: " + buyScript.basicTurretPrice.ToString("F2");
        difficultyMultiplierText.text = "DIFFICULTY: " + enemySpawner.difficultyMultiplier.ToString("F2") + "x";
        
        healthText.text =  "HEALTH: " + baseHealth.health.ToString("F0") + " HP";
        if (baseHealth.health > 50)
        {
            healthText.color = new Color(1f, 1f, 1f);  
        } else if (baseHealth.health is <= 50 and > 10)
        {
            healthText.color = new Color(1f, 0.7f, 0.7f);  
        } else if (baseHealth.health <= 10)
        {
            healthText.color = new Color(1f, 0.3f, 0.3f);  
        }
        
    }
}
