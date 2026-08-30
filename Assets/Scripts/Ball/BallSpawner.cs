using UnityEngine;

/// <summary>
/// Спавнер шариков. Держит не более одного активного шарика за раз.
/// </summary>
public class BallSpawner : MonoBehaviour
{
    [SerializeField] private BallPool _ballPool;

    private Ball _activeBall;
    private float _spawnTimer;
    private bool _waitingForNextSpawn;

    public Ball ActiveBall => _activeBall;
    public bool HasActiveBall => _activeBall != null && _activeBall.IsActive;

    /// <summary>
    /// Сбрасывает состояние спавнера при рестарте игры.
    /// </summary>
    public void ResetSpawner()
    {
        if (_activeBall != null)
        {
            _activeBall.ReturnToPool();
            _activeBall = null;
        }

        _spawnTimer = 0f;
        _waitingForNextSpawn = false;
    }

    /// <summary>
    /// Запускает первый шарик сразу при старте игры.
    /// </summary>
    public void SpawnFirstBall()
    {
        _waitingForNextSpawn = false;
        _spawnTimer = 0f;
        TrySpawnBall();
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
        {
            return;
        }

        if (HasActiveBall || !_waitingForNextSpawn)
        {
            return;
        }

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= GameManager.Instance.CurrentSpawnInterval)
        {
            TrySpawnBall();
        }
    }

    /// <summary>
    /// Планирует появление следующего шарика после задержки.
    /// </summary>
    public void ScheduleNextBall()
    {
        _activeBall = null;
        _waitingForNextSpawn = true;
        _spawnTimer = 0f;
    }

    /// <summary>
    /// Немедленно создаёт новый шарик, если на поле нет активного.
    /// </summary>
    public void TrySpawnBall()
    {
        if (HasActiveBall || GameManager.Instance.State != GameState.Playing)
        {
            return;
        }

        FlaskColor color = GameManager.Instance.GetRandomAvailableBallColor();
        Sprite sprite = GameManager.Instance.GetBallSprite(color);

        if (sprite == null)
        {
            Debug.LogError($"[BallSpawner] Не найден спрайт для цвета: {color}. Проверьте настройки GameManager!");
            return; // Прерываем спавн, чтобы не создавать невидимый шарик
        }

        _activeBall = _ballPool.Get(transform.position);
        _activeBall.Launch(
            color,
            sprite,
            GameManager.Instance.CurrentFallSpeed,
            GameManager.Instance.Balance.failLineY,
            OnBallReturnedToPool);

        _waitingForNextSpawn = false;
        _spawnTimer = 0f;
    }

    private void OnBallReturnedToPool(Ball ball)
    {
        _ballPool.Return(ball);

        if (_activeBall == ball)
        {
            _activeBall = null;
        }
    }
}
