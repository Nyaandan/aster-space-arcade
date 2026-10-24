using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] float qty;

    void Start()
    {
        float maxScale = 3.5f;
        Vector2 boundingBoxSize = obstaclePrefab.GetComponent<SpriteRenderer>().bounds.size * maxScale;
        float cameraSize = Camera.main.orthographicSize;
        float spawnAreaHalfHeight = cameraSize - boundingBoxSize.y / 2;
        float spawnAreaHalfWidth = cameraSize * Camera.main.aspect - boundingBoxSize.x / 2;

        
        for (int i = 0; i < qty; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.SetParent(transform);

            float scale = Random.Range(1, maxScale);
            obstacle.transform.localScale = new Vector3(scale, scale, 1);
            float spawnX = Random.Range(-spawnAreaHalfWidth, spawnAreaHalfWidth);
            float spawnY = Random.Range(-spawnAreaHalfHeight, spawnAreaHalfHeight);
            float speed = 8f / Mathf.Sqrt(scale);

            /*obstacle.GetComponent<Obstacle>().SetProperties(
                new Vector3(spawnX, spawnY, 0),
                Random.Range(0f, 360f),
                Random.insideUnitCircle * speed);*/
        }
    }

    void Update()
    {
        
    }
}
