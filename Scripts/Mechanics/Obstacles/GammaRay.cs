using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Hazard))]
public class GammaRay : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D collider;
    ParticleSystem ps0, ps1;
    Hazard hazard;

    float spawnTime;
    float burstDelay;
    float burstDuration;
    [SerializeField] Color telegraphColor;
    [SerializeField] Color burstColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        ParticleSystem[] psArray = GetComponentsInChildren<ParticleSystem>();
        ps0 = psArray[0];
        ps1 = psArray[1];
        collider = GetComponent<BoxCollider2D>();
        hazard = GetComponent<Hazard>();
    }

    void Start()
    {
        spawnTime = Time.time;
        StartCoroutine(TriggerBurst());
    }

    private void OnEnable()
    {
        collider.enabled = false;
        //sr.color = telegraphColor;
    }

    IEnumerator TriggerBurst()
    {
        float lifetime = 0;
        while (lifetime < burstDelay)
        {
            //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.PingPong(lifetime * 2, 1));
            yield return null;
            lifetime += Time.deltaTime;
        }

        //sr.color = burstColor;
        ps0.Stop();
        collider.enabled = true;
        ps1.Play();

        while (lifetime < burstDelay + burstDuration)
        {
            int contacts = rb.Overlap(hazard.ContactFilter, hazard.hitColliders);
            for (int i = 0; i < contacts; i++)
            {
                print(hazard.hitColliders[i].name);
                HandleCollision(hazard.hitColliders[i]);
            }
            yield return null;
            lifetime += Time.deltaTime;
        }

        // Fade out or disable the burst effect here
        yield return new WaitWhile(() => ps1.isPlaying);
        gameObject.SetActive(false);
    }

    void HandleCollision(Collider2D collider)
    {
        // Handle collision with the player, e.g., apply damage or effects
        collider.GetComponentInParent<PlayerController>().TakeDamage(); // Might change when the assets are ready
    }

    public void Reinitialize(float halfMagnitude, float burstDelay, float burstDuration)
    {
        collider.size = new(halfMagnitude * 2, 32);

        var psMain = ps1.main;
        psMain.duration = burstDuration - psMain.startLifetime.constant;
        var psShape = ps0.shape;
        psShape.scale = new(collider.size.x, collider.size.y, 1);
        psShape = ps1.shape;
        psShape.scale = new(collider.size.x, collider.size.y, 1);
        /*psMain.startColor = new ParticleSystem.MinMaxGradient
        {
            mode = ParticleSystemGradientMode.TwoColors,
            colorMin = sr.color,
            colorMax = Color.Lerp(sr.color, Color.white, 0.95f)
        };*/

        // sr.size = new Vector2(magnitude, sr.size.y);
        // collider.size = new Vector2(magnitude, collider.size.y);

        this.burstDuration = burstDuration;
        this.burstDelay = burstDelay;
    }
}