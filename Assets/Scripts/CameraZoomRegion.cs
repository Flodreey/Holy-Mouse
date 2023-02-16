using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomRegion : MonoBehaviour
{
    [SerializeField] float cameraDistance;
    [SerializeField] float cameraHeight = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CameraController.SetCameraMaxDistance(false, cameraDistance, cameraHeight);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CameraController.SetCameraMaxDistance(true, 0, 0);
        }
    }
}
