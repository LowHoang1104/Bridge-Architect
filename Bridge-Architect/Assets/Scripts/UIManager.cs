using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button roadButton;
    public Button woodButton;
    public BarCreator barCreator;

    public Slider budgetSlider;
    public TextMeshProUGUI BudgetText;
    public TextMeshProUGUI materialStatsText;
    public GameObject materialStatsPanel;
    public TextMeshProUGUI stressIndicatorText;
    public GameObject stressIndicatorPanel;
    private Bar previouslyHighlightedBar;

    public Gradient myGradient;

    public GameObject[] starImages;

    public Image star1;
    public Image star2;
    public Image star3;

    public Sprite starFull;
    public Sprite starEmpty;
    public GameObject resultPanel;

    // Start is called before the first frame update
    void Start()
    {
        roadButton.onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        Time.timeScale = 1;
    }

    public void Restart(int map)
    {
        SceneManager.LoadScene("Map" + map);
    }

    public void ChangeBar(int barType)
    {
        if(barType == 0)
        {
            roadButton.GetComponent<Outline>().enabled = true;
            woodButton.GetComponent<Outline>().enabled = false;
            barCreator.barToInstantiate = barCreator.roadBar;
        }
        if (barType == 1)
        {
            woodButton.GetComponent<Outline>().enabled = true;
            roadButton.GetComponent<Outline>().enabled = false;
            barCreator.barToInstantiate = barCreator.woodBar;
        }
    }

    public void UpdateBudgetUI(float currentBudget, float levelBudget)
    {
        BudgetText.text = Mathf.FloorToInt(currentBudget).ToString() + "$";
        budgetSlider.value = currentBudget / levelBudget;
        budgetSlider.fillRect.GetComponent<Image>().color = myGradient.Evaluate(budgetSlider.value);
    }

        public void ShowStars(int starCount)
    {
        resultPanel.SetActive(true);
        resultPanel.transform.SetAsLastSibling();
        star1.sprite = (starCount >= 1) ? starFull : starEmpty;
        star2.sprite = (starCount >= 2) ? starFull : starEmpty;
        star3.sprite = (starCount >= 3) ? starFull : starEmpty;
    }

    public void ShowMaterialStats(int woodCount, int ironCount)
    {
        if (materialStatsPanel != null && materialStatsText != null)
        {
            materialStatsPanel.SetActive(true);
            materialStatsText.text = $"Materials Used:\n Wood: {woodCount}\n Iron: {ironCount}";
        }
    }

    public void HighlightWeakestBar(Bar weakestBar)
    {
        if (previouslyHighlightedBar != null && previouslyHighlightedBar != weakestBar)
        {
            previouslyHighlightedBar.ResetHighlight();
        }

        if (weakestBar != null)
        {
            weakestBar.HighlightAsWeakest();
            previouslyHighlightedBar = weakestBar;
            
            if (stressIndicatorPanel != null && stressIndicatorText != null)
            {
                stressIndicatorPanel.SetActive(true);
                stressIndicatorText.text = "Weakest Bar Under Stress!";
                stressIndicatorText.color = Color.red;
            }
        }
        else
        {
            if (stressIndicatorPanel != null)
            {
                stressIndicatorPanel.SetActive(false);
            }
        }
    }
}
