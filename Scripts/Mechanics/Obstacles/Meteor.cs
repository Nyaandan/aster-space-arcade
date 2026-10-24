using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Hazard))]
public class Meteor : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Hazard hazard;
    ParticleSystem ps;
    [SerializeField] GameObject ExplosionPrefab;
    
    Vector2 direction;
    float speed;
    float speedMultiplier = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        hazard = GetComponent<Hazard>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 movement = speed * Time.deltaTime * direction;
        float distance = movement.magnitude;

        int contacts = rb.Cast(direction, hazard.ContactFilter, hazard.hits, distance);

        for (int i = 0; i < contacts; i++)
        {
            print(hazard.hits[i].collider.name);
            HandleCollision(hazard.hits[i].collider);
        }

        transform.Translate(movement, Space.World);
    }

    void HandleCollision(Collider2D collider)
    {
        // Handle collision with the player, e.g., apply damage or effects
        collider.GetComponentInParent<PlayerController>().TakeDamage(); // Might change when the assets are ready
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void Reinitialize(Vector2 dir, float spd, Color col)
    {
        direction = dir;
        speed = spd;
        sr.color = col;

        var psMain = ps.main;
        psMain.startColor = new ParticleSystem.MinMaxGradient
        {
            mode = ParticleSystemGradientMode.TwoColors,
            colorMin = sr.color,
            colorMax = Color.Lerp(sr.color, Color.white, 0.95f)
        };
    }
}