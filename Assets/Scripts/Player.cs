using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider hitBox;
    public int damage = 50;
    public float speed = 5f;
    public float jumpSpeed = 5f;
    public float gravity = 20f;
    public CharacterController controller;
    // Attribute
    [Tooltip("Duration hitbox is enabled (in seconds)")] 
    public float hitDuration = 1f;
    public float hitDelay = 2f;

    private bool isAllowedToHit = true;
    private Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(inputH, 0, inputV);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);


        // Check if space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isAllowedToHit)
            {
                // Run hit sequence
                StartCoroutine(Hit());
                StartCoroutine(HitDelay());
            }
        }
    }

    IEnumerator HitDelay()
    {
        isAllowedToHit = false;
        yield return new WaitForSeconds(hitDelay);
        isAllowedToHit = true;
    }

    IEnumerator Hit()
    {
        hitBox.enabled = true;
        yield return new WaitForSeconds(hitDuration);
        hitBox.enabled = false;
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Detect enemy
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Deal damage
            enemy.DealDamage(damage);
        }
    }
}
