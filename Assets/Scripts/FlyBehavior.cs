using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float rotationAngle = 10f;
    [SerializeField] private float speed = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.linearVelocity = Vector2.up * speed;
        }
        transform.rotation = Quaternion.Euler(0, 0, rb.linearVelocity.y * rotationAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();
    }
    
}
