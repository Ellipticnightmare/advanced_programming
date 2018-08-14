using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cam;

    public GameObject weapon;

    public float speed = 5f;
    public float jumpSpeed = 5f;
    public float gravity = 20f;
    private Vector3 moveDirection = Vector3.zero;

    public CharacterController charC;
    
    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        if (charC.isGrounded)
        {
            Vector3 euler = cam.transform.eulerAngles;
            transform.rotation = Quaternion.AngleAxis(euler.y, Vector3.up);

            moveDirection = new Vector3(inputH, 0, inputV);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        charC.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            weapon.SetActive(true);
        }
    }
}
