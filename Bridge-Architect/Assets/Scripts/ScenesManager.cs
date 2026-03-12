using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchCamera()
    {
       
    }
    /// <summary>
    /// Load a scene with fade effect
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    /// <summary>
    /// Load Main Menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    /// <summary>
    /// Load Level Select scene
    /// </summary>
    public void LoadLevelSelect()
    {
        LoadScene("LevelSelect");
    }

    /// <summary>
    /// Load a specific level by name
    /// </summary>
    public void LoadLevel(string levelName)
    {
        Debug.Log($"Loading level: {levelName}");
        LoadScene(levelName);
    }

    /// <summary>
    /// Restart current scene
    /// </summary>
    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quit application
    /// </summary>
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        // Fade out
        yield return FadeOut();
  
        // Load scene
        SceneManager.LoadScene(sceneName);
        
        // Fade in
        yield return FadeIn();
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null) yield break;
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null) yield break;
        
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }
}
