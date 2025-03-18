using UnityEngine;

public class RainbowPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            PaddleController paddle = other.GetComponent<PaddleController>();
            if (paddle != null)
            {
                paddle.ActivateRainbowPowerUp();
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
        }
    }
}
