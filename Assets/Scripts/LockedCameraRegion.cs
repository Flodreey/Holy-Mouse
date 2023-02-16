using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedCameraRegion : MonoBehaviour
{
    [SerializeField] Transform cameraPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            //CameraController.SetFixedPos(true, cameraPos.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //CameraController.SetFixedPos(false, cameraPos.position);
        }
    }
}
