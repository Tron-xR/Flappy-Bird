using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }

    private AudioSource effectsSource;
    private AudioSource musicSource;
    private AudioClip flapClip;
    private AudioClip scoreClip;
    private AudioClip gameOverClip;
    private AudioClip musicClip;

    public static GameAudio GetOrCreate()
    {
        if (Instance != null)
        {
            return Instance;
        }

        GameObject audioObject = new GameObject("Game Audio");
        return audioObject.AddComponent<GameAudio>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        effectsSource = gameObject.AddComponent<AudioSource>();
        effectsSource.playOnAwake = false;
        effectsSource.volume = 0.75f;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = 0.18f;

        flapClip = CreateToneClip("Flap", 620f, 0.08f, 0.45f);
        scoreClip = CreateTwoToneClip("Score", 880f, 1320f, 0.14f, 0.45f);
        gameOverClip = CreateTwoToneClip("GameOver", 220f, 110f, 0.35f, 0.55f);
        musicClip = CreateMusicLoop();
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayFlap()
    {
        effectsSource.PlayOneShot(flapClip);
    }

    public void PlayScore()
    {
        effectsSource.PlayOneShot(scoreClip);
    }

    public void PlayGameOver()
    {
        effectsSource.PlayOneShot(gameOverClip);
    }

    private static AudioClip CreateToneClip(string clipName, float frequency, float duration, float volume)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            float fade = 1f - ((float)i / sampleCount);
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * volume * fade;
        }

        AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip CreateTwoToneClip(string clipName, float firstFrequency, float secondFrequency, float duration, float volume)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            float progress = (float)i / sampleCount;
            float frequency = progress < 0.5f ? firstFrequency : secondFrequency;
            float fade = 1f - progress;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * volume * fade;
        }

        AudioClip clip = AudioClip.Create(clipName, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static AudioClip CreateMusicLoop()
    {
        const int sampleRate = 44100;
        const float duration = 2f;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];
        float[] notes = { 262f, 330f, 392f, 330f };

        for (int i = 0; i < sampleCount; i++)
        {
            float time = (float)i / sampleRate;
            int noteIndex = Mathf.FloorToInt(time / 0.5f) % notes.Length;
            float frequency = notes[noteIndex];
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * time) * 0.25f;
        }

        AudioClip clip = AudioClip.Create("MusicLoop", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
