using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject gameOverCanvas;
    public static GameManager Instance;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1f;
        isGameOver = false;
        GameAudio.GetOrCreate().PlayMusic();
        ParallaxBackground.SetupScene();
    }

    
    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        gameOverCanvas.SetActive(true);
        GameAudio.GetOrCreate().StopMusic();
        GameAudio.GetOrCreate().PlayGameOver();
        Time.timeScale = 0f;
        Debug.Log("function is working");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
