using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class MouseHandler : MonoBehaviour
{
    public GameObject mouseCollider;
    
    public Vector2 mousePosFinal;
    
    public static MouseHandler Instance;
    
    GameObject selectedObject;

    void Awake()
    {
        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");
        Vector2 mouseDelta = new Vector2(mouseHorizontal, mouseVertical);

        if (mouseDelta.x  != 0 || mouseDelta.y != 0)
            OnMouseActive();
    }

    void OnMouseActive()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        mousePosFinal = Camera.main.ScreenToWorldPoint(mousePosition);
        
        mouseCollider.transform.position = mousePosFinal;
    }

}
