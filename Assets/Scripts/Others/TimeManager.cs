using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    [SerializeField] private float slowdownFactor = 0.05f;
    [SerializeField] private float slowdownLenght = 2f;
    [SerializeField] private Animator postProcessAnimator = null;


    // When this is false 
    bool isWaiting = false;
    void Update()
    {
        if (!isWaiting && !PauseScreen.instance.isPaused) Time.timeScale += (1f / slowdownLenght) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

    }

    public void DoSlowmotion(float length, float transitionDuration)
    {

        postProcessAnimator.SetTrigger("slowDown");

        slowdownLenght = transitionDuration;
        StartCoroutine(WaitSeconds(length));

        Time.timeScale = slowdownFactor;

    }


    IEnumerator WaitSeconds(float s)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(s);
        isWaiting = false;
    }
    

    
}
