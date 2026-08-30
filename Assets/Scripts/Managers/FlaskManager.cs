using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет расположением колб в ряд внизу экрана.
/// Отвечает за создание, очистку и добавление новых колб согласно цветам радуги.
/// </summary>
public class FlaskManager : MonoBehaviour
{
    [SerializeField] private Flask _flaskPrefab;
    [SerializeField] private float _rowY = -6.44f;

    private readonly List<Flask> _flasks = new List<Flask>();
    private GameBalanceData _balance;
    private ColorSpritePair[] _sprites;
    private Action<Flask> _onFlaskFilled;

    public IReadOnlyList<Flask> Flasks => _flasks;
    public int FlaskCount => _flasks.Count;

    /// <summary>
    /// Инициализирует менеджер колб и создаёт стартовый набор.
    /// </summary>
    public void Initialize(GameBalanceData balance, ColorSpritePair[] sprites, Action<Flask> onFlaskFilled)
    {
        _balance = balance;
        _sprites = sprites;
        _onFlaskFilled = onFlaskFilled;
    }

    /// <summary>
    /// Очищает все колбы перед новой игрой.
    /// </summary>
    public void ClearFlasks()
    {
        for (int i = _flasks.Count - 1; i >= 0; i--)
        {
            if (_flasks[i] != null)
            {
                Destroy(_flasks[i].gameObject);
            }
        }

        _flasks.Clear();
    }

    /// <summary>
    /// Создаёт начальные колбы Red и Orange.
    /// </summary>
    public void SpawnStartFlasks()
    {
        ClearFlasks();
        CreateFlask(FlaskColor.Red);
        CreateFlask(FlaskColor.Orange);
        LayoutFlasks();
    }

    /// <summary>
    /// Обрабатывает заполнение колбы:
    /// 1. Очищает заполненную колбу (шарики исчезают, колба остается на поле).
    /// 2. Добавляет новую колбу следующего цвета радуги, если не достигнут максимум.
    /// </summary>
    public void HandleFlaskFilled(Flask filledFlask)
    {
        // Очищаем заполненную колбу (она остается на поле)
        filledFlask.Clear();

        // Добавляем новую колбу следующего цвета, если еще не достигли максимума
        if (_flasks.Count < _balance.maxFlasksCount)
        {
            FlaskColor? nextColor = GetNextColorAfterLastFlask();
            if (nextColor.HasValue)
            {
                CreateFlask(nextColor.Value);
            }
        }
        LayoutFlasks();
    }

    /// <summary>
    /// Располагает все колбы в одну горизонтальную линию внизу экрана.
    /// Колбы равномерно распределяются по ширине с учетом их количества.
    /// </summary>
    private void LayoutFlasks()
{
    int count = _flasks.Count;
    if (count == 0)
    {
        return;
    }

    Camera cam = Camera.main;
    if (cam == null)
    {
        return;
    }

    // Получаем ширину экрана в мировых единицах
    float screenHeight = cam.orthographicSize * 2f;
    float screenWidth = screenHeight * cam.aspect;
    
    // Доступная ширина для колб (85% от ширины экрана)
    float availableWidth = screenWidth * 0.85f;
    
    // Рассчитываем расстояние между колбами
    // Для 2 колб: большое расстояние, для 7 колб: маленькое
    float spacing = count > 1 ? availableWidth / (count - 1) : 0f;
    
    // Ограничиваем максимальное расстояние (чтобы 2 колбы не были слишком далеко)
    float maxSpacing = 3.0f;
    spacing = Mathf.Min(spacing, maxSpacing);
    
    // Центрируем колбы по горизонтали
    float totalWidth = (count - 1) * spacing;
    float startX = -totalWidth * 0.5f;

    // Расставляем все колбы в одну линию
    for (int i = 0; i < count; i++)
    {
        Vector3 newPosition = new Vector3(startX + i * spacing, _rowY, 0f);
        _flasks[i].transform.localPosition = newPosition;
    }
}
    private Flask CreateFlask(FlaskColor color)
    {
        Flask flask = Instantiate(_flaskPrefab, transform);
        flask.Initialize(color, GetFlaskSprite(color), _balance.ballsPerFlask, _onFlaskFilled);
        _flasks.Add(flask);
        return flask;
    }

    /// <summary>
    /// Возвращает следующий цвет радуги после последнего добавленного цвета.
    /// Порядок: Red → Orange → Yellow → Green → DarkBlue → Blue → Violet
    /// </summary>
    private FlaskColor? GetNextColorAfterLastFlask()
    {
        if (_flasks.Count == 0)
        {
            return FlaskColor.Red;
        }

        // Находим последний добавленный цвет (последний в списке)
        FlaskColor lastColor = _flasks[_flasks.Count - 1].Color;

        // Возвращаем следующий цвет по порядку
        switch (lastColor)
        {
            case FlaskColor.Red:
                return FlaskColor.Orange;
            case FlaskColor.Orange:
                return FlaskColor.Yellow;
            case FlaskColor.Yellow:
                return FlaskColor.Green;
            case FlaskColor.Green:
                return FlaskColor.DarkBlue;
            case FlaskColor.DarkBlue:
                return FlaskColor.Blue;
            case FlaskColor.Blue:
                return FlaskColor.Violet;
            case FlaskColor.Violet:
                // Все 7 цветов уже добавлены
                return null;
            default:
                return null;
        }
    }

    private Sprite GetFlaskSprite(FlaskColor color)
    {
        foreach (ColorSpritePair pair in _sprites)
        {
            if (pair.color == color)
            {
                return pair.flaskSprite;
            }
        }

        return null;
    }

    public List<FlaskColor> GetAvailableColors()
    {
        var colors = new List<FlaskColor>();

        foreach (Flask flask in _flasks)
        {
            if (flask.gameObject.activeSelf)
            {
                colors.Add(flask.Color);
            }
        }

        return colors;
    }
}