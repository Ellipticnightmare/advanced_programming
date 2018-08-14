using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public Transform target;
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

    private float distance = 5.0f;
    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        transform.SetParent(null);
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
                if(Physics.Raycast(camRay, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                {

                }
            }
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
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scrollWheel * 5, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit, ~ignoreLayers))
            {
                distance -= hit.distance;
            }
            else
            {
                distance = maxDistance;
            }

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.position = position;

            Vector3 euler = transform.eulerAngles;
            target.rotation = Quaternion.AngleAxis(euler.y, Vector3.up);
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