using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private string firstLevelString = "Testing Scene";
    [SerializeField] private Animator sceneControlAnim = null;

    [SerializeField] private GameObject[] objectsToToggle = new GameObject[0];
    [SerializeField] private GameObject aboutObjects = null;
    [SerializeField] private GameObject controlsObjects = null;

    [Space(10)]
    [Header("Variables")]
    [SerializeField] private float fadeOutDuration = 1f;

    bool aboutOn = false;
    bool controlsOn = false;

    public void LoadFirstLevel()
    {
        StartCoroutine(fadeOut());
    }

    public void toggleAbout()
    {
        if (!aboutOn)
        {

            foreach (GameObject g in objectsToToggle)
            {
                g.SetActive(false);
            }
            aboutObjects.SetActive(true);
            aboutOn = true;

        } else
        {
            foreach (GameObject g in objectsToToggle)
            {
                g.SetActive(true);
            }
            aboutObjects.SetActive(false);
            aboutOn = false;
        }
    }

    public void toggleControls()
    {
        if (!controlsOn)
        {

            foreach (GameObject g in objectsToToggle)
            {
                g.SetActive(false);
            }
            controlsObjects.SetActive(true);
            controlsOn = true;

        }
        else
        {
            foreach (GameObject g in objectsToToggle)
            {
                g.SetActive(true);
            }
            controlsObjects.SetActive(false);
            controlsOn = false;
        }
    }

    IEnumerator fadeOut()
    {
        sceneControlAnim.SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(fadeOutDuration);
        SceneManager.LoadScene(firstLevelString);
    }
}
