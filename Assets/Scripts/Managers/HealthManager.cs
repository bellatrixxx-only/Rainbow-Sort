using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Настройки Спрайтов")]
    [SerializeField] private Sprite _fullHeartSprite;  // Закрашенное сердечко
    [SerializeField] private Sprite _emptyHeartSprite; // Пустое сердечко (силуэт)

    [Header("UI Элементы")]
    [SerializeField] private Image[] heartImages;

    private int maxHealth = 3;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }
    public void UpdateHearts(int lives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < lives ? _fullHeartSprite : _emptyHeartSprite;
        }
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHeartsUI();
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                
                heartImages[i].sprite = _fullHeartSprite;
            }
            else
            {
                
                heartImages[i].sprite = _emptyHeartSprite;
            }
        }
    }
}
