using UnityEngine;
[RequireComponent(typeof(Hazard))]

[RequireComponent(typeof(Rigidbody2D))]
public class Comet : MonoBehaviour
{
    public enum Pattern { Triangle = 3, Star = 5, Shower = 10 }
    Rigidbody2D rb;
    SpriteRenderer sr;
    Hazard hazard;
    ParticleSystem ps;

    [SerializeField] GameObject meteorPrefab;
    [SerializeField] Color[] colorRange;
    Pattern pattern;
    Transform target;

    Vector2 direction;
    float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        hazard = GetComponent<Hazard>();
    }

    void Update()
    {
        // Consider slowing down the comet as it approaches the target

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
        target.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CometTarget"))
        {
            // Handle collision with the player, e.g., apply damage or effects
            collision.gameObject.SetActive(false);
            TriggerPattern();
        }
    }

    public void Reinitialize(Transform target, float speed, Pattern pattern)
    {
        this.target = target;
        this.speed = speed;
        this.pattern = pattern;

        direction = (target.position - transform.position).normalized;

        sr.color = Color.Lerp(colorRange[0], colorRange[1], Random.Range(0f, 1f));
        var psMain = ps.main;
        psMain.startColor = new ParticleSystem.MinMaxGradient
        {
            mode = ParticleSystemGradientMode.TwoColors,
            colorMin = sr.color,
            colorMax = Color.Lerp(sr.color, Color.white, 0.95f)
        };

    }

    void TriggerPattern()
    {
        switch (pattern)
        {
            case Pattern.Triangle:
                for (int i = 0; i < 3; i++)
                {
                    SpawnMeteor((i - 1) * 120f);
                }
                break;

            case Pattern.Star:
                for (int i = 0; i < 5; i++)
                {
                    SpawnMeteor((i - 2) * 72f);
                }
                break;
                
            case Pattern.Shower:
                // TODO: Account for comet direction
                for(int i = 0; i < 4; i++)
                {
                    SpawnMeteor((i - 1.5f) * 30f);
                }
                break;
        }

        // Destroy the comet after triggering the pattern and spawing meteors
        gameObject.SetActive(false);
    }

    void SpawnMeteor(float angleOffset)
    {
        //GameObject meteor = Instantiate(meteorPrefab);
        GameObject meteor = PoolManager.Instance.GetPooledObject(ObjectType.METEOR);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;

        meteor.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, angle));
        meteor.SetActive(true);
        
        Vector2 mdirection = new(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        meteor.GetComponent<Meteor>().Reinitialize(mdirection, speed, sr.color);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y));
    }
}