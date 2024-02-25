using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;
    private float currentShakeDuration = 0f;
    public static CameraShake instance;
    public static CameraShake Instance {  get { return instance; } }
    private void Awake()
    {
        if (instance !=null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.GetComponent<Transform>();
        }
        originalPos = cameraTransform.localPosition;
    }

    void FixedUpdate()
    {
        if (currentShakeDuration > 0)
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            currentShakeDuration -= Time.fixedDeltaTime * decreaseFactor;
        }
        else
        {
            currentShakeDuration = 0f;
            cameraTransform.localPosition = originalPos;
        }
    }

    public void Shake()
    {
        currentShakeDuration = shakeDuration;
    }
}
