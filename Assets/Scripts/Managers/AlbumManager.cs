using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlbumManager : MonoBehaviour
{
    [Header("Данные")]
    [SerializeField] private CollectionData[] _collections;

    [Header("Альбом: шапка")]
    [SerializeField] private TextMeshProUGUI _pointsText;

    [Header("Альбом: сетка")]
    [SerializeField] private Transform _drawingsGrid;
    [SerializeField] private GameObject _cellTemplate;

    [Header("Раскрашивание")]
    [SerializeField] private GameObject _coloringPanel;
    [SerializeField] private TextMeshProUGUI _drawingTitleText;
    [SerializeField] private Transform _elementsContainer;
    [SerializeField] private GameObject _elementTemplate;
    [SerializeField] private TextMeshProUGUI _rewardText;

    private int _currentCollectionIndex;
    private DrawingData _currentDrawing;

    private readonly List<GameObject> _spawnedCells = new List<GameObject>();
    private readonly List<GameObject> _spawnedElements = new List<GameObject>();

    private static readonly Color[] ElementPalette =
    {
        Color.red,
        new Color(1f, 0.5f, 0f),
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.6f, 0f, 0.8f)
    };

    private void OnEnable()
    {
        OpenAlbum();
    }

    public void OpenAlbum()
    {
        _currentCollectionIndex = 0;

        if (_coloringPanel != null)
        {
            _coloringPanel.SetActive(false);
        }

        RefreshAll();
    }

    public void OnTabClicked(int index)
    {
        if (_collections == null || index < 0 || index >= _collections.Length)
        {
            return;
        }

        _currentCollectionIndex = index;
        RefreshGrid();
    }

    public void OnCloseClicked()
    {
        if (ScreenManager.Instance != null)
        {
            ScreenManager.Instance.ShowMainMenu();
        }
    }

    public void OnColoringBackClicked()
    {
        if (_coloringPanel != null)
        {
            _coloringPanel.SetActive(false);
        }

        RefreshAll();
    }

    private void RefreshAll()
    {
        if (_pointsText != null && SaveManager.Instance != null)
        {
            _pointsText.text = $"Очки: {SaveManager.Instance.TotalPoints}";
        }

        RefreshGrid();
    }

    private void RefreshGrid()
    {
        ClearList(_spawnedCells);

        if (_collections == null || _collections.Length == 0)
        {
            return;
        }

        CollectionData collection = _collections[_currentCollectionIndex];

        if (collection == null)
        {
            return;
        }

        foreach (DrawingData drawing in collection.drawings)
        {
            SpawnCell(drawing);
        }
    }

    private void SpawnCell(DrawingData drawing)
    {
        if (_cellTemplate == null || _drawingsGrid == null)
        {
            return;
        }

        GameObject cell = Instantiate(_cellTemplate, _drawingsGrid);
        cell.SetActive(true);
        _spawnedCells.Add(cell);

        Image image = cell.GetComponent<Image>();
        TextMeshProUGUI text = cell.GetComponentInChildren<TextMeshProUGUI>();
        Button button = cell.GetComponent<Button>();

        bool unlocked = SaveManager.Instance != null &&
            SaveManager.Instance.IsDrawingUnlocked(drawing.drawingId);
        int colored = SaveManager.Instance != null ?
            SaveManager.Instance.GetColoredCount(drawing.drawingId) : 0;
        bool completed = colored >= drawing.elementCount;

        if (image != null)
        {
            if (!unlocked)
            {
                image.color = new Color(0.35f, 0.35f, 0.35f);
            }
            else if (completed)
            {
                image.color = new Color(0.6f, 0.9f, 0.6f);
            }
            else
            {
                image.color = Color.white;
            }
        }

        if (text != null)
        {
            if (!unlocked)
            {
                text.text = $"Цена: {drawing.unlockCost}";
            }
            else if (completed)
            {
                text.text = "Готово";
            }
            else
            {
                text.text = $"{colored}/{drawing.elementCount}";
            }
        }

        if (button != null)
        {
            button.onClick.AddListener(() => OnCellClicked(drawing));
        }
    }

    private void OnCellClicked(DrawingData drawing)
    {
        if (SaveManager.Instance == null)
        {
            return;
        }

        if (!SaveManager.Instance.IsDrawingUnlocked(drawing.drawingId))
        {
            if (SaveManager.Instance.TrySpendPoints(drawing.unlockCost))
            {
                SaveManager.Instance.UnlockDrawing(drawing.drawingId);
                RefreshAll();
            }
            else
            {
                Debug.Log("Альбом: недостаточно очков для разблокировки");
            }
        }
        else
        {
            OpenColoring(drawing);
        }
    }

    private void OpenColoring(DrawingData drawing)
    {
        _currentDrawing = drawing;

        if (_coloringPanel != null)
        {
            _coloringPanel.SetActive(true);
        }

        if (_drawingTitleText != null)
        {
            _drawingTitleText.text = drawing.drawingName;
        }

        if (_rewardText != null)
        {
            _rewardText.gameObject.SetActive(false);
        }

        ClearList(_spawnedElements);

        int colored = SaveManager.Instance != null ?
            SaveManager.Instance.GetColoredCount(drawing.drawingId) : 0;

        for (int i = 0; i < drawing.elementCount; i++)
        {
            SpawnElement(i, i < colored);
        }
    }

    private void SpawnElement(int index, bool isColored)
    {
        if (_elementTemplate == null || _elementsContainer == null)
        {
            return;
        }

        GameObject element = Instantiate(_elementTemplate, _elementsContainer);
        element.SetActive(true);
        _spawnedElements.Add(element);

        Image image = element.GetComponent<Image>();
        Button button = element.GetComponent<Button>();

        if (image != null)
        {
            image.color = isColored
                ? ElementPalette[index % ElementPalette.Length]
                : new Color(0.8f, 0.8f, 0.8f);
        }

        if (button != null)
        {
            int capturedIndex = index;
            button.onClick.AddListener(() => OnElementClicked(capturedIndex));
        }
    }

    private void OnElementClicked(int index)
    {
        if (_currentDrawing == null || SaveManager.Instance == null)
        {
            return;
        }

        int colored = SaveManager.Instance.GetColoredCount(_currentDrawing.drawingId);

        if (index != colored)
        {
            return;
        }

        SaveManager.Instance.SetColoredCount(_currentDrawing.drawingId, colored + 1);

        if (index < _spawnedElements.Count)
        {
            Image image = _spawnedElements[index].GetComponent<Image>();

            if (image != null)
            {
                image.color = ElementPalette[index % ElementPalette.Length];
            }
        }

        if (colored + 1 >= _currentDrawing.elementCount)
        {
            CompleteDrawing();
        }
    }

    private void CompleteDrawing()
    {
        if (_currentDrawing.rewardBonus != BonusType.None && SaveManager.Instance != null)
        {
            SaveManager.Instance.AddBonus(_currentDrawing.rewardBonus, _currentDrawing.rewardBonusCount);

            if (_rewardText != null)
            {
                _rewardText.gameObject.SetActive(true);
                _rewardText.text = $"Награда: {_currentDrawing.rewardBonus} +{_currentDrawing.rewardBonusCount}";
            }
        }
    }

    private void ClearList(List<GameObject> list)
    {
        foreach (GameObject go in list)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }

        list.Clear();
    }
}