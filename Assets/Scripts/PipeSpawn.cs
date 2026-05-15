using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject pipe;
    [SerializeField] private float verticalRange = 1.65f;
    [SerializeField] private float spawnInterval = 2.15f;
    [SerializeField] private float maxHeightChange = 0.9f;

    private float timer;
    private float lastSpawnY;

    private void Start()
    {
        lastSpawnY = transform.position.y;
        PipeSpawnBehaviour();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            PipeSpawnBehaviour();
            timer = 0;
        }
    }

    private void PipeSpawnBehaviour()
    {
        float targetY = Random.Range(-verticalRange, verticalRange);
        float spawnY = Mathf.Clamp(targetY, lastSpawnY - maxHeightChange, lastSpawnY + maxHeightChange);
        lastSpawnY = spawnY;

        Vector3 pipePos = transform.position + new Vector3(0, spawnY, 0);
        GameObject spawnedPipe = Instantiate(pipe, pipePos, Quaternion.identity);

        Object.Destroy(spawnedPipe, 8f);
    }
}
