using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ThirdPersonController : MonoBehaviour
    {
        public float walkSpeed = 2.0f;

        public float runSpeed = 4.0f;

        private float speed;

        public float turnSmooth = 0.1f;
        public float runSmooth = 0.3f;

        public Transform cam; // Main camera, not vcam

        private float turnSmoothVelocity;

        private CharacterController controller;

        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
        }

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            if (Input.GetButton("Fire1"))
            {
                speed = Mathf.SmoothStep(speed, runSpeed, runSmooth);
                controller.radius = 0.55f;
            }
            else
            {
                speed = Mathf.SmoothStep(speed, walkSpeed, runSmooth);
                controller.radius = 0.4f;
            }

            Vector3 direction = new Vector3(h * speed, 0f, v * speed);
            Vector3 clampedDir = Vector3.ClampMagnitude(direction, speed); // We don't want to move faster in diagonal directions

            if (clampedDir.magnitude >= 0.01) // Minimum movement threshold
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * clampedDir.magnitude * Time.deltaTime);
            }
            animator.SetFloat("Speed", clampedDir.magnitude / runSpeed);

            Debug.Log("Velocity: " + clampedDir.magnitude);
        }
    }
}