using UnityEngine;

public class Hazard : MonoBehaviour
{
    bool appeared;
    public readonly RaycastHit2D[] hits = new RaycastHit2D[5];
    public readonly Collider2D[] hitColliders = new Collider2D[5];
    ContactFilter2D contactFilter;

    public ContactFilter2D ContactFilter { get => contactFilter; private set => contactFilter = value; }

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        contactFilter.useTriggers = true;
    }

    void Start()
    {
        appeared = false;
    }

    void Update()
    {
        
    }

    void OnBecameVisible()
    {
        appeared = true;
    }

    void OnBecameInvisible()
    {
        if (appeared)
        {
            gameObject.SetActive(false);
        }
    }
}
