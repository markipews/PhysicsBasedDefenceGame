using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StartButton : MonoBehaviour
{
    GameObject InGameUI;
    GameObject StartMenuUI;
    
    Volume volume;
    ColorAdjustments coloradjustments;
    
    MouseHandler mouseHandler;
    Spawner enemySpawner;

    private bool gameHasStarted;
    public void StartGameButton()
    {
        volume = GameObject.FindGameObjectWithTag("VignetteController").GetComponent<Volume>();
        if (!volume.profile.TryGet(out coloradjustments))
        {
            Debug.LogError("No ColorAdjustments found on " + gameObject.name);
        }
        
        gameHasStarted = true;
    }

    public void ExitGameButton()
    {
        Application.Quit();
        Debug.Log("Exited Game.");
    }

    void Start()
    {
        mouseHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseHandler>();
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<Spawner>();
        InGameUI = GameObject.FindGameObjectWithTag("UserInterface");
        StartMenuUI = GameObject.FindGameObjectWithTag("StartMenu");
    }

    private bool timeWarp;
    void Update()
    {
        if (gameHasStarted)
        {
            mouseHandler.mouseCollider.GetComponent<Collider2D>().enabled = true;
            enemySpawner.enabled = true;
            InGameUI.GetComponent<Canvas>().enabled = true;
            StartMenuUI.GetComponent<Canvas>().enabled = false;
            gameHasStarted = false; //Make it run for one loop.
            
            timeWarp = true;
        }

        if (timeWarp && coloradjustments.saturation.value < 1f)
        {
            coloradjustments.saturation.value = Mathf.Lerp(coloradjustments.saturation.value, 1, 5 /*speed*/ * Time.deltaTime);
            return;
        }
        timeWarp = false;
    }
    
}
