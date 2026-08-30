using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Минимальный пул объектов для шариков.
/// </summary>
public class BallPool : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private int _prewarmCount = 5;
    [SerializeField] private Transform _poolRoot;

    private readonly Queue<Ball> _availableBalls = new Queue<Ball>();

    private void Awake()
    {
        if (_poolRoot == null)
        {
            _poolRoot = transform;
        }

        Prewarm();
    }

    /// <summary>
    /// Создаёт начальный набор шариков в пуле.
    /// </summary>
    private void Prewarm()
    {
        for (int i = 0; i < _prewarmCount; i++)
        {
            CreateBall();
        }
    }

    private Ball CreateBall()
    {
        Ball ball = Instantiate(_ballPrefab, _poolRoot);
        ball.gameObject.SetActive(false);
        _availableBalls.Enqueue(ball);
        return ball;
    }

    /// <summary>
    /// Берёт свободный шарик из пула или создаёт новый.
    /// </summary>
    public Ball Get(Vector3 position)
    {
        Ball ball = _availableBalls.Count > 0 ? _availableBalls.Dequeue() : CreateBall();
        ball.transform.position = position;
        return ball;
    }

    /// <summary>
    /// Возвращает шарик обратно в пул.
    /// </summary>
      public void Return(Ball ball)
    {
        ball.transform.SetParent(_poolRoot);
        _availableBalls.Enqueue(ball);
    }
}
