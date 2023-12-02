using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private PlayerInput playerInput;
    private bool isPaused;

    public void OnPauseClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        playerInput.enabled = true;
        isPaused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    private void Pause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            pauseMenuUI.SetActive(true);
            playerInput.enabled = false;
            isPaused = true;
        }
    }
}
