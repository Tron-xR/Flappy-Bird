using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject pipe;
    [SerializeField] private float range=5f;
    [SerializeField] private float maxTimer = 2f;

    private float timer;

    private void Update()
    {

        if (timer > maxTimer)
        {
            PipeSpawnBehaviour();
            timer = 0;

        }
        timer += Time.deltaTime;
    }

    

    private void PipeSpawnBehaviour()
    {
        Vector3 pipePos= transform.position+new Vector3 (0, Random.Range(-range, range),0);
        GameObject spawnedPipe= Instantiate(pipe,pipePos,Quaternion.identity);

        Object.Destroy(spawnedPipe, 5f);
    }
}
