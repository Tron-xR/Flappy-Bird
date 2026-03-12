using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;

    [SerializeField] private TextMeshProUGUI scoreText;

    int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
     
    }

    private void Start()
    {
        scoreText.text = score.ToString();
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
