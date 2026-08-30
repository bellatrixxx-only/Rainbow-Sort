using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Лучший счет")]
    [SerializeField] private TMPro.TextMeshProUGUI _bestScoreText;

    [Header("Кнопки настроек")]
    [SerializeField] private TMPro.TextMeshProUGUI _languageButtonText;
    [SerializeField] private TMPro.TextMeshProUGUI _musicButtonText;
    [SerializeField] private TMPro.TextMeshProUGUI _soundButtonText;

    private void OnEnable()
    {
        UpdateBestScoreDisplay();
        UpdateSettingsDisplay();
    }

    private void Start()
    {
        UpdateBestScoreDisplay();
        UpdateSettingsDisplay();
    }

    public void OnStartClicked()
    {
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.StartGame();
        }
    }

    public void OnAlbumClicked()
    {
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.ShowAlbum();
        }
    }

    public void OnLeaderboardClicked()
    {
        Debug.Log("Лидерборд - в разработке");
    }

    public void OnLanguageClicked()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        string newLanguage = SaveManager.Instance.Language == "ru" ? "en" : "ru";
        SaveManager.Instance.SaveLanguage(newLanguage);
        UpdateSettingsDisplay();
    }

    public void OnMusicClicked()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        SaveManager.Instance.SaveMusicSettings(!SaveManager.Instance.MusicEnabled);
        UpdateSettingsDisplay();
    }

    public void OnSoundClicked()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        SaveManager.Instance.SaveSoundSettings(!SaveManager.Instance.SoundEnabled);
        UpdateSettingsDisplay();
    }

    private void UpdateSettingsDisplay()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        if (_languageButtonText != null)
        {
            _languageButtonText.text = SaveManager.Instance.Language == "ru"
                ? "ЯЗЫК\nRU"
                : "ЯЗЫК\nEN";
        }

        if (_musicButtonText != null)
        {
            _musicButtonText.text = SaveManager.Instance.MusicEnabled
                ? "МУЗЫКА\nВКЛ"
                : "МУЗЫКА\nВЫКЛ";
        }

        if (_soundButtonText != null)
        {
            _soundButtonText.text = SaveManager.Instance.SoundEnabled
                ? "ЗВУК\nВКЛ"
                : "ЗВУК\nВЫКЛ";
        }
    }

    private void UpdateBestScoreDisplay()
    {
        if (_bestScoreText != null)
        {
            int bestScore = SaveManager.Instance != null ?
                SaveManager.Instance.BestScore : 0;
            _bestScoreText.text = bestScore.ToString();
        }
    }
}