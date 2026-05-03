using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float radius;
    public float shootForce;
    
    private GameObject _parent;
    private GameObject _direction;
    private GameObject visibleRadius;
    
    public GameObject ammoType;
    
    public List<GameObject> enemiesInSight = new List<GameObject>();

    private Buy buyScript;

    private Collider2D selfCollider;

    private Transform barrel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _parent = transform.parent.gameObject;
        _direction = _parent.transform.Find("Direction").gameObject;
        barrel = _direction.transform.Find("Barrel");
        visibleRadius = _parent.transform.Find("VisibleRadius").gameObject;
        
        visibleRadius.transform.localScale = new Vector3(radius, radius, radius);
        //Debug.Log(visibleRadius);
        transform.localScale = new Vector3(radius, radius, radius);
        
        buyScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Buy>();
        
        selfCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesInSight.Count > 0)
        {
            AimAtEnemy();
        }

    }

    void AimAtEnemy()
    {
        Vector2 difference =  enemiesInSight[0].transform.position - transform.position;
        
        //Calcualte the Radian
        float angleInRad =  Mathf.Atan2(difference.y, difference.x);
        float angleInDeg = angleInRad * Mathf.Rad2Deg;
        
        _direction.transform.eulerAngles = new Vector3(0, 0, angleInDeg);
        
        if (!buyScript.buyModeLock)
            ShootEnemy();
    }

    private bool ableToShoot = true;
    
    private void ShootEnemy()
    {
        if (!ableToShoot)
            return;

        ableToShoot = false;
        
        var shotAmmo = Instantiate(ammoType, barrel.transform.position, Quaternion.identity);
        shotAmmo.GetComponent<Rigidbody2D>().AddForce(_direction.transform.right * shootForce, ForceMode2D.Impulse);
        StartCoroutine(Wait(1f));
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        ableToShoot =  true;
    }

    void OnTriggerEnter2D(Collider2D spotted)
    {
        if (spotted.gameObject.CompareTag("Enemy"))
        {
            enemiesInSight.Add(spotted.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D spotted)
    {
        if (spotted.gameObject.CompareTag("Enemy"))
        {
            enemiesInSight.Remove(spotted.gameObject);
        }
    }
}
