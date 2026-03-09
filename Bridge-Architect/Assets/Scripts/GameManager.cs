using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelBudget = 1000f;
    public float currentBudget = 0;
    public UIManager myUIManager;
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>();

    public bool gameEnded = false;
    public UIManager uiManager;
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
        myUIManager.UpdateBudgetUI(currentBudget, levelBudget);
    }

    public bool CanBuyItem(float itemCost)
    {
        return itemCost < levelBudget;
    }

    public void UpdateBudget(float itemCost)
    {
        currentBudget -= itemCost;
        myUIManager.UpdateBudgetUI(currentBudget, levelBudget);
    }


    public int CalculateScore()
    {
        float efficiency = currentBudget / levelBudget;
        int score = Mathf.RoundToInt(efficiency * 100);
        return score;
    }

    public int GetStarCount(int score)
    {
        if (score >= 80) return 3;
        if (score >= 50) return 2;
        if (score >= 20) return 1;

        return 0;
    }


    public void GameWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        int score = CalculateScore();
        int stars = GetStarCount(score);

        uiManager.ShowResult(score, stars);

        Time.timeScale = 0;
    }

}
