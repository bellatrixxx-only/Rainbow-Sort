using UnityEngine;
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Экраны")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _gameplayHUD;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _albumPanel;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowAlbum()
    {
        SetActiveScreen(_albumPanel);
        Time.timeScale = 1f;
    }

    public void OnAlbumClicked()
    {
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.ShowAlbum();
        }
    }

    public void ShowMainMenu()
    {
        SetActiveScreen(_mainMenuPanel);
        Time.timeScale = 0f;
    }

    public void StartGame() // Запускает игру
    {
        SetActiveScreen(_gameplayHUD);
        
        Time.timeScale = 1f;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    public void ShowGameOver() // Показывает экран Game Over
    {
        SetActiveScreen(_gameOverPanel);
    }

private void SetActiveScreen(GameObject screen)
{
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
        if (_gameplayHUD != null) _gameplayHUD.SetActive(false);
        if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_albumPanel != null) _albumPanel.SetActive(false);

        if (screen != null)
    {
        screen.SetActive(true);
    }
}

    public void ShowPause()
    { 
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (_pausePanel != null)
        {
        _pausePanel.SetActive(false);
        }
    Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
    ResumeGame(); 
    ShowMainMenu();
    }

    
}