using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (PlayerController.instance != null)
        {
            vcam.Follow = PlayerController.instance.transform;
            vcam.LookAt = PlayerController.instance.transform;
        }
    }
}
