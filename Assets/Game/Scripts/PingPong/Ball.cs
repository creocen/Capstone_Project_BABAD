using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 4f;
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float overlapPushDistance = 0.05f;

    private float currentSpeed;
    private bool hardModeActive;
    private float hardModeMultiplier = 1f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(LaunchBall), 1f);
    }

    public void LaunchBall()
    {
        Vector2 direction = Vector2.zero;
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            direction = new Vector2(-1f, 0f);
        }
        else
        {
            direction = new Vector2(1f, 0f);
        }

        rb.AddForce(direction.normalized * currentSpeed, ForceMode2D.Impulse);
        Invoke(nameof(EnableVerticalVariance), 0.3f);
    }

    private void EnableVerticalVariance()
    {
        Vector2 lv = rb.linearVelocity;
        if (lv.magnitude == 0f)
        {
            return;
        }

        float y = Random.Range(-0.5f, 0.5f);
        rb.linearVelocity = new Vector2(lv.x, y).normalized * currentSpeed;
    }

    public void EnableHardMode(float multiplier)
    {
        hardModeActive = true;
        hardModeMultiplier = multiplier;
        IncreaseSpeed(multiplier);
    }

    public void IncreaseSpeed(float multiplier)
    {
        currentSpeed = currentSpeed * multiplier;

        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        Vector2 lv = rb.linearVelocity;
        if (lv.sqrMagnitude > 0f)
        {
            rb.linearVelocity = lv.normalized * currentSpeed;
        }
    }

    private void FixedUpdate()
    {
        Vector2 lv = rb.linearVelocity;
        float mag = lv.magnitude;
        if (mag == 0f)
        {
            return;
        }

        Vector2 direction = lv / mag;
        float targetSpeed = currentSpeed;

        if (targetSpeed > maxSpeed)
        {
            targetSpeed = maxSpeed;
        }

        rb.linearVelocity = direction * targetSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Paddle"))
        {
            return;
        }

        float hitFactor = (transform.position.y - collision.transform.position.y) /
                          collision.collider.bounds.size.y;

        Vector2 current = rb.linearVelocity;
        float speed = current.magnitude;

        float directionX = -Mathf.Sign(current.x);
        Vector2 direction = new Vector2(directionX, hitFactor).normalized;

        rb.linearVelocity = direction * speed;
        transform.position = transform.position + (Vector3)(direction * overlapPushDistance);
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;

        if (hardModeActive)
        {
            currentSpeed = baseSpeed * hardModeMultiplier;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        Invoke(nameof(LaunchBall), 0.6f);
    }
}
