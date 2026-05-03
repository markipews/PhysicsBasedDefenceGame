using UnityEngine;

public class LandCheckForCursor : MonoBehaviour
{
    public bool landSelected;
    public bool turretSelected;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Land"))
        {
            landSelected = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Land"))
        {
            landSelected = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Turret"))
        {
            turretSelected = true;
        }
        else
        {
            turretSelected = false;
        }
    }
}
