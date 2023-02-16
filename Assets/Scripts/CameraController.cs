using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static float cameraSpeed = 15f;

    [SerializeField] static float maxCameraSpeed = 30f;
    [SerializeField] float focusObjectMoveSpeed = 2f;
    [SerializeField] float focusObjectMaxDistance = .5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] GameObject focusObject;
    Vector3 focusPointTarget;
    static CameraController instance;

    static float speedXYRatio;
    Cinemachine.CinemachineFreeLook freeLook;

    void Start()
    {
        instance = this;

        freeLook = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
        freeLook.LookAt = focusObject.transform;
        freeLook.Follow = focusObject.transform;
        //cameraSpeed = freeLook.m_YAxis.m_MaxSpeed;
        speedXYRatio = freeLook.m_XAxis.m_MaxSpeed / freeLook.m_YAxis.m_MaxSpeed;

        SetCameraSpeed(cameraSpeed);
    }

    void SetCameraSpeed(float value)
    {
        //cameraSpeed = value * maxCameraSpeed;
        if (freeLook == null) return;
        freeLook.m_XAxis.m_MaxSpeed = speedXYRatio * cameraSpeed;
        freeLook.m_YAxis.m_MaxSpeed = cameraSpeed;
    }

    public static void SetMouseSensitivity(float value)
    {
        cameraSpeed = value * maxCameraSpeed;
        if (instance == null) return;
        instance.SetCameraSpeed(value);
    }
    
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, focusObjectMaxDistance, layerMask);

        if (hit)
        {
            focusPointTarget = hitInfo.point;
        } else
        {
            focusPointTarget = transform.position + Vector3.up * focusObjectMaxDistance;
        }

        float focusHeight = Mathf.Lerp(focusObject.transform.position.y, focusPointTarget.y, Time.deltaTime * focusObjectMoveSpeed);
        focusObject.transform.position = new Vector3(transform.position.x, focusHeight, transform.position.z);
    }
}
