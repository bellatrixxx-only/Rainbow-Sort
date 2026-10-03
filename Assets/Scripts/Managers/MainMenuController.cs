using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Управляет главным меню и отображением сохранённых настроек.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Лучший счёт")]
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    [Header("Язык")]
    [SerializeField] private TextMeshProUGUI _languageTextValue;
    [SerializeField] private LanguagePopupController _languagePopupController;

    [Header("Музыка")]
    [SerializeField] private Slider _musicToggle;
    [SerializeField] private Image _musicToggleBg;

    [Header("Звук")]
    [SerializeField] private Slider _soundToggle;
    [SerializeField] private Image _soundToggleBg;

    [Header("Цвета переключателей")]
    [SerializeField]
    private Color _enabledColor =
        new Color32(115, 190, 45, 255);

    [SerializeField]
    private Color _disabledColor =
        new Color32(150, 150, 150, 255);

    private void OnEnable()
    {
        UpdateBestScoreDisplay();
        UpdateSettingsDisplay();
    }

    private void Start()
    {
        // Повторное обновление после Awake всех объектов,
        // включая загрузку данных в SaveManager.
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
        if (_languagePopupController == null)
        {
            Debug.LogWarning(
                "MainMenuController: не назначено окно настроек.",
                this
            );
            return;
        }

        _languagePopupController.Open();
        RefreshSettings();
    }

    public void RefreshSettings()
    {
        UpdateSettingsDisplay();
    }

    /// <summary>
    /// Подключается к On Value Changed слайдера музыки.
    /// </summary>
    public void OnMusicValueChanged(float value)
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        bool isEnabled = value >= 0.5f;

        if (SaveManager.Instance.MusicEnabled != isEnabled)
        {
            SaveManager.Instance.SaveMusicSettings(isEnabled);
        }

        UpdateSettingsDisplay();
    }

    /// <summary>
    /// Подключается к On Value Changed слайдера звука.
    /// </summary>
    public void OnSoundValueChanged(float value)
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        bool isEnabled = value >= 0.5f;

        if (SaveManager.Instance.SoundEnabled != isEnabled)
        {
            SaveManager.Instance.SaveSoundSettings(isEnabled);
        }

        UpdateSettingsDisplay();
    }

    // Оставлены для совместимости со старыми кнопками.
    // К событиям слайдеров эти методы не подключать.
    public void OnMusicClicked()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        OnMusicValueChanged(
            SaveManager.Instance.MusicEnabled ? 0f : 1f
        );
    }

    public void OnSoundClicked()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        OnSoundValueChanged(
            SaveManager.Instance.SoundEnabled ? 0f : 1f
        );
    }

    private void UpdateSettingsDisplay()
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        if (_languageTextValue != null)
        {
            _languageTextValue.text =
                SaveManager.Instance.Language == "ru"
                    ? "РУС"
                    : "ENG";
        }

        SetToggle(
            _musicToggle,
            _musicToggleBg,
            SaveManager.Instance.MusicEnabled
        );

        SetToggle(
            _soundToggle,
            _soundToggleBg,
            SaveManager.Instance.SoundEnabled
        );
    }

    private void SetToggle(
        Slider slider,
        Image background,
        bool isEnabled)
    {
        if (slider != null)
        {
            // Обновляет положение без повторного вызова
            // события On Value Changed.
            slider.SetValueWithoutNotify(isEnabled ? 1f : 0f);
        }

        if (background != null)
        {
            background.color =
                isEnabled ? _enabledColor : _disabledColor;
        }
    }

    private void UpdateBestScoreDisplay()
    {
        if (_bestScoreText == null)
        {
            return;
        }

        int bestScore = SaveManager.Instance != null
            ? SaveManager.Instance.BestScore
            : 0;

        _bestScoreText.text = bestScore.ToString();
    }
}