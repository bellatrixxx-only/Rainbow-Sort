using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string KEY_BEST_SCORE = "BestScore";
    private const string KEY_TOTAL_POINTS = "TotalPoints";
    private const string KEY_IS_FIRST_LAUNCH = "IsFirstLaunch";
    private const string KEY_MUSIC_ENABLED = "MusicEnabled";
    private const string KEY_SOUND_ENABLED = "SoundEnabled";
    private const string KEY_LANGUAGE = "Language";

    private const string KEY_UNLOCKED_PREFIX = "Unlocked_";
    private const string KEY_COLORED_PREFIX = "Colored_";
    private const string KEY_BONUS_PREFIX = "Bonus_";

    public int BestScore { get; private set; }
    public int TotalPoints { get; private set; }
    public bool IsFirstLaunch { get; private set; }
    public bool MusicEnabled { get; private set; }
    public bool SoundEnabled { get; private set; }
    public string Language { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAllData();
    }
    private void LoadAllData()
    {
        BestScore = PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);
        TotalPoints = PlayerPrefs.GetInt(KEY_TOTAL_POINTS, 0);
        IsFirstLaunch = PlayerPrefs.GetInt(KEY_IS_FIRST_LAUNCH, 1) == 1;
        MusicEnabled = PlayerPrefs.GetInt(KEY_MUSIC_ENABLED, 1) == 1;
        SoundEnabled = PlayerPrefs.GetInt(KEY_SOUND_ENABLED, 1) == 1;
        Language = PlayerPrefs.GetString(KEY_LANGUAGE, "ru");

        Debug.Log($"SaveManager: Загружены данные — BestScore: {BestScore}, TotalPoints: {TotalPoints}");
    }

    public void SaveBestScore(int score)
    {
        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt(KEY_BEST_SCORE, BestScore);
            PlayerPrefs.Save();
            Debug.Log($"SaveManager: Новый рекорд! {BestScore}");
        }
    }
    public void AddTotalPoints(int points)
    {
        TotalPoints += points;
        PlayerPrefs.SetInt(KEY_TOTAL_POINTS, TotalPoints);
        PlayerPrefs.Save();
        Debug.Log($"SaveManager: Добавлено {points} очков. Всего: {TotalPoints}");
    }
    public bool TrySpendPoints(int cost)
    {
        if (TotalPoints < cost)
        {
            return false;
        }

        TotalPoints -= cost;
        PlayerPrefs.SetInt(KEY_TOTAL_POINTS, TotalPoints);
        PlayerPrefs.Save();
        Debug.Log($"SaveManager: Списано {cost} очков. Осталось: {TotalPoints}");
        return true;
    }

    public void SaveMusicSettings(bool enabled)
    {
        MusicEnabled = enabled;
        PlayerPrefs.SetInt(KEY_MUSIC_ENABLED, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveSoundSettings(bool enabled)
    {
        SoundEnabled = enabled;
        PlayerPrefs.SetInt(KEY_SOUND_ENABLED, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveLanguage(string languageCode)
    {
        Language = languageCode;
        PlayerPrefs.SetString(KEY_LANGUAGE, languageCode);
        PlayerPrefs.Save();
    }

    public void MarkAsLaunched()
    {
        if (IsFirstLaunch)
        {
            IsFirstLaunch = false;
            PlayerPrefs.SetInt(KEY_IS_FIRST_LAUNCH, 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsDrawingUnlocked(string drawingId)
    {
        return PlayerPrefs.GetInt(KEY_UNLOCKED_PREFIX + drawingId, 0) == 1;
    }

    public void UnlockDrawing(string drawingId)
    {
        PlayerPrefs.SetInt(KEY_UNLOCKED_PREFIX + drawingId, 1);
        PlayerPrefs.Save();
        Debug.Log($"SaveManager: рисунок {drawingId} разблокирован");
    }

    public int GetColoredCount(string drawingId)
    {
        return PlayerPrefs.GetInt(KEY_COLORED_PREFIX + drawingId, 0);
    }

    public void SetColoredCount(string drawingId, int count)
    {
        PlayerPrefs.SetInt(KEY_COLORED_PREFIX + drawingId, count);
        PlayerPrefs.Save();
    }

    public int GetBonusCount(BonusType bonusType)
    {
        return PlayerPrefs.GetInt(KEY_BONUS_PREFIX + bonusType, 0);
    }

    public void AddBonus(BonusType bonusType, int count)
    {
        int current = GetBonusCount(bonusType);
        PlayerPrefs.SetInt(KEY_BONUS_PREFIX + bonusType, current + count);
        PlayerPrefs.Save();
        Debug.Log($"SaveManager: бонус {bonusType} +{count}, всего {current + count}");
    }

    public bool TrySpendBonus(BonusType bonusType)
    {
        int current = GetBonusCount(bonusType);

        if (current <= 0)
        {
            return false;
        }

        PlayerPrefs.SetInt(KEY_BONUS_PREFIX + bonusType, current - 1);
        PlayerPrefs.Save();
        return true;
    }

    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        LoadAllData();
        Debug.Log("SaveManager: Все данные сброшены");
    }
}