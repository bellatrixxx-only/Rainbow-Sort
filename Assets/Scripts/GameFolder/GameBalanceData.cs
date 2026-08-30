using UnityEngine;

[CreateAssetMenu(fileName = "GameBalanceData", menuName = "Rainbow Sort/Game Balance Data")]
public class GameBalanceData : ScriptableObject
{
    [Header("Колбы")]
    [Tooltip("Начальное количество колб на поле")]
    public int startFlasksCount = 2;

    [Tooltip("Максимальное количество колб")]
    public int maxFlasksCount = 7;

    [Tooltip("Шариков для заполнения одной колбы")]
    public int ballsPerFlask = 4;

    [Header("Жизни")]
    [Tooltip("Количество жизней игрока")]
    public int livesCount = 3;

    [Header("Скорость и спавн")]
    [Tooltip("Начальная скорость падения шарика")]
    public float initialFallSpeed = 3f;

    [Tooltip("Начальный интервал между шариками")]
    public float initialSpawnInterval = 1.5f;

    [Tooltip("Увеличение скорости за каждую новую колбу")]
    public float speedIncreasePerFlask = 0.3f;

    [Tooltip("Уменьшение интервала спавна за каждую новую колбу")]
    public float spawnIntervalDecreasePerFlask = 0.1f;

    [Header("Очки")]
    [Tooltip("Очки за правильно пойманный шарик")]
    public int pointsPerBall = 1;

    [Tooltip("Бонусные очки за заполненную колбу")]
    public int pointsPerFlask = 4;

    [Header("Промах")]
    [Tooltip("Y-позиция линии промаха — ниже неё шарик считается упавшим")]
    public float failLineY = -8f;

    [Header("Множитель")]
    [Tooltip("Максимальный множитель очков")]
    public int maxMultiplier = 7;
}
