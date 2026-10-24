using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(ObstacleManagerLegacy))]
public class DebugObstacleManager : MonoBehaviour
{
    ObstacleManagerLegacy obstacleManager;
    Vector3 position1;
    Vector3 position2;
    [SerializeField] Transform[] markers;

    [Header("Asteroid")]
    [SerializeField] float asteroidSpeed = 14f;

    [Header("Comet")]
    [SerializeField] float cometSpeed = 14f;
    [SerializeField] Comet.Pattern cometPattern = Comet.Pattern.Star;

    [Header("Gamma Ray")]
    [SerializeField] float gammaRayDuration = 2f;
    [SerializeField] float gammaRayBurstDelay = 3f;


    void Awake()
    {
        obstacleManager = GetComponent<ObstacleManagerLegacy>();
    }

    void Start()
    {
        // Register the input action callback for click performed
        InputSystem.actions.FindAction("Click R").performed += Point;
        InputSystem.actions.FindAction("Click M").performed += Direct;
        // Point is left click, Direct is right click
        Button[] buttons = Debugger.Instance.DebugUI.rootVisualElement.Q<GroupBox>("DebugButtons").Query<Button>().Build().ToArray();
        buttons[0].clicked += () => SpawnAsteroid();
        buttons[1].clicked += () => SpawnComet();
        buttons[2].clicked += () => SpawnGammaRay();
    }

    void Point(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        position1 = Camera.main.ScreenToWorldPoint(mousePosition) + new Vector3(0, 0, 10);
        markers[0].position = position1;
    }

    void Direct(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        position2 = Camera.main.ScreenToWorldPoint(mousePosition) + new Vector3(0, 0, 10);
        markers[1].position = position2;
    }

    void SpawnAsteroid()
    {
        Vector2 marker = position1;
        Vector2 direction = (position2 - position1).normalized;
        Vector2 position = marker - direction * 22f + direction;
        Dictionary<string, float> parameters = new()
        {
            { "x", position.x },
            { "y", position.y },
            { "dx", direction.x },
            { "dy", direction.y },
            { "speed", asteroidSpeed }
        };
        obstacleManager.SpawnAsteroid(parameters);
    }

    void SpawnComet()
    {

        Vector2 marker = position1;
        Vector2 direction = (position2 - position1).normalized;
        Vector2 position = marker - direction * 22f + direction;
        Dictionary<string, float> parameters = new()
        {
            { "x", position.x },
            { "y", position.y },
            { "target_x", position2.x },
            { "target_y", position2.y },
            { "speed", cometSpeed },
            { "pattern", (float)cometPattern }
        };
        obstacleManager.SpawnComet(parameters);
    }

    void SpawnGammaRay()
    {
        Vector3 position = position1 + (position2 - position1) * 0.5f;

        Vector3 magVector = (position2.x > position1.x) ? position2 - position1 : position1 - position2;
        Vector3 direction = new(-magVector.y, magVector.x, 0f);
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        Dictionary<string, float> parameters = new()
        {
            { "x", position.x },
            { "y", position.y },
            { "rotation", rotation },
            { "magnitude", Vector3.Distance(position1, position2) },
            { "burstDelay", gammaRayBurstDelay },
            { "burstDuration", gammaRayDuration }
        };
        obstacleManager.SpawnGammaRay(parameters);
    }
}