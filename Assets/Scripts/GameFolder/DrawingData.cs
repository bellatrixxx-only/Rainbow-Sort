using UnityEngine;

[CreateAssetMenu(fileName = "NewDrawing", menuName = "Album/DrawingData")]
public class DrawingData : ScriptableObject
{
    [Header("Основные данные")]
    [Tooltip("Название рисунка")]
    public string drawingName;

    [Tooltip("Уникальный идентификатор для сохранений")]
    public string drawingId;

    [Tooltip("Стоимость разблокировки в очках")]
    public int unlockCost;

    [Header("Элементы")]
    [Tooltip("Количество элементов для раскрашивания")]
    public int elementCount;

    [Header("Награда")]
    [Tooltip("Бонус за полное раскрашивание (None — без награды)")]
    public BonusType rewardBonus = BonusType.None;

    [Tooltip("Количество выдаваемых бонусов")]
    public int rewardBonusCount = 1;
}