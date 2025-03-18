using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 200f;
    private Rigidbody2D rb;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    private void ResetBall()
    {
        transform.position = new Vector2(400, 185);
        rb.linearVelocity = new Vector2(Random.Range(-1f, 1f), 1f).normalized * speed;
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (gameManager != null && gameManager.GetColorCount() > 0)
        {
            int randomIndex = Random.Range(0, gameManager.GetColorCount());
            GetComponent<SpriteRenderer>().color = gameManager.GetColorAt(randomIndex);
            Debug.Log("Ball color changed to: " + gameManager.GetColorAt(randomIndex).ToString());
        }
        else
        {
            Debug.LogWarning("GameManager or Color Array is not set properly!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            PaddleController paddle = collision.gameObject.GetComponent<PaddleController>();
            if (paddle != null)
            {
                Color ballColor = GetComponent<SpriteRenderer>().color;
                Color paddleColor = paddle.GetCurrentColor();

                if (gameManager.AreColorsMatching(ballColor, paddleColor) || paddleColor == Color.white)
                {
                    gameManager.AddScore(1);
                    speed *= 1.05f;  // Gradual speed increase
                    rb.linearVelocity = rb.linearVelocity.normalized * speed;
                    ChangeColor();  // Change color after a successful hit
                }
                else
                {
                    gameManager.AddStrike();
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("TopWall"))
        {
            ChangeColor();  // Change color when hitting walls
        }
        else if (collision.gameObject.CompareTag("OutOfBounds"))
        {
            gameManager.AddStrike();
            ResetBall();
        }
    }
}
