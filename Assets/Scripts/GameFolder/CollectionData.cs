using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollection", menuName = "Album/CollectionData")]
public class CollectionData : ScriptableObject
{
    [Header("Основные данные")]
    [Tooltip("Название коллекции")]
    public string collectionName;

    [Tooltip("Уникальный идентификатор для сохранений")]
    public string collectionId;

    [Header("Рисунки")]
    [Tooltip("Рисунки коллекции (9 штук)")]
    public List<DrawingData> drawings;
}