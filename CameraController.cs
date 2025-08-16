using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positioning")]
    [SerializeField] Vector3 CameraOffset;
    [SerializeField] Vector3 CameraRotation;
    Quaternion CamRotation;
    [SerializeField] float minVerticalRotationVal, maxVerticalRotationVal;

    [Header("Player")]
    [SerializeField] Transform followTarget;
    Vector3 LookAtPosition;

    [Header("Camera Config")]
    [SerializeField] float sens;


    [Header("Camera Reposition")]
    [SerializeField] float distance;
    [SerializeField] float minDistance = 0.5f;
    [SerializeField] float maxDistance = 2f;
    [SerializeField] float smoothing;
    float targetDistance;

    Vector3 zVec, zWorld;
    Vector3 yVec;
    Vector3 yzRotateVec;
    LayerMask GroundLayer;
    RaycastHit playerHit, zcamHit, ycamHit, yzcamHit;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CameraOffset = new Vector3(0f, -0.2f, 2f);

        #region Ray Blocks Setups

        targetDistance = maxDistance;

        zVec = -transform.forward;  // unit vector (0,0,1)

        yVec = -transform.up;  // unit vector (0,0,1)

        Vector3 yzVec = -transform.forward;
        Quaternion yzRot = Quaternion.AngleAxis(50f, Vector3.right);
        yzRotateVec = yzRot * yzVec;
        


        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        int ignoreLayer = LayerMask.NameToLayer("Ground");
        int layerMask = ~(1 << ignoreLayer); // bitwise NOT -> hit everything except this layer


        CameraRotation += InputManager._instance.GetMouseInput() * Time.deltaTime * sens;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, minVerticalRotationVal, maxVerticalRotationVal);
        CamRotation = Quaternion.Euler(CameraRotation);

        #region Camera Repositioning
        Vector3 CamDest = (transform.position - followTarget.position);
        bool playerBlocked = Physics.Raycast(followTarget.position, CamDest, out playerHit, maxDistance) ;
        bool cameraRayBlocks =(
            Physics.Raycast(transform.position, zVec, out zcamHit, maxDistance - playerHit.distance, layerMask) ||
            Physics.Raycast(transform.position, yVec, out ycamHit, 2f, layerMask) ||
            Physics.Raycast(transform.position, yzRotateVec, out yzcamHit, 2f, layerMask) 
            );

        Debug.DrawRay(followTarget.position, CamDest, Color.white, maxDistance);
        Debug.DrawRay(transform.position, zVec, Color.blue, maxDistance - playerHit.distance);
        Debug.DrawRay(transform.position, yVec, Color.green, 2f);
        Debug.DrawRay(transform.position, yzRotateVec, Color.red, 2f);

        

        if (playerBlocked || (cameraRayBlocks && playerBlocked))
        {
            targetDistance = minDistance;            
        }
        else if (!playerBlocked && cameraRayBlocks && targetDistance == minDistance)
        {
            targetDistance = minDistance;
        }
        else
        {
            targetDistance = maxDistance;
        }
            //distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * smoothing);
            CameraOffset.z = targetDistance;
        #endregion

    }

    private void LateUpdate()
    {
        LookAtPosition = followTarget.position - CamRotation * CameraOffset;
        this.transform.position = LookAtPosition;
        this.transform.rotation = CamRotation;
    }

    private void OnDraGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 zVec = -transform.forward * 1f;
        Gizmos.DrawRay(transform.position, zVec);

        Gizmos.color = Color.green;
        Vector3 yVec = -transform.up * 1f; 
        Gizmos.DrawRay(transform.position, yVec);


        Gizmos.color = Color.red;
        Vector3 yzVec = -transform.forward * 2f;
        Quaternion yZRot = Quaternion.AngleAxis(-50f, Vector3.right); // 45° around Y axis
        Vector3 yzRotatedVec = yZRot * yzVec;
        Gizmos.DrawRay(transform.position, yzRotatedVec);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(followTarget.position, (transform.position - followTarget.position));
    }
}
