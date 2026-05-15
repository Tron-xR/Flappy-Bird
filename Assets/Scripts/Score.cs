using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private const string HighScoreKey = "FlappyBird.HighScore";

    public static Score Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore;
    private int highScore;
    private TextMeshProUGUI scoreValueText;
    private TextMeshProUGUI bestValueText;
    private TextMeshProUGUI bestLabelText;
    private RectTransform scoreCard;
    private float punchTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void Start()
    {
        BuildHud();
        RefreshHud();
    }

    private void Update()
    {
        AnimateScoreCard();
    }

    public void UpdateScore()
    {
        AddScore(1);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        punchTimer = 0.18f;
        RefreshHud();
        GameAudio.GetOrCreate().PlayScore();
    }

    private void RefreshHud()
    {
        if (scoreValueText == null || bestValueText == null)
        {
            if (scoreText != null)
            {
                scoreText.text = currentScore.ToString();
            }

            return;
        }

        scoreValueText.text = currentScore.ToString();
        bestValueText.text = highScore.ToString();
    }

    private void BuildHud()
    {
        Canvas canvas = GetOrCreateCanvas();
        TMP_FontAsset font = scoreText != null ? scoreText.font : null;
        Material fontMaterial = scoreText != null ? scoreText.fontSharedMaterial : null;

        HideOldScoreUi();

        GameObject rootObject = new GameObject("Score HUD");
        rootObject.transform.SetParent(canvas.transform, false);

        RectTransform root = rootObject.AddComponent<RectTransform>();
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.offsetMin = Vector2.zero;
        root.offsetMax = Vector2.zero;

        scoreCard = CreatePanel(root, "Score Card", new Vector2(0f, 1f), new Vector2(82f, -44f), new Vector2(132f, 58f), new Color(0.05f, 0.07f, 0.09f, 0.78f));
        CreateLabel(scoreCard, "Score Label", "SCORE", font, fontMaterial, 11, new Color(1f, 0.86f, 0.32f, 1f), new Vector2(-35f, 13f), new Vector2(48f, 18f));
        scoreValueText = CreateLabel(scoreCard, "Score Value", "0", font, fontMaterial, 30, Color.white, new Vector2(32f, 9f), new Vector2(58f, 34f));

        bestLabelText = CreateLabel(scoreCard, "Best Label", "BEST", font, fontMaterial, 10, new Color(0.78f, 0.84f, 0.9f, 1f), new Vector2(-35f, -13f), new Vector2(48f, 16f));
        bestValueText = CreateLabel(scoreCard, "Best Value", "0", font, fontMaterial, 18, new Color(0.98f, 0.72f, 0.24f, 1f), new Vector2(31f, -15f), new Vector2(58f, 22f));
    }

    private void HideOldScoreUi()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }

        TextMeshProUGUI[] labels = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        foreach (TextMeshProUGUI label in labels)
        {
            if (label == scoreValueText || label == bestValueText || label == bestLabelText)
            {
                continue;
            }

            if (label.gameObject.name == "ScoreText" || label.text.Trim() == "Score")
            {
                label.gameObject.SetActive(false);
            }
        }
    }

    private Canvas GetOrCreateCanvas()
    {
        if (scoreText != null && scoreText.GetComponentInParent<Canvas>() != null)
        {
            return scoreText.GetComponentInParent<Canvas>();
        }

        Canvas existingCanvas = FindFirstObjectByType<Canvas>();
        if (existingCanvas != null)
        {
            return existingCanvas;
        }

        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private void AnimateScoreCard()
    {
        if (scoreCard == null)
        {
            return;
        }

        if (punchTimer <= 0f)
        {
            scoreCard.localScale = Vector3.one;
            return;
        }

        punchTimer -= Time.unscaledDeltaTime;
        float progress = punchTimer / 0.18f;
        float scale = 1f + Mathf.Sin(progress * Mathf.PI) * 0.12f;
        scoreCard.localScale = Vector3.one * scale;
    }

    private static RectTransform CreatePanel(Transform parent, string objectName, Vector2 anchor, Vector2 position, Vector2 size, Color color)
    {
        GameObject panelObject = new GameObject(objectName);
        panelObject.transform.SetParent(parent, false);

        RectTransform rectTransform = panelObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = anchor;
        rectTransform.anchorMax = anchor;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        Image image = panelObject.AddComponent<Image>();
        image.color = color;

        return rectTransform;
    }

    private static TextMeshProUGUI CreateLabel(RectTransform parent, string objectName, string text, TMP_FontAsset font, Material fontMaterial, int fontSize, Color color, Vector2 position, Vector2 size)
    {
        GameObject labelObject = new GameObject(objectName);
        labelObject.transform.SetParent(parent, false);

        RectTransform rectTransform = labelObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = size;

        TextMeshProUGUI label = labelObject.AddComponent<TextMeshProUGUI>();
        label.text = text;
        label.font = font;
        label.fontSharedMaterial = fontMaterial;
        label.fontSize = fontSize;
        label.fontSizeMin = 10;
        label.fontSizeMax = fontSize;
        label.enableAutoSizing = true;
        label.color = color;
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;

        Shadow shadow = labelObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
        shadow.effectDistance = new Vector2(1.4f, -1.4f);

        return label;
    }
}
