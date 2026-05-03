using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Buy : MonoBehaviour
{
    private Score score; 
    private ColorAdjustments coloradjustments;
    private Volume volume;
    
    MouseHandler mouseHandler;
    LandCheckForCursor landCheckForCursor;

    public float speedToSlowMotion;

    private bool constructionMode;

    public float basicTurretPrice;
    public float basicTurretPriceOriginal
        ;
    public GameObject basicTurret;
    private Spawner spawnerScript;
    
    
    [FormerlySerializedAs("ghostTurretBasic")] public GameObject ghostTurret;
    SpriteRenderer ghostTurretSprite;
    
    //private Collider2D mouseCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basicTurretPriceOriginal = basicTurretPrice;
        score = GetComponent<Score>();
        volume = GameObject.FindGameObjectWithTag("VignetteController").GetComponent<Volume>();
        if (!volume.profile.TryGet(out coloradjustments))
        {
            Debug.LogError("No ColorAdjustments found on " + gameObject.name);
        }
        
        mouseHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseHandler>();
        landCheckForCursor = GameObject.FindGameObjectWithTag("Player").GetComponent<LandCheckForCursor>();

        //mouseCollider = mouseHandler.mouseCollider.GetComponent<Collider2D>();

        ghostTurretSprite = ghostTurret.GetComponent<SpriteRenderer>();

        spawnerScript = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<Spawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (score.score - basicTurretPrice >= 0 && Input.GetKeyDown(KeyCode.B)) //If you have enough cash for basic turret
        {
            constructionMode  = !constructionMode;
        }
        else if (score.score - basicTurretPrice < 0)
        {
            constructionMode  = false;
        }
        
        ConstructionMode(constructionMode);
        
        //Debug.Log(basicTurretPrice);
        basicTurretPrice = basicTurretPriceOriginal * spawnerScript.difficultyMultiplier/2; /*Slows down increase*/
    }


    [FormerlySerializedAs("buyModePublicLock")] public bool buyModeLock;
    void ConstructionMode(bool enabledToggle)
    {
        if (enabledToggle)
        {
            coloradjustments.saturation.value = Mathf.Lerp(coloradjustments.saturation.value, -75f, speedToSlowMotion * Time.deltaTime);  
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.1f, speedToSlowMotion * Time.deltaTime);
            ghostTurret.transform.position = mouseHandler.mousePosFinal;
            buyModeLock = true;

            if (landCheckForCursor.landSelected && !landCheckForCursor.turretSelected)
            {
                ghostTurretSprite.color = new Color(0f, 1f, 0f, ghostTurretSprite.color.a);
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(basicTurret, mouseHandler.mousePosFinal, Quaternion.identity);
                    score.score -= basicTurretPrice;
                }
            }
            else
            {
                ghostTurretSprite.color = new Color(1f, 0f, 0f, ghostTurretSprite.color.a);
            }
            

            
        }
        else if (Time.timeScale < 1)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, speedToSlowMotion * Time.deltaTime);
            coloradjustments.saturation.value = Mathf.Lerp(coloradjustments.saturation.value, 0f, 7 * Time.deltaTime);
            ghostTurret.transform.position = new Vector3(1000, 1000, 0);
            buyModeLock = false;
        }
    }
    
}
