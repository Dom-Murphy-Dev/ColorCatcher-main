using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private float horizontal;
    private float speed = 400f;
    private int currentColorIndex = 0;
    private bool isRainbowActive = false;
    private float rainbowDuration = 5f;
    private Coroutine rainbowCoroutine;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = gameManager.GetColorAt(currentColorIndex);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (!isRainbowActive)
        {
            if (Input.GetKeyDown(KeyCode.Z)) ChangeColor(-1);
            if (Input.GetKeyDown(KeyCode.X)) ChangeColor(1);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void ChangeColor(int direction)
    {
        currentColorIndex = (currentColorIndex + direction + gameManager.GetColorCount()) % gameManager.GetColorCount();
        spriteRenderer.color = gameManager.GetColorAt(currentColorIndex);
    }

    public Color GetCurrentColor()
    {
        return isRainbowActive ? Color.white : spriteRenderer.color;
    }

    public void ActivateRainbowPowerUp()
    {
        if (rainbowCoroutine != null)
            StopCoroutine(rainbowCoroutine);
        rainbowCoroutine = StartCoroutine(RainbowEffect());
    }

    private IEnumerator RainbowEffect()
    {
        isRainbowActive = true;
        float elapsed = 0f;

        while (elapsed < rainbowDuration)
        {
            spriteRenderer.color = gameManager.GetColorAt((int)(elapsed * 10) % gameManager.GetColorCount());
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRainbowActive = false;
        spriteRenderer.color = gameManager.GetColorAt(currentColorIndex);  // Revert to last selected color
    }
}
