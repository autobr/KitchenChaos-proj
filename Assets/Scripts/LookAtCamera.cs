using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    // Update is called once per frame
    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                // NOTE: this one is "-Camera..." because it keeps the bar at Left to Right fill, and Mode.LookAt also has Left to Right fill.
                transform.forward = -Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = Camera.main.transform.forward;
                break;
        }
    }
}
