using Unity.VisualScripting;
using UnityEngine;

public class TurretAmmoSwap : MonoBehaviour
{
    private Turret turretScript;
    Transform turretRadius;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turretRadius = transform.Find("Radius");
        turretScript = turretRadius.GetComponent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turretScript.ammoType.transform.position != transform.position)
        {
            turretScript.ammoType.transform.position = transform.position;
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Snapped");
            turretScript.ammoType = other.gameObject;
        }
    }

}
