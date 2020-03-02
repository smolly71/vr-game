using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    private Rigidbody rigid;
    public float speed;
    public float jumpHeight;
    public float SMASH;
    public float mousesens;

    public ParticleSystem respawnParticle;
    public GameObject verticalRotation;
    private float vertical = 0.0f;
    private Vector3 spawnPoint;

    private bool isGrounded = true;

    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
           Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            var locVel = transform.InverseTransformDirection(rigid.velocity);
            locVel = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Jump") != 0 && isGrounded? Input.GetAxis("Jump") * jumpHeight : locVel.y, Input.GetAxis("Vertical") * speed);
            rigid.velocity = transform.TransformDirection(locVel);

            float horizontal = Input.GetAxis("Mouse X") * mousesens;
            vertical -= Input.GetAxis("Mouse Y") * mousesens;
            vertical = Mathf.Clamp(vertical, -80, 80);

            if(Input.GetKey(KeyCode.LeftShift)) rigid.AddForce(new Vector3(0,-SMASH,0));

            transform.Rotate(0, horizontal, 0);
            verticalRotation.transform.localRotation = Quaternion.Euler(vertical, 0, 0);
        }

        if (transform.position.y < -10) respawn();
    }
    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void respawn()
    {
        transform.position = spawnPoint;
        Instantiate(respawnParticle, transform.position, new Quaternion()).Play();
    }
}
