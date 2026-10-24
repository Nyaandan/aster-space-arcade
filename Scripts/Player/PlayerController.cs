using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    LineRenderer lr;
    Vector2 point;
    Transform booster;
    Transform shield;
    [SerializeField] GameObject explosionFX;
    [SerializeField] float maxRotationSpeed;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float rotationAcceleration;
    float currentMoveSpeed = 0f;
    float currentRotationSpeed = 0f;
    float currentAngle = 90f;
    bool click;
    bool invincible;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        booster = transform.Find("Booster");
        shield = transform.Find("Shield");
    }

    private void OnEnable()
    {
        InputSystem.actions.FindAction("Point").performed += Point;
        InputSystem.actions.FindAction("Click").started += ClickStarted;
        InputSystem.actions.FindAction("Click").canceled += ClickStopped;
    }

    private void OnDisable()
    {
        InputSystem.actions.FindAction("Point").performed -= Point;
        InputSystem.actions.FindAction("Click").started -= ClickStarted;
        InputSystem.actions.FindAction("Click").canceled -= ClickStopped;
    }


    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        TriggerInvincibility();
        Invoke(nameof(DisableInvincibility), 3f);
        currentMoveSpeed = maxMoveSpeed;
        rb.linearVelocity = Vector2.up * currentMoveSpeed;
        currentAngle = 90f;
    }

    private void TriggerInvincibility()
    {
        invincible = true;
        shield.gameObject.SetActive(true);
    }

    private void DisableInvincibility()
    {
        invincible = false;
        shield.gameObject.SetActive(false);
    }

    void Update()
    {
        if (click)
        {
            // lr.SetPositions(new Vector3[] { transform.position, point });

            Move();
        }
        else if (currentMoveSpeed > 0f)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0f, acceleration * Time.deltaTime);
            if (rb.linearVelocity.magnitude > 0f)
                rb.linearVelocity *= (currentMoveSpeed / rb.linearVelocity.magnitude);
        }
    }

    private void Move()
    {
        currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, maxRotationSpeed, rotationAcceleration * Time.deltaTime);
        Vector2 targetDirection = (point - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, currentRotationSpeed * Time.deltaTime);
        float eulerAngle = (currentAngle < 90) ? currentAngle - 90f : currentAngle + 270f;
        transform.rotation = Quaternion.Euler(0f, 0f, eulerAngle);

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, transform.up * maxMoveSpeed, acceleration * Time.deltaTime);

        // currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, maxMoveSpeed, acceleration * Time.deltaTime);
        // rb.linearVelocity = transform.up * currentMoveSpeed;
    }

    void Point(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        point = Camera.main.ScreenToWorldPoint(value);
    }

    void ClickStarted(InputAction.CallbackContext context)
    {
        click = true;
        booster.gameObject.SetActive(true);
    }

    void ClickStopped(InputAction.CallbackContext context)
    {
        click = false;
        booster.gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        if (invincible)
            return;

        Instantiate(explosionFX, transform.position, Quaternion.identity);
        GameMaster.Instance.GameOver(false);
        Destroy(gameObject);
        // Handle collision with obstacle (e.g., reduce health, play sound, etc.)
    }

    private void OnDestroy()
    {
    }
}
