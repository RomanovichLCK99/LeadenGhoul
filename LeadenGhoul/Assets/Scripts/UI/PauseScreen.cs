using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{

    public static PauseScreen instance;

    [SerializeField] private GameObject pauseScreen = null;
    [SerializeField] private string mainMenuString = "MainMenu";
    [SerializeField] private Animator sceneControlAnim = null;

    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float fadeOutDuration = 1f;

    [HideInInspector] public bool isPaused = false;
    float lastTimeScale = 1f;

    #region Singleton
    private void Awake()
    {
        instance = this;
    }
    #endregion

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            isPaused = true;
        }
        else
        {
            Time.timeScale = lastTimeScale;
            pauseScreen.SetActive(false);
            isPaused = false;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 0;
        StartCoroutine(fadeOutMainMenu());
    }

    IEnumerator fadeOutMainMenu(){
        Physics2D.IgnoreLayerCollision(8, 17, false);
        sceneControlAnim.SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(fadeOutDuration);
        Time.timeScale = lastTimeScale;
        SceneManager.LoadScene(mainMenuString);
    }

      
        
}
