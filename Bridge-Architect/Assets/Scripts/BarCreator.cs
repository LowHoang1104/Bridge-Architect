using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarCreator : MonoBehaviour, IPointerDownHandler
{
    public GameManager gameManager;

    public GameObject roadBar;
    public GameObject woodBar;

    bool barCreationStarted = false;

    public Bar currentBar;
    public GameObject barToInstantiate;
    public Transform barParent;

    public Point currentStartPoint;
    public Point currentEndPoint;
    public GameObject pointToInstantiate;
    public Transform pointParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GameOver() || gameManager.winScene) return;
        if (barCreationStarted)
        {
            Vector2 endPosition = (Vector2)Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 dir = endPosition - currentBar.startPosition;

            float maxAllowedLength = currentBar.maxLength;
            if (currentBar.cost > 0f)
            {
                maxAllowedLength = Mathf.Min(currentBar.maxLength, Mathf.Max(0f, gameManager.currentBudget / currentBar.cost));
            }
            Vector2 clampedPosition = currentBar.startPosition + Vector2.ClampMagnitude(dir, maxAllowedLength);

            currentEndPoint.transform.position = (Vector2)Vector2Int.FloorToInt(clampedPosition);
            currentEndPoint.pointId = currentEndPoint.transform.position;
            currentBar.UpdateCreatingBar(currentEndPoint.transform.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameManager.GameOver() || gameManager.winScene) return;

        if (!barCreationStarted)
        {
            barCreationStarted = true;
            StartBarCreation(Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(eventData.position)));
        }
        else
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if(gameManager.CanBuyItem(currentBar.actualCost)) FinishBarCreation();
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                barCreationStarted = false;
                DeleteCurrentBar();
            }
        }
    }

    private void DeleteCurrentBar()
    {
        if (gameManager.GameOver() || gameManager.winScene) return;
        Destroy(currentBar.gameObject);
        if (currentStartPoint.connectedBars.Count == 0 && currentStartPoint.runtime) Destroy(currentStartPoint.gameObject);
        if (currentEndPoint.connectedBars.Count == 0 && currentEndPoint.runtime) Destroy(currentEndPoint.gameObject);
    }

    private void FinishBarCreation()
    {
        if (gameManager.GameOver() || gameManager.winScene) return;
        if (GameManager.AllPoints.ContainsKey(currentEndPoint.transform.position))
        {
            Destroy(currentEndPoint.gameObject);
            currentEndPoint = GameManager.AllPoints[currentEndPoint.transform.position];
        }
        else
        {
            GameManager.AllPoints.Add(currentEndPoint.transform.position, currentEndPoint);
        }

        currentStartPoint.connectedBars.Add(currentBar);
        currentEndPoint.connectedBars.Add(currentBar);

        currentBar.startJoint.connectedBody = currentStartPoint.rb;
        currentBar.startJoint.anchor = currentBar.transform.InverseTransformPoint(currentBar.startPosition);
        currentBar.endJoint.connectedBody = currentEndPoint.rb;
        currentBar.endJoint.anchor = currentBar.transform.InverseTransformPoint(currentEndPoint.transform.position);

        gameManager.UpdateBudget(currentBar.actualCost);

        StartBarCreation(currentEndPoint.transform.position);
    }

    private void StartBarCreation(Vector2 startPosition)
    {
        if (gameManager.GameOver() || gameManager.winScene) return;

        currentBar = Instantiate(barToInstantiate, barParent).GetComponent<Bar>();
        currentBar.startPosition = startPosition;

        if (GameManager.AllPoints.ContainsKey(startPosition))
        {
            currentStartPoint = GameManager.AllPoints[startPosition];
        }
        else
        {
            currentStartPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<Point>();
            GameManager.AllPoints.Add(startPosition, currentStartPoint);
        }

        currentEndPoint = Instantiate(pointToInstantiate, startPosition, Quaternion.identity, pointParent).GetComponent<Point>();
    }
}
