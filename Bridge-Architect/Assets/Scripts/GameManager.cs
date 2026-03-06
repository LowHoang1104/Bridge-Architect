using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelBudget = 1000f;
    public float currentBudget = 0;
    public UIManager myUIManager;
    public static Dictionary<Vector2, Point> AllPoints = new Dictionary<Vector2, Point>();
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
    
}
