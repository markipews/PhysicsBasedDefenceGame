using UnityEngine;

public class Enemy : MonoBehaviour
{
    Health health;
    private Rigidbody2D rb;
    
    GameObject playerBase;
    
    private float angleInDeg;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        
        playerBase = GameObject.FindWithTag("Base");
    }

    void Update()
    {
        if (health.knockOutTimer <= 0)
        {
            AttackBase(3);
        }
    }

    void AttackBase(float attackSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, attackSpeed * Time.deltaTime);
        
        Vector2 difference =  playerBase.transform.position - transform.position;
        
        //Calcualte the Radian
        float angleInRad =  Mathf.Atan2(difference.y, difference.x);
        angleInDeg = angleInRad * Mathf.Rad2Deg;
        
        transform.eulerAngles = new Vector3(0, 0, angleInDeg);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball") && health.invncibilityTimer <= 0 && other.gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 5)
        {
            GameObject grabObject = other.transform.Find("Grab").gameObject;
            float objectWeightRaw = grabObject.gameObject.GetComponent<Pickup>().objectWeightRaw;
            health.Damage(objectWeightRaw * other.transform.GetComponent<Rigidbody2D>().linearVelocity.magnitude);
            Debug.Log(other.transform.GetComponent<Rigidbody2D>().linearVelocity.magnitude);
        }
    }
}
