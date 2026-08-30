using UnityEngine;
public class PauseInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePauseToggle();
        }
    }

    private void HandlePauseToggle()
    {
        if (GameManager.Instance == null || ScreenManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.State == GameState.Playing)
        {
            ScreenManager.Instance.ShowPause();
        }
    }
}