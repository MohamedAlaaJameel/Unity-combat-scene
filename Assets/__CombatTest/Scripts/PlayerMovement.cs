using UnityEngine;
using Zenject;
using static GameInstaller;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Inject] SignalBus _signalBus;

    public Transform target;
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private CharacterController controller;
    private Transform cam;
    private Vector3 velocity;
    private bool isDashing;
    private float dashTimer;
    private float lastDashTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main?.transform;
    }

    void Update()
    {
        transform.LookAt(target);
        if (isDashing)
            HandleDash();
        else
            MovePlayer();

    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move;

        if (cam != null)
        {
            Vector3 forward = cam.forward;
            Vector3 right = cam.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();
            move = forward * z + right * x;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Dash trigger
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown && move.magnitude > 0.1f)
        {
            _signalBus.Fire<DashBTNSignal>();
            StartDash(move);
        }
    }

    void StartDash(Vector3 direction)
    {
        isDashing = true;
        dashTimer = dashDuration;
        velocity = direction.normalized * dashSpeed;
        lastDashTime = Time.time;
    }

    void HandleDash()
    {
        controller.Move(velocity * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
            isDashing = false;
    }

 
}
