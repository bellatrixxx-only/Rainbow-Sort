using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Падающий шарик. Движется через Transform.Translate без физики.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Ball : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private FlaskColor _color;
    private float _fallSpeed;
    private float _failLineY;
    private bool _isActive;
    private bool _isMovingToFlask;
    private Action<Ball> _onReturnedToPool;

    public FlaskColor Color => _color;
    public bool IsActive => _isActive;
    public bool IsMovingToFlask => _isMovingToFlask;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Активирует шарик из пула и задаёт параметры падения.
    /// </summary>
    public void Launch(FlaskColor color, Sprite sprite, float fallSpeed, float failLineY, Action<Ball> onReturnedToPool)
    {
        _color = color;
        _fallSpeed = fallSpeed;
        _failLineY = failLineY;
        _onReturnedToPool = onReturnedToPool;
        _isActive = true;
        _isMovingToFlask = false;

        _spriteRenderer.sprite = sprite;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_isActive || _isMovingToFlask)
        {
            return;
        }

        // Падение строго вниз
        transform.Translate(Vector3.down * (_fallSpeed * Time.deltaTime), Space.World);

        if (transform.position.y <= _failLineY)
        {
            GameManager.Instance.OnBallReachedFailLine(this);
        }
    }

    /// <summary>
    /// Плавно перемещает шарик в колбу и возвращает в пул.
    /// </summary>
    public void MoveToFlask(Vector3 targetPosition, Action onComplete)
    {
        if (!_isActive)
        {
            return;
        }

        StartCoroutine(MoveToFlaskRoutine(targetPosition, onComplete));
    }

    private IEnumerator MoveToFlaskRoutine(Vector3 targetPosition, Action onComplete)
    {
        _isMovingToFlask = true;
        Vector3 startPosition = transform.position;
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        onComplete?.Invoke();
        ReturnToPool();
    }

    /// <summary>
    /// возвращает шарик в пуль объектов
    /// </summary>
  public void ReturnToPool()
    {
        if (!_isActive)
        {
            return;
        }

        _isActive = false;
        _isMovingToFlask = false;
        
        transform.localScale = Vector3.one; 
        
        gameObject.SetActive(false);
        _onReturnedToPool?.Invoke(this);
    }
}
