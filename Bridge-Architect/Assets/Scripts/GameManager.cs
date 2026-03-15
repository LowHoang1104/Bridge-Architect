using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelBudget = 1000f;
    public float currentBudget = 0;
    public UIManager myUIManager;
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>();
    public bool winScene = false;
    
    public int woodBarsUsed = 0;
    public int ironBarsUsed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        AllPoints.Clear();
        Time.timeScale = 0;
        currentBudget = levelBudget;
        woodBarsUsed = 0;
        ironBarsUsed = 0;
        myUIManager.UpdateBudgetUI(currentBudget, levelBudget);
    }

    public bool CanBuyItem(float itemCost)
    {
        return itemCost <= currentBudget;
    }

    public void UpdateBudget(float itemCost)
    {
        currentBudget -= itemCost;
        myUIManager.UpdateBudgetUI(currentBudget, levelBudget);
    }

    public void AddMaterialUsage(string materialType)
    {
        if (materialType == "wood")
        {
            woodBarsUsed++;
        }
        else if (materialType == "iron")
        {
            ironBarsUsed++;
        }
    }

    public bool GameOver()
    {
        return currentBudget <= 0;
    }

    public void WinGame()
    {
        int achievedStars = GetStars();
            Debug.Log("Stars: " + achievedStars);
        myUIManager.ShowStars(achievedStars);
        myUIManager.ShowMaterialStats(woodBarsUsed, ironBarsUsed);
        
        winScene = true;
    }

    public int GetStars()
    {
        float usedBudget = levelBudget - currentBudget;
        float onePart = levelBudget / 4f;

        if (usedBudget <= 2f * onePart)
        {
            return 3;
        }
        else if (usedBudget <= 3f * onePart)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
