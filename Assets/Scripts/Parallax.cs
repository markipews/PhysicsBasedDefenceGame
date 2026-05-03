using UnityEngine;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private GameObject land;
    private Parallax scriptOfParallax;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        land = GameObject.Find("Land");
        GameObject parallax = Instantiate(land, Vector3.zero, Quaternion.identity, transform);
        scriptOfParallax = parallax.GetComponent<Parallax>();
        scriptOfParallax.enabled = false;
        CameraHandler cameraHandler = parallax.AddComponent<CameraHandler>();

        cameraHandler.cameraFollowSensitivity = 50000;
        cameraHandler.cameraShaker = GameObject.Find("CameraShaker");
        
        parallax.GetComponent<TilemapRenderer>().sortingOrder = -4;
        parallax.GetComponent<Tilemap>().color = new Color32(50,50,50,255);
        parallax.tag = "Untagged";
    }
}
