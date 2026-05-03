using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickup : MonoBehaviour
{
    public bool isPickedUp;
    private bool isSelected;

    private const float holdSpeed = 14f;
    private const float holdDistance = 10f;

    public float dropThreshold = 500f;
    
    private const float selectSpeed = 14f;
    
    SpriteRenderer spriteRenderer;
    
    CameraEffects cameraShake;
    
    GameObject parent;
    GameObject main;
    
    const float GRAVITY = 9.81f;
    
    MathExponential mathExponential;

    private bool isAffectedByGravity;
    
    [FormerlySerializedAs("objectWeightInput")] [SerializeField] public float objectWeightRaw;
    private float objectWeight;
    
    public float parentOriginalScale;

    private Vector2 cursorPosRelativeToThis;

    private PhysicsMaterial2D mat;
    private float materialBounciness;

    private Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent.gameObject;
        main = parent.transform.Find("Main").gameObject;
        spriteRenderer = main.GetComponent<SpriteRenderer>();
        parentOriginalScale = (parent.transform.localScale.x + parent.transform.localScale.y)/2;
        cameraShake = FindFirstObjectByType<CameraEffects>();
        
        mathExponential = parent.GetComponent<MathExponential>();
        
        mat = parent.GetComponent<Rigidbody2D>().sharedMaterial;
        health = main.GetComponent<Health>();

    }
    
    // Update is called once per frame
    void Update()
    {
        materialBounciness = mat.bounciness;
        
        objectWeight = (1f / objectWeightRaw) * 10;
        
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPickedUp = true;
            } else if (Input.GetMouseButtonUp(0))
            {
                isPickedUp = false;
            }
        }
        else
        {
            if (!Input.GetMouseButton(0) && isPickedUp)
                isPickedUp = false;
        }
        
        spriteRenderer.color = Color32.Lerp(spriteRenderer.color,
            isSelected
                ? new Color32(255,
                    255,
                    255,
                    100)
                : new Color32(255,
                    255,
                    255,
                    255),
            selectSpeed * Time.deltaTime);

        if (isPickedUp)
        {
            transform.parent.localScale = Vector3.Lerp(
                transform.parent.localScale, 
                new Vector3(parentOriginalScale+objectWeight/holdDistance, parentOriginalScale+objectWeight/holdDistance, transform.parent.localScale.z),
                holdSpeed * Time.deltaTime
            );
            
            ToggleColliders(false, 1);
        }
        

        
        transform.localScale = Vector3.Lerp(
            transform.localScale, 
            isPickedUp ? new Vector3(objectWeight, objectWeight, transform.parent.localScale.z) : new Vector3(parentOriginalScale/2f, parentOriginalScale/2f, transform.parent.localScale.z),
            holdSpeed * Time.deltaTime
        );

        cursorPosRelativeToThis = MouseHandler.Instance.mousePosFinal - (Vector2) transform.position;
        
        if (isPickedUp && isSelected)
        {
            parent.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(
                cursorPosRelativeToThis.x * objectWeight,
                cursorPosRelativeToThis.y * objectWeight
            );
            
            parent.GetComponent<Rigidbody2D>().linearDamping = objectWeightRaw;
            parent.GetComponent<Rigidbody2D>().mass = objectWeightRaw;
        }
        
        if (isPickedUp && !isSelected)
        {
            StartDropping();
        }
        
        if (!isPickedUp)
        {
            ToggleColliders(true, 0);
            isAffectedByGravity = true;

            if (setTarget)
            {
                targetGrowth = ((parent.transform.localScale.x + parent.transform.localScale.y) / 2) * materialBounciness;
                setTarget = false;
            }
        }
        else
        {
            isAffectedByGravity = false;
            setTarget = true;
        }

        if (isAffectedByGravity)
        {
            Bounce();
        }
    }
    
    bool setTarget = true;

    private float targetGrowth;

    private bool isGrowing;
    void Bounce()
    {

        if (Mathf.Round(parent.transform.localScale.x * 100)/100 > parentOriginalScale && !isGrowing)
        {
            mathExponential.increaseOnCallNumber = true;
            parent.transform.localScale -= new Vector3((Time.deltaTime * mathExponential.onCallNumber) / objectWeightRaw  , (Time.deltaTime * mathExponential.onCallNumber) / objectWeightRaw, parent.transform.localScale.z);
            return;
        }
        mathExponential.increaseOnCallNumber = false;

        if (!isGrowing)
        {
            targetGrowth *= materialBounciness;
        }
        
        isGrowing = true;
        
        if (parent.transform.localScale.x < parentOriginalScale + targetGrowth && isGrowing)
        {
            parent.transform.localScale = Vector3.Lerp(parent.transform.localScale,
                new Vector3(parentOriginalScale + targetGrowth + 0.1f /*LeniencyBandageFix*/, parentOriginalScale + targetGrowth + 0.1f /*LeniencyBandageFix*/, parent.transform.localScale.z),
                Time.deltaTime * GRAVITY);
            return;
        }
        
        isGrowing = false;
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isSelected = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isSelected = false;
        }
    }
    
    int shakeConstrainer = 1000;
    void StartDropping()
    {
        parent.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(
            cursorPosRelativeToThis.x * mathExponential.decreaseExp * objectWeight,
            cursorPosRelativeToThis.y * mathExponential.decreaseExp * objectWeight
        );
        
        cameraShake.ShakeCamera(0.1f, mathExponential.increaseExp/shakeConstrainer, true, true);
        
        if (mathExponential.increaseExp > dropThreshold)
        {
            isPickedUp = false;
        }
    }

    void ToggleColliders(bool enableColliders, int layer)
    {
        main.GetComponent<CircleCollider2D>().enabled = enableColliders;
        main.GetComponent<SpriteRenderer>().sortingOrder = layer;
    }
    
}
