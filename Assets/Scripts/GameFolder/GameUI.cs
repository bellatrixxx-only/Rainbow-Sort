using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Игровой HUD")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _multiplierText;
    [SerializeField] private Text _livesText;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Text _finalScoreText;
    [SerializeField] private Text _bestScoreText;
    [SerializeField] private Text _newRecordText;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
        }

        HideGameOver();
    }
    public void UpdateScore(int score)
    {
        if (_scoreText != null)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
    public void UpdateMultiplier(int multiplier)
    {
        if (_multiplierText != null)
        {
            _multiplierText.text = $"x{multiplier}";
        }
    }
    public void UpdateLives(int lives)
    {
        if (_livesText != null)
        {
            _livesText.text = $"Lives: {lives}";
        }
    }
    public void ShowGameOver(int finalScore, int bestScore, bool isNewRecord)
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }

        if (isNewRecord)
        {
            if (_finalScoreText != null)
            {
                _finalScoreText.gameObject.SetActive(false);
            }

            if (_bestScoreText != null)
            {
                _bestScoreText.gameObject.SetActive(false);
            }

            if (_newRecordText != null)
            {
                _newRecordText.gameObject.SetActive(true);
                _newRecordText.text = $"НОВЫЙ РЕКОРД: {finalScore}";
            }
        }
        else
        {
            if (_finalScoreText != null)
            {
                _finalScoreText.gameObject.SetActive(true);
                _finalScoreText.text = $"Final Score: {finalScore}";
            }

            if (_bestScoreText != null)
            {
                _bestScoreText.gameObject.SetActive(true);
                _bestScoreText.text = $"Лучший: {bestScore}";
            }

            if (_newRecordText != null)
            {
                _newRecordText.gameObject.SetActive(false);
            }
        }

        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.ShowGameOver();
        }
    }

    public void HideGameOver()
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(false);
        }
    }

    public void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
}