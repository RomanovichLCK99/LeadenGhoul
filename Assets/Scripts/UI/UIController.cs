using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    #region Singleton
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject containerParent;
    public Color fullHeartColor;
    public Color emptyHeartColor;
    public int numOfHearts;
    [Space(10)]
    public Slider slowDownSlider;
    public float maxSlowDown = 25f;
    public float minSlowDown = 0f;
    public float numberOfSlowDwns = 5f;
    [Space(10)]
    public TextMeshProUGUI enemyCounter;

    public int health = 5;
    [HideInInspector] public int enemyCount = 0;
    [HideInInspector] public float slowDownValue = 5f;

    private Image[] heartContainers;

    private void Start()
    {
        heartContainers = containerParent.GetComponentsInChildren<Image>();
        slowDownSlider.minValue = minSlowDown;
        slowDownSlider.maxValue = maxSlowDown;
        
    }
    private void Update()
    {
        enemyCounter.text = "x " + enemyCount.ToString();


        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < health)
            {          
                heartContainers[i].color = fullHeartColor;    
            } else
            {
                heartContainers[i].color = emptyHeartColor;
            }

            // Disable heart containers depending on number of hearts
            if (i < numOfHearts)
            {
                heartContainers[i].enabled = true;
            }
            else
            {
                heartContainers[i].enabled = false;
            }

        }

        slowDownValue = Mathf.Clamp(slowDownValue, minSlowDown, maxSlowDown);
        slowDownSlider.value = slowDownValue;
        
    }

}
