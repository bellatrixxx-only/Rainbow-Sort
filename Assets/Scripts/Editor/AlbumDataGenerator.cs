using UnityEditor;
using UnityEngine;

public static class AlbumDataGenerator
{
    [MenuItem("Tools/Generate Draft Album")]
    private static void Generate()
    {
        const string path = "Assets/Data/Album";

        if (!AssetDatabase.IsValidFolder("Assets/Data"))
        {
            AssetDatabase.CreateFolder("Assets", "Data");
        }

        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Data", "Album");
        }

        int[] elementCounts = { 3, 5, 7 };

        BonusType[] firstRewards =
        {
            BonusType.ExtraLife,
            BonusType.SlowMotion,
            BonusType.ExtraLife
        };

        for (int c = 0; c < 3; c++)
        {
            CollectionData collection = ScriptableObject.CreateInstance<CollectionData>();
            collection.collectionName = $"Коллекция {c + 1}";
            collection.collectionId = $"collection_{c + 1}";
            collection.drawings = new System.Collections.Generic.List<DrawingData>();

            for (int d = 0; d < 9; d++)
            {
                DrawingData drawing = ScriptableObject.CreateInstance<DrawingData>();
                drawing.drawingName = $"Рисунок {c + 1}-{d + 1}";
                drawing.drawingId = $"c{c + 1}_d{d + 1}";
                drawing.elementCount = elementCounts[c];

                drawing.unlockCost = (c == 0 && d == 0) ? 0 : (c + 1) * 10 + d * 5;

                drawing.rewardBonus = (d == 0) ? firstRewards[c] : BonusType.None;
                drawing.rewardBonusCount = 1;

                AssetDatabase.CreateAsset(drawing, $"{path}/Drawing_c{c + 1}_d{d + 1}.asset");
                collection.drawings.Add(drawing);
            }

            AssetDatabase.CreateAsset(collection, $"{path}/Collection_{c + 1}.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Черновые данные альбома сгенерированы: 3 коллекции, 27 рисунков.");
    }
}