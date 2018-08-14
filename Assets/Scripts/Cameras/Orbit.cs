using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public bool hideCursor = true;
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public LayerMask ignoreLayers;
    public float maxDistance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    [Header("Collision")]
    public bool cameraCollision = false;
    public float rayDistance = 1000f;
    public LayerMask ignoreLayer;

    private Vector3 originalOffset;
    private float distance = 5.0f;
    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        originalOffset = transform.position - target.position;
        rayDistance = originalOffset.magnitude;

        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        transform.SetParent(null);
    }

    void OnDrawGizmosSelected()
    {

    }

    private void FixedUpdate()
    {
        if (target)
        {
            if (cameraCollision)
            {
                //Create a Ray that goes backwards from target
                Ray camRay = new Ray(target.position, -transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(camRay, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                {
                    distance = hit.distance;
                    return;
                }
            }

            //Reset distance if not hitting
            distance = originalOffset.magnitude;
        }
    }

    public void Look(float mouseX, float mouseY)
    {
        x += mouseX * xSpeed * Time.deltaTime;
        y -= mouseY * ySpeed * Time.deltaTime;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;

    }

    void LateUpdate()
    {
        if (target)
        {
            //Convert from world to local
            Vector3 localOffset = transform.TransformDirection(offset);
            transform.position = (target.position + offset) + -transform.forward * distance;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}