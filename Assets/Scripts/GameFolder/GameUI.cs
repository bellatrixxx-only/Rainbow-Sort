using TMPro;
using UnityEngine;

/// <summary>
/// Контроллер HUD и экрана Game Over.
/// Текст — TMP, жизни — через HealthManager, Game Over — два варианта блоков.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    [SerializeField] private TextMeshProUGUI _comboValueText;
    [SerializeField] private HealthManager _healthManager;

    [Header("Game Over: обычный вариант")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _blockDefault;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    [Header("Game Over: рекорд")]
    [SerializeField] private GameObject _blockRecord;
    [SerializeField] private TextMeshProUGUI _recordScoreText;

    private void Awake()
    {
        HideGameOver();
    }

    public void UpdateScore(int score)
    {
        if (_scoreValueText != null)
        {
            _scoreValueText.text = score.ToString();
        }
    }

    public void UpdateMultiplier(int multiplier)
    {
        if (_comboValueText != null)
        {
            _comboValueText.text = "x" + multiplier;
        }
    }

    public void UpdateLives(int lives)
    {
        if (_healthManager != null)
        {
            _healthManager.UpdateHearts(lives);
        }
    }

    public void ShowGameOver(int finalScore, int bestScore, bool isNewRecord)
    {
        if (_blockDefault != null)
        {
            _blockDefault.SetActive(!isNewRecord);
        }

        if (_blockRecord != null)
        {
            _blockRecord.SetActive(isNewRecord);
        }

        if (!isNewRecord)
        {
            if (_currentScoreText != null)
            {
                _currentScoreText.text = finalScore.ToString();
            }

            if (_bestScoreText != null)
            {
                _bestScoreText.text = bestScore.ToString();
            }
        }
        else if (_recordScoreText != null)
        {
            _recordScoreText.text = finalScore.ToString();
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

    public void OnMenuClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ToMenu();
        }
    }
}