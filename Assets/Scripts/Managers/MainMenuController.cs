using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Контроллер главного меню. Обрабатывает нажатия кнопок, обновляет отображение данных и настроек.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Лучший счет")]
    [SerializeField] private TMPro.TextMeshProUGUI _bestScoreText;

    [Header("Язык")]
    [SerializeField] private TMPro.TextMeshProUGUI _languageTextValue;

    [Header("Тумблеры")]
    [SerializeField] private Slider _musicToggle;
    [SerializeField] private Image _musicToggleBg;
    [SerializeField] private Slider _soundToggle;
    [SerializeField] private Image _soundToggleBg;

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

        if (_languageTextValue != null)
        {
            _languageTextValue.text = SaveManager.Instance.Language == "ru" ? "РУС" : "ENG";
        }

        SetToggle(_musicToggle, _musicToggleBg, SaveManager.Instance.MusicEnabled);
        SetToggle(_soundToggle, _soundToggleBg, SaveManager.Instance.SoundEnabled);
    }

    private void SetToggle(Slider slider, Image background, bool isOn)
    {
        if (slider != null)
        {
            slider.value = isOn ? 1f : 0f;
        }

        if (background != null)
        {
            background.color = isOn
                ? new Color(0.45f, 0.85f, 0.45f)
                : new Color(0.9f, 0.35f, 0.35f);
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