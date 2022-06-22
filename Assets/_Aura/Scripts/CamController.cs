using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void InitCamera(Transform lookAtValue,Transform followValue)
    {
        virtualCam.LookAt = lookAtValue;
        virtualCam.Follow = followValue;
    }

}
