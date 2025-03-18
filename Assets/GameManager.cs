using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int strikes = 0;
    public int maxStrikes = 3;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI strikesText;
    public GameObject gameOverScreen;

    private bool gameStarted = false;
    private bool isGameOver = false;
    private bool canTakeStrike = true;
    private Coroutine scoreCoroutine;

    // Background Music
    private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;

    // Power-Up System
    public GameObject rainbowPowerUpPrefab;

    // Color System
    public Color[] colorArray = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.cyan };

    private void Start()
    {
        score = 0;
        strikes = 0;
        isGameOver = false;
        gameStarted = false;

        UpdateScoreUI();
        UpdateStrikesUI();

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        Invoke("EnableStrikeChecking", 2f);

        // Initialize background music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; // Adjust as needed
        audioSource.Play();

        // Start Power-Up Spawning
        InvokeRepeating("SpawnRainbowPowerUp", 10f, 20f);
    }

    private void EnableStrikeChecking()
    {
        gameStarted = true;
        canTakeStrike = true;
        scoreCoroutine = StartCoroutine(IncrementScoreOverTime());
    }

    private IEnumerator IncrementScoreOverTime()
    {
        while (!isGameOver)
        {
            score++;
            UpdateScoreUI();
            yield return new WaitForSeconds(1f);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    public void AddStrike()
    {
        if (isGameOver || !gameStarted || !canTakeStrike) return;

        canTakeStrike = false;
        strikes++;
        Debug.Log("Strike Counted: " + strikes);
        UpdateStrikesUI();

        if (strikes >= maxStrikes)
        {
            GameOver();
        }
        else
        {
            Invoke("ResetStrikeCooldown", 1f);
        }
    }

    private void ResetStrikeCooldown()
    {
        canTakeStrike = true;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateStrikesUI()
    {
        if (strikesText != null)
            strikesText.text = "Strikes: " + strikes + "/" + maxStrikes;
    }

    private void GameOver()
    {
        isGameOver = true;
        if (scoreCoroutine != null)
            StopCoroutine(scoreCoroutine);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Background Music Controls
    public void ToggleMusic()
    {
        if (audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
        }
    }

    // Power-Up Spawning Logic
    private void SpawnRainbowPowerUp()
    {
        float spawnX = Random.Range(113f, 657f);
        float spawnY = Random.Range(400f, 700f);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        GameObject powerUp = Instantiate(rainbowPowerUpPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = powerUp.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f;  // Makes it fall
        rb.linearVelocity = Vector2.down * 2f;  // Ensures it moves downward
    }

    // **Newly Added: GetColorAt() Method**
    public Color GetColorAt(int index)
    {
        if (colorArray != null && colorArray.Length > 0)
            return colorArray[index % colorArray.Length];

        Debug.LogWarning("Color array is empty or not assigned in GameManager!");
        return Color.white; // Fallback to white
    }

    public int GetColorCount()
    {
        return colorArray.Length;
    }

    public bool AreColorsMatching(Color ballColor, Color paddleColor)
    {
        return Mathf.Approximately(ballColor.r, paddleColor.r) &&
               Mathf.Approximately(ballColor.g, paddleColor.g) &&
               Mathf.Approximately(ballColor.b, paddleColor.b);
    }
}
