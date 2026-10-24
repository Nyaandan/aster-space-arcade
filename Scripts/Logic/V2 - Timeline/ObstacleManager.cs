using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Assets.Scripts.Logic.V2___Timeline;
/////////////////////
//
//  THE ORCHESTRA
// 
/////////////////////
public class ObstacleManager : MonoBehaviour, INotificationReceiver
{
    [SerializeField] GameObject[] obstaclePrefabs;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is SpawnMarker marker) HandleEvent(marker);
    }

    private void HandleEvent(SpawnMarker marker)
    {
        switch (marker)
        {
            case AsteroidSpawnMarker asteroid:
                SpawnAsteroid(asteroid.GetPrefab(),
                    asteroid.position, asteroid.direction, asteroid.speed);
                break;
            case CometSpawnMarker comet:
                SpawnComet(comet.GetPrefab(), comet.GetTargetPrefab(),
                    comet.position, comet.target, comet.speed, comet.pattern);
                break;
            case GammaRaySpawnMarker gamma:
                SpawnGammaRay(gamma.GetPrefab(),
                    gamma.distanceToCenter, gamma.orientation, gamma.magnitude,
                    gamma.burstDelay, gamma.burstDuration);
                break;
            case EndOfLevelMarker:
                EndLevel();
                break;
            default:
                Debug.LogWarning($"Unknown event: {marker.id}");
                break;
        }
    }

    /// <summary>
    /// Spawns an asteroid obstacle based on the provided parameters.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the asteroid.</param>
    public void SpawnAsteroid(GameObject prefab,
            Vector2 position, Vector2 direction, float speed)
    {
        // GameObject asteroid = Instantiate(prefab, position, Quaternion.identity);

        GameObject asteroid = PoolManager.Instance.GetPooledObject(ObjectType.ASTEROID);
        asteroid.transform.SetPositionAndRotation(position, Quaternion.identity);
        asteroid.SetActive(true);
        asteroid.GetComponent<Asteroid>().Reinitilialize(direction.normalized, speed);
    }

    /// <summary>
    /// Spawns a comet obstacle based on the provided parameters.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the comet.</param>
    public void SpawnComet(GameObject cometPrefab, GameObject targetPrefab,
        Vector2 position, Vector2 target, float speed, Comet.Pattern pattern)
    {
        //GameObject targetObject = Instantiate(targetPrefab);
        GameObject targetObject = PoolManager.Instance.GetPooledObject(ObjectType.COMET_TARGET);

        // Calculate the comet and target rotation based on direction.
        // Might need to adjust/remove when the sprites are ready.
        Vector2 direction = (target - position).normalized;
        float angleRad = Mathf.Atan2(direction.y, direction.x);
        if (angleRad < 0) angleRad += 2 * Mathf.PI;
        float angle = angleRad * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        targetObject.transform.SetPositionAndRotation(target, rotation);
        targetObject.SetActive(true);

        //GameObject comet = Instantiate(cometPrefab, position, rotation);
        GameObject comet = PoolManager.Instance.GetPooledObject(ObjectType.COMET);
        comet.transform.SetPositionAndRotation(position, rotation);
        comet.SetActive(true);
        comet.GetComponent<Comet>().Reinitialize(targetObject.transform, speed, pattern);
    }

    /// <summary>
    /// Spawns a gamma ray obstacle based on the provided parameters.
    /// The list of parameters should include:
    /// - "x": The x-coordinate of the spawn position.
    /// - "y": The y-coordinate of the spawn position.
    /// - "rotation": The rotation of the gamma ray.
    /// - "halfMagnitude": The half-magnitude of the gamma ray.
    /// - "burstDelay": The delay before the gamma ray bursts.
    /// - "burstDuration": The duration of the gamma ray bursts.
    /// </summary>
    /// <param name="parameters">The parameters for spawning the gamma ray.</param>
    public void SpawnGammaRay(GameObject prefab, float distance,
        float orientation, float halfMagnitude, float burstDelay, float burstDuration)
    {
        //GameObject gammaRay = Instantiate(prefab, position, Quaternion.Euler(0, 0, rotation));
        GameObject gammaRay = PoolManager.Instance.GetPooledObject(ObjectType.GAMMA_RAY);
        gammaRay.transform.SetLocalPositionAndRotation(
            new(orientation < 90 ? distance : -distance, 0, 0),
            Quaternion.Euler(0, 0, orientation));
        gammaRay.SetActive(true);

        gammaRay.GetComponent<GammaRay>().Reinitialize(halfMagnitude, burstDelay, burstDuration);
    }

    public void EndLevel()
    {
        GameMaster.Instance.GameOver(true);
    }
}
