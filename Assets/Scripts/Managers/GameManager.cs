using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Данные")]
    [SerializeField] private GameBalanceData _balance;
    [SerializeField] private ColorSpritePair[] _colorSprites;

    [Header("Ссылки на сцену")]
    [SerializeField] private BallSpawner _ballSpawner;
    [SerializeField] private FlaskManager _flaskManager;
    [SerializeField] private GameUI _gameUI;

    private GameState _state;
    private int _score;
    private int _multiplier = 1;
    private int _lives;
    private int _currentFlaskCount;

    public GameState State => _state;
    public GameBalanceData Balance => _balance;
    public int Score => _score;
    public int Multiplier => _multiplier;
    public int Lives => _lives;
    public int CurrentFlaskCount => _currentFlaskCount;
    public float CurrentFallSpeed
    {
        get
        {
            int extraFlasks = Mathf.Max(0, _currentFlaskCount - _balance.startFlasksCount);
            return _balance.initialFallSpeed + extraFlasks * _balance.speedIncreasePerFlask;
        }
    }
    public float CurrentSpawnInterval
    {
        get
        {
            int extraFlasks = Mathf.Max(0, _currentFlaskCount - _balance.startFlasksCount);
            return Mathf.Max(0.3f, _balance.initialSpawnInterval - extraFlasks * _balance.spawnIntervalDecreasePerFlask);
        }
    }

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
        _flaskManager.Initialize(_balance, _colorSprites, OnFlaskFilled);

        if (_gameUI != null)
        {
            _gameUI.HideGameOver();
        }
    }
    public void StartGame()
    {
        _state = GameState.Playing;
        _score = 0;
        _multiplier = 1;
        _lives = _balance.livesCount;
        _currentFlaskCount = _balance.startFlasksCount;

        _gameUI.HideGameOver();

        _ballSpawner.ResetSpawner();
        _flaskManager.SpawnStartFlasks();
        _currentFlaskCount = _flaskManager.FlaskCount;

        _ballSpawner.SpawnFirstBall();

        RefreshUI();
    }
    public void RestartGame()
    {
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.StartGame();
        }
        else
        {
            StartGame();
        }
    }
    public void OnFlaskClicked(Flask flask)
    {
        if (_state != GameState.Playing || _ballSpawner.ActiveBall == null)
        {
            return;
        }

        Ball ball = _ballSpawner.ActiveBall;

        if (ball.IsMovingToFlask)
        {
            return;
        }

        if (flask.CanAccept(ball.Color))
        {
            CatchBallInFlask(ball, flask);
        }
        else
        {
            LoseLife();
        }
    }
    public void OnBallReachedFailLine(Ball ball)
    {
        if (_state != GameState.Playing || ball == null || !ball.IsActive)
        {
            return;
        }

        ball.ReturnToPool();
        LoseLife();
        _ballSpawner.ScheduleNextBall();
    }
    public FlaskColor GetRandomAvailableBallColor()
    {
        List<FlaskColor> colors = _flaskManager.GetAvailableColors();

        if (colors.Count == 0)
        {
            return FlaskColor.Red;
        }

        return colors[Random.Range(0, colors.Count)];
    }
    public Sprite GetBallSprite(FlaskColor color)
    {
        foreach (ColorSpritePair pair in _colorSprites)
        {
            if (pair.color == color)
            {
                return pair.ballSprite;
            }
        }

        return null;
    }
    private void CatchBallInFlask(Ball ball, Flask flask)
    {
        Sprite ballSprite = GetBallSprite(ball.Color);
        int fillIndex = flask.FillCount;

        ball.MoveToFlask(flask.GetCatchPoint(), () =>
        {
            AddScore(_balance.pointsPerBall);
            flask.AddVisualBall(ballSprite, fillIndex, _balance.ballsPerFlask);
            flask.AddBall();
            _ballSpawner.ScheduleNextBall();
        });
    }
    private void OnFlaskFilled(Flask flask)
    {
        AddScore(_balance.pointsPerFlask * _multiplier);
        _multiplier = Mathf.Min(_multiplier + 1, _balance.maxMultiplier);

        _flaskManager.HandleFlaskFilled(flask);
        _currentFlaskCount = _flaskManager.FlaskCount;

        RefreshUI();
    }
    private void LoseLife()
    {
        if (_state != GameState.Playing)
        {
            return;
        }

        _lives--;
        _multiplier = 1;

        RefreshUI();

        if (_lives <= 0)
        {
            TriggerGameOver();
        }
    }
    private void TriggerGameOver()
    {
        _state = GameState.GameOver;

        if (_ballSpawner.ActiveBall != null)
        {
            _ballSpawner.ActiveBall.ReturnToPool();
        }

        _ballSpawner.ResetSpawner();
        bool isNewRecord = false;
        if (SaveManager.Instance != null)
        {
            int previousBest = SaveManager.Instance.BestScore;
            SaveManager.Instance.SaveBestScore(_score);
            SaveManager.Instance.AddTotalPoints(_score);

            isNewRecord = _score > previousBest && _score > 0;
        }
        int bestScore = SaveManager.Instance != null ? SaveManager.Instance.BestScore : 0;
        _gameUI.ShowGameOver(_score, bestScore, isNewRecord);
    }

    private void AddScore(int points)
    {
        _score += points;
        RefreshUI();
    }

    private void RefreshUI()
    {
        _gameUI.UpdateScore(_score);
        _gameUI.UpdateMultiplier(_multiplier);
        _gameUI.UpdateLives(_lives);
    }
}