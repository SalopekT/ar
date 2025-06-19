using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    Light light;
    [SerializeField] private ARCameraManager cameraManager;
    void Awake ()
    {
        light = GetComponent<Light>();
    }

    void OnEnable()
    {
        if (cameraManager != null)
            cameraManager.frameReceived += FrameChanged;
    }

    void OnDisable()
    {
        if (cameraManager != null)
            cameraManager.frameReceived -= FrameChanged;
    }

    void FrameChanged(ARCameraFrameEventArgs args)
    {
        ARLightEstimationData lightData = args.lightEstimation;
        if (lightData.mainLightColor.HasValue)
        {
            light.color = lightData.mainLightColor.Value;
        }

        if (lightData.averageMainLightBrightness.HasValue)
        {
            light.intensity = lightData.averageMainLightBrightness.Value * 2f;
        }
        if (lightData.mainLightDirection != null)
        {
            light.transform.rotation = Quaternion.LookRotation(lightData.mainLightDirection.Value);
        }
        
    }
}
