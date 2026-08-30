using UnityEngine;

/// <summary>
/// Обрабатывает клики мыши и тапы по экрану через Raycast2D.
/// </summary>
public class GameInputHandler : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
        {
            return;
        }

        if (!WasPointerPressed())
        {
            return;
        }

        Vector3 screenPosition = GetPointerScreenPosition();
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider == null)
        {
            return;
        }

        Flask flask = hit.collider.GetComponent<Flask>();
        if (flask != null)
        {
            GameManager.Instance.OnFlaskClicked(flask);
        }
    }

    /// <summary>
    /// Проверяет нажатие мыши или первого касания.
    /// </summary>
    private bool WasPointerPressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    /// <summary>
    /// Возвращает экранные координаты указателя.
    /// </summary>
    private Vector3 GetPointerScreenPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }

        return Input.mousePosition;
    }
}
