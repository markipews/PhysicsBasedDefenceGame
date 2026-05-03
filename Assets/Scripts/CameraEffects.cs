using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraEffects : MonoBehaviour
{
    Vector3 originalPosition;

    private Volume volume;
    private Vignette vignette;
    private float vignetteSpeed;

    private bool isShaking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.localPosition;
        
        volume = GameObject.FindGameObjectWithTag("VignetteController").GetComponent<Volume>();
        if (!volume.profile.TryGet(out vignette))
        {
            Debug.LogError("No Vignette found on " + gameObject.name);
        }
    }

    public void ShakeCamera(float duration, float severity, bool vertical, bool horizontal)
    {
        if (isShaking)
        {
            return;
        }
        StartCoroutine(Shake(duration, severity, vertical, horizontal));
    }

    private IEnumerator Shake(float duration, float severity, bool vertical, bool horizontal)
    {
        originalPosition = transform.localPosition;
        
        isShaking = true;

        while (duration > 0)
        {
            Vector3 shakeOffset = Vector3.zero;
            if (horizontal)
            {
                shakeOffset.x = Random.Range(-1f, 1f) * severity;
            }

            if (vertical)
            {
                shakeOffset.y = Random.Range(-1f, 1f) * severity;
            }
            
            transform.localPosition = originalPosition + shakeOffset;
            
            duration -= Time.deltaTime;
            
            yield return null;
        }
        transform.localPosition = originalPosition;
        isShaking = false;
    }

    void Update()
    {
        if (vignette.intensity.value > 0)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0, Time.deltaTime * vignetteSpeed);
        }
    }
    
    public void Vignette(float intensity, Color colour, float speed)
    {
        vignette.intensity.value = intensity;
        vignette.color.value = colour;
        vignetteSpeed = speed;
    }
    
}
