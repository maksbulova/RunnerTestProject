using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private ParticleSystem warpEffect;

    public enum GameState
    {
        startScreen,
        run,
        gameOver
    }

    public static GameState gameState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ShowStartMenu();
    }

    // Game start or restart.
    public void ShowStartMenu()
    {
        gameState = GameState.startScreen;
        startScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        warpEffect.Pause();

        StartCoroutine(startTouchHandler());
    }

    // By screen touch.
    public void StartLevel()
    {
        gameState = GameState.run;
        startScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        warpEffect.Play();
    }

    // Player dead.
    public void EndLevel()
    {
        gameState = GameState.gameOver;
        startScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        warpEffect.Pause();
    }

    // Called by button.
    public void SceneReload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        ShowStartMenu();
    }

    private IEnumerator startTouchHandler()
    {
        while (gameState == GameState.startScreen)
        {
            if (Input.touchCount > 0)
            {
                StartLevel();
            }
            yield return null;
        }
    }
}
