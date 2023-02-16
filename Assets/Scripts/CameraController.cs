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
    [SerializeField] Camera camera;
    int activeZoomRegions = 0;
    float maxCameraDistance;
    float focusObjectCurrentDistance;
    //bool locked;
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
        speedXYRatio = freeLook.m_XAxis.m_MaxSpeed / freeLook.m_YAxis.m_MaxSpeed;

        focusObjectCurrentDistance = focusObjectMaxDistance;
        maxCameraDistance = freeLook.m_Orbits[0].m_Radius;
        for (int i = 0; i < freeLook.m_Orbits.Length; i++)
        {
            freeLook.m_Orbits[i].m_Radius = maxCameraDistance;
        }

        SetCameraSpeed(cameraSpeed);
    }

    void SetCameraSpeed(float value)
    {
        if (freeLook == null) return;
        freeLook.m_XAxis.m_MaxSpeed = speedXYRatio * value;
        freeLook.m_YAxis.m_MaxSpeed = value;
    }

    public static void SetMouseSensitivity(float value)
    {
        cameraSpeed = value * maxCameraSpeed;
        if (instance == null) return;
        instance.SetCameraSpeed(cameraSpeed);
    }

    public static void SetCameraMaxDistance(bool reset, float distance, float height)
    {
        if (instance == null) return;
        instance.applyCameraMaxDistance(reset, distance, height);
    }

    void applyCameraMaxDistance(bool reset, float distance, float height)
    {
        activeZoomRegions += reset ? -1 : 1;
        float newDistance = activeZoomRegions == 0 ? maxCameraDistance : reset ? freeLook.m_Orbits[0].m_Radius : distance;
        for (int i = 0; i < freeLook.m_Orbits.Length; i++)
        {
            freeLook.m_Orbits[i].m_Radius = newDistance;
        }
        focusObjectCurrentDistance = activeZoomRegions == 0 ? focusObjectMaxDistance : height;
    }

    /* Requires a patch similar to the Grim Fandango Remaster
    public static void SetFixedPos(bool setLock, Vector3 pos)
    {
        if (instance == null) return;
        instance.ApplyFixedPos(setLock, pos);
    }

    void ApplyFixedPos(bool setLock, Vector3 pos)
    {
        locked = setLock;
        freeLook.enabled = !locked;
        SetCameraSpeed(locked ? 0 : cameraSpeed);
        freeLook.transform.position = pos;
        camera.transform.position = pos;
    }
    */
    
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, focusObjectCurrentDistance, layerMask);

        if (hit)
        {
            focusPointTarget = hitInfo.point;
        } else
        {
            focusPointTarget = transform.position + Vector3.up * focusObjectCurrentDistance;
        }

        float focusHeight = Mathf.Lerp(focusObject.transform.position.y, focusPointTarget.y, Time.deltaTime * focusObjectMoveSpeed);
        focusObject.transform.position = new Vector3(transform.position.x, focusHeight, transform.position.z);

        /*
        if (locked)
        {
            camera.transform.LookAt(focusObject.transform.position);
        }
        */
    }
}
