using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ThirdPersonController : MonoBehaviour
    {
        public Transform mainCamera;

        [Header("Walk/Run Settings")]
        public float walkSpeed = 2.0f;
        public float runSpeed = 4.0f;
        public float turnSmooth = 0.1f;
        public float runSmooth = 0.3f;
        private float turnSmoothVelocity;
        private float speed;

        [Header("Roll Settings")]
        public float rollCooldown = 0.9f;

        [Header("Info")]
        private CharacterController controller;
        private Animator animator;

        [SerializeField]
        [ReadOnly]
        /* As long as cooldown is not equal to 0, player movements are disabled.*/
        private int cooldownCount = 0;

        public bool HasCooldown
        {
            get
            {
                return cooldownCount > 0;
            }
        }

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
            if (HasCooldown) return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(h * speed, 0f, v * speed);
            Vector3 clampedDir = Vector3.ClampMagnitude(direction, speed); // We don't want to move faster in diagonal directions
            if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Jump")) // Roll
            {
                controller.radius = 0.5f;
                animator.SetTrigger("Rolling");
                AddCooldown(rollCooldown);
            }
            else if (Input.GetButton("Fire1") || Input.GetButton("Fire3")) // Run
            {
                speed = Mathf.SmoothStep(speed, runSpeed, runSmooth);
                controller.radius = 0.55f;
            }
            else // Simple walk
            {
                speed = Mathf.SmoothStep(speed, walkSpeed, runSmooth);
                controller.radius = 0.4f;
            }

            if (clampedDir.magnitude >= 0.01) // Minimum movement threshold
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * clampedDir.magnitude * Time.deltaTime);
            }
            animator.SetFloat("Speed", clampedDir.magnitude / runSpeed);
        }
        public void AddCooldown(float duration)
        {
            StartCoroutine(_AddCooldown(duration));
        }

        private IEnumerator _AddCooldown(float duration)
        {
            cooldownCount++;

            yield return new WaitForSeconds(duration);

            cooldownCount--;
        }
    }
}