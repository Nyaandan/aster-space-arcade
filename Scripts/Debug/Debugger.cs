using UnityEngine;
using UnityEngine.UIElements;

public class Debugger : MonoBehaviour
{
    public static Debugger Instance { get; private set; }
    [SerializeField] UIDocument debugUI;

    public UIDocument DebugUI => debugUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
