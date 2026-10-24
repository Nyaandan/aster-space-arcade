using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(Hazard))]
public class Asteroid : MonoBehaviour
{
    Rigidbody2D rb;
    Hazard hazard;
    [SerializeField] GameObject ExplosionPrefab;

    Vector2 direction;
    float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hazard = GetComponent<Hazard>();
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

        transform.Translate(movement);
    }

    void HandleCollision(Collider2D collider)
    {
        // Handle collision with the player, e.g., apply damage or effects
        collider.GetComponentInParent<PlayerController>().TakeDamage(); // Might change when the assets are ready
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void Reinitilialize(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;

        foreach (Transform shard in transform.GetChild(0)) {
            shard.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
        }

        foreach (Transform crater in transform.GetChild(1)) {
            float x = Random.Range(-.3f, .3f), y = Random.Range(-.3f, .3f),
                scaleX = Random.Range(.1f, .2f), scaleY = Random.Range(.1f, .2f);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
            crater.SetLocalPositionAndRotation(new(x, y, 0), rotation);
            crater.localScale = new(scaleX, scaleY, 1);
        }
    }
}