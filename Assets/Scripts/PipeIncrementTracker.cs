using UnityEngine;

public class PipeIncrementTracker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Score.Instance.UpdateScore();
            Debug.Log("Score Updated: ");
        }
    }
}
