using UnityEngine;
using TMPro;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public GameObject titleScreenPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI promptText;
    private GameManager gameManager;
    private int currentColorIndex = 0;
    private float colorChangeSpeed = 0.5f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null || gameManager.GetColorCount() == 0)
        {
            Debug.LogError("GameManager or Color Array is missing!");
            return;
        }

        titleScreenPanel.SetActive(true);
        StartCoroutine(ShiftTitleColors());
        StartCoroutine(BlinkPrompt());
    }

    private IEnumerator ShiftTitleColors()
    {
        while (true)
        {
            titleText.color = gameManager.GetColorAt(currentColorIndex);
            currentColorIndex = (currentColorIndex + 1) % gameManager.GetColorCount();
            yield return new WaitForSeconds(colorChangeSpeed);
        }
    }

    private IEnumerator BlinkPrompt()
    {
        while (true)
        {
            promptText.alpha = 1f;
            yield return new WaitForSeconds(0.8f);
            promptText.alpha = 0f;
            yield return new WaitForSeconds(0.8f);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            titleScreenPanel.SetActive(false);
        }
    }
}