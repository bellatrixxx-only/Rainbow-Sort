using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Колба определённого цвета. Принимает шарики и отслеживает заполненность.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Flask : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fillIndicator;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private FlaskColor _color;
    private int _fillCount;
    private int _ballsPerFlask;
    private Action<Flask> _onFilled;

    public FlaskColor Color => _color;
    public int FillCount => _fillCount;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

 public class BallInFlask : MonoBehaviour
 {
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Sprite sprite, Vector3 localPosition)
    {
        _spriteRenderer.sprite = sprite;
        transform.localPosition = localPosition;
    }
}

private readonly List<Transform> _visualBalls = new List<Transform>();

// Метод добавления визуального шарика:
public void AddVisualBall(Sprite ballSprite, int fillIndex, int ballsPerFlask)
{
    // Создаем визуальный шарик как дочерний объект колбы
    GameObject ballObject = new GameObject($"Ball_{fillIndex}");
    ballObject.transform.SetParent(transform);
    
    SpriteRenderer sr = ballObject.AddComponent<SpriteRenderer>();
    sr.sprite = ballSprite;
    sr.sortingOrder = 5; // Поверх колбы
    
    // Рассчитываем позицию: шарики укладываются снизу вверх
    float ballHeight = 0.8f; // Высота одного шарика (подбери под свой спрайт)
    float startY = -1.5f; // Начальная позиция (низ колбы)
    ballObject.transform.localPosition = new Vector3(0, startY + fillIndex * ballHeight, 0);
    
    _visualBalls.Add(ballObject.transform);
}


public void ClearVisualBalls()
{
    foreach (Transform ball in _visualBalls)
    {
        if (ball != null)
        {
            Destroy(ball.gameObject);
        }
    }
    _visualBalls.Clear();
}

    public void Initialize(FlaskColor color, Sprite flaskSprite, int ballsPerFlask, Action<Flask> onFilled)
    {
        _color = color;
        _ballsPerFlask = ballsPerFlask;
        _onFilled = onFilled;
        _fillCount = 0;

        _spriteRenderer.sprite = flaskSprite;
        UpdateFillVisual();
    }

    /// <summary>
    /// Проверяет, подходит ли цвет шарика для этой колбы.
    /// </summary>
    public bool CanAccept(FlaskColor ballColor)
    {
        return ballColor == _color && _fillCount < _ballsPerFlask;
    }

    /// <summary>
    /// Увеличивает счётчик заполненности после попадания шарика.
    /// </summary>
    public void AddBall()
    {
        _fillCount++;
        UpdateFillVisual();

        if (_fillCount >= _ballsPerFlask)
        {
            _onFilled?.Invoke(this);
        }
    }

    /// <summary>
    /// Возвращает точку, куда должен лететь шарик.
    /// </summary>
    public Vector3 GetCatchPoint()
    {
        return transform.position + Vector3.up * 1.5f;
    }

    /// <summary>
    /// Скрывает колбу после «лопания».
    /// </summary>
   public void Clear()
{
    _fillCount = 0;
    ClearVisualBalls();
    UpdateFillVisual();
}

    /// <summary>
    /// Показывает колбу с новым цветом после замены.
    /// </summary>
    public void ResetFlask(FlaskColor newColor, Sprite flaskSprite)
    {
        _color = newColor;
        _fillCount = 0;
        _spriteRenderer.sprite = flaskSprite;
        gameObject.SetActive(true);
        UpdateFillVisual();
    }

    private void UpdateFillVisual()
    {
        if (_fillIndicator == null)
        {
            return;
        }

        // Простая индикация заполненности через прозрачность
        float fillRatio = _ballsPerFlask > 0 ? (float)_fillCount / _ballsPerFlask : 0f;
        Color indicatorColor = _fillIndicator.color;
        indicatorColor.a = 0.25f + fillRatio * 0.5f;
        _fillIndicator.color = indicatorColor;
    }
}
