using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контроллер окна выбора языка. Висит на корневом объекте окна.
/// Управляет активностью всего окна через gameObject.
/// </summary>
public class LanguagePopupController : MonoBehaviour
{
    [Header("Отображение")]
    [SerializeField] private Image _flagImage;
    [SerializeField] private Sprite _rusFlag;
    [SerializeField] private Sprite _engFlag;

    [Header("Взаимосвязь")]
    [SerializeField] private MainMenuController _mainMenuController;

    private int _viewIndex;

    /// <summary>
    /// Открывает окно выбора языка на сохранённом языке.
    /// </summary>
    public void Open()
    {
        _viewIndex = SaveManager.Instance != null && SaveManager.Instance.Language == "en" ? 1 : 0;
        RefreshFlag();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Листание языков стрелками.
    /// </summary>
    public void OnArrowClicked(int direction)
    {
        _viewIndex = (_viewIndex + 1) % 2;
        RefreshFlag();
    }

    /// <summary>
    /// Применение выбранного языка и закрытие окна.
    /// </summary>
    public void OnConfirmClicked()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveLanguage(_viewIndex == 0 ? "ru" : "en");
        }

        if (_mainMenuController != null)
        {
            _mainMenuController.RefreshSettings();
        }

        Close();
    }

    /// <summary>
    /// Закрытие окна без применения изменений.
    /// </summary>
    public void OnCloseClicked()
    {
        Close();
    }

    /// <summary>
    /// Скрывает всё окно целиком.
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void RefreshFlag()
    {
        if (_flagImage != null)
        {
            _flagImage.sprite = _viewIndex == 0 ? _rusFlag : _engFlag;
        }
    }
}