using UnityEngine;

public class VoidCheck : MonoBehaviour
{
    GameObject parent;
    GameObject grab;
    GameObject main;
    
    Collider2D colliderSelf;
    Collider2D colliderPlayerCursor;
    public GameObject playerCursor;
    
    GameObject voidArea;

    private Pickup pickupScript;

    private MathExponential mathExp;

    private CircleCollider2D mainCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent.gameObject;
        grab = parent.transform.Find("Grab").gameObject;
        main = parent.transform.Find("Main").gameObject;
        
        colliderSelf =  GetComponent<Collider2D>();
        playerCursor = GameObject.FindWithTag("Player");
        colliderPlayerCursor = playerCursor.GetComponent<Collider2D>();
        
        voidArea = GameObject.FindWithTag("Void");
        
        pickupScript = grab.GetComponent<Pickup>();
        
        mathExp = parent.GetComponent<MathExponential>();
        
        mainCollider = main.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inVoid && !inLand && Mathf.Round(parent.transform.localScale.x * 100)/100 <= pickupScript.parentOriginalScale)
        {
            FallIntoVoid();
        }

        //if (inVoid && inLand) make this push the object towards the void
        
        Physics2D.IgnoreCollision(colliderSelf, colliderPlayerCursor);
        
        //Debug.Log("In Void: " + inVoid + " In Land: " + inLand);
    }

    private bool inLand;
    private bool inVoid;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Land") && grab != null /*Here I am checking if the object is not already falling as "Grab"
        is destroyed when falling*/)
            inLand = true;

        if (other.CompareTag("Void"))
            inVoid = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Land"))
            inLand = false;
        
        if (other.CompareTag("Void"))
            inVoid = false;
    }
    
    public void FallIntoVoid()
    {
        mathExp.increaseOnCallNumber = true;
        //Debug.Log("FallIntoVoid");
        parent.transform.localScale = Vector2.MoveTowards(parent.transform.localScale,
            new Vector3(0, 0, 0), 
            Time.deltaTime * mathExp.onCallNumber);
        Destroy(grab);
        mainCollider.isTrigger = true; //Removes collisions when falling
        main.GetComponent<SpriteRenderer>().sortingOrder = -7;
        
        if (parent.transform.localScale.x <= 0)
        {
            mathExp.increaseOnCallNumber = false;
            Destroy(parent);
        }
    }
}
