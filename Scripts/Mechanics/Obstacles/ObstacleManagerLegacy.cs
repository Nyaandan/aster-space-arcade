using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

// The Orchestra
public class ObstacleManagerLegacy : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs;

    private void OnEnable() => SequencePlayer.OnAttackTriggered += HandleEvent;
    private void OnDisable() => SequencePlayer.OnAttackTriggered -= HandleEvent;

    private void HandleEvent(TimelineEvent attackEvent)
    {
        switch (attackEvent.id)
        {
            // TODO: Add the action hazard events here.
            case "Asteroid":
                SpawnAsteroid(attackEvent.parameters);
                break;

            case "Meteors":
                SpawnComet(attackEvent.parameters);
                break;

            case "Gamma Ray":
                SpawnGammaRay(attackEvent.parameters);
                break;

            default:
                Debug.LogWarning($"Unknown attack ID: {attackEvent.id}");
                break;
        }
    }

    /// <summary>
    /// Spawns an asteroid obstacle based on the provided parameters.
    /// The list of parameters should include:
    /// - "x": The x-coordinate of the spawn position.
    /// - "y": The y-coordinate of the spawn position.
    /// - "dx": The x-component of the direction vector.
    /// - "dy": The y-component of the direction vector.
    /// - "speed": The speed of the asteroid.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the asteroid.</param>
    public void SpawnAsteroid(Dictionary<string, float> parameters)
    {
        Vector2 position = new(parameters["x"], parameters["y"]);
        Vector2 direction = new Vector2(parameters["dx"], parameters["dy"]).normalized;
        float speed = parameters["speed"];

        GameObject asteroid = Instantiate(obstaclePrefabs[0], position, Quaternion.identity);
        asteroid.GetComponent<Asteroid>().Reinitilialize(direction, speed);
    }

    /// <summary>
    /// Spawns a comet obstacle based on the provided parameters.
    /// The list of parameters should include:
    /// - "x": The x-coordinate of the spawn position.
    /// - "y": The y-coordinate of the spawn position.
    /// - "target_x": The x-coordinate of the target position.
    /// - "target_y": The y-coordinate of the target position.
    /// - "speed": The speed of the comet.
    /// - "pattern": The pattern of the comet.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the comet.</param>
    public void SpawnComet(Dictionary<string, float> parameters)
    {
        Vector2 position = new(parameters["x"], parameters["y"]);
        Vector2 target = new(parameters["target_x"], parameters["target_y"]);
        float speed = parameters["speed"];
        Comet.Pattern pattern = (Comet.Pattern)System.Enum.Parse(typeof(Comet.Pattern), parameters["pattern"].ToString());

        GameObject targetObject = Instantiate(obstaclePrefabs[2]);
        targetObject.transform.position = target;

        // Calculate the comet and target rotation based on direction.
        // Might need to adjust/remove when the sprites are ready.
        Vector2 direction = (target - position).normalized;
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        if (angleRad < 0) angleRad += 2 * Mathf.PI;
        float angle = angleRad * Mathf.Rad2Deg - 90;
        targetObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        GameObject comet = Instantiate(obstaclePrefabs[1], position, Quaternion.Euler(0, 0, angle));
        comet.GetComponent<Comet>().Reinitialize(targetObject.transform, speed, pattern);
    }

    /// <summary>
    /// Spawns a gamma ray obstacle based on the provided parameters.
    /// The list of parameters should include:
    /// - "x": The x-coordinate of the spawn position.
    /// - "y": The y-coordinate of the spawn position.
    /// - "rotation": The rotation of the gamma ray.
    /// - "magnitude": The magnitude of the gamma ray.
    /// - "burstDelay": The delay before the gamma ray bursts.
    /// - "burstDuration": The duration of the gamma ray bursts.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the gamma ray.</param>
    public void SpawnGammaRay(Dictionary<string, float> parameters)
    {
        Vector2 position = new(parameters["x"], parameters["y"]);
        float rotation = parameters["rotation"];
        float magnitude = parameters["magnitude"];
        float burstDelay = parameters["burstDelay"];
        float burstDuration = parameters["burstDuration"];

        GameObject gammaRay = Instantiate(obstaclePrefabs[3], position, Quaternion.Euler(0, 0, rotation));
        gammaRay.GetComponent<GammaRay>().Reinitialize(magnitude, burstDelay, burstDuration);
    }
}
