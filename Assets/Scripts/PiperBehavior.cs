using UnityEngine;

public class PiperBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
    
}
