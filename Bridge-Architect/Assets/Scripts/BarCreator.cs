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

            //cost bar preview
            gameManager.myUIManager.UpdateBarCostPreview(currentBar.actualCost,Input.mousePosition);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameManager.GameOver() || gameManager.winScene) return;

        //delete bar with right click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (barCreationStarted)
            {
                barCreationStarted = false;
                DeleteCurrentBar();
            }
            else
            {
                TryDeleteBar(eventData.position);
            }
            return;
        }

        if (!barCreationStarted)
        {
            barCreationStarted = true;
            StartBarCreation(Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(eventData.position)));
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (gameManager.CanBuyItem(currentBar.actualCost))
                    FinishBarCreation();
            }
        }
    }

    // Try to delete a bar at the given mouse position
    void TryDeleteBar(Vector2 mousePos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            Bar bar = hit.collider.GetComponent<Bar>();

            if (bar != null)
            {
                DeleteBar(bar);
            }
        }
    }

    // Delete the given bar and handle all related cleanup
    void DeleteBar(Bar bar)
    {
        Point start = bar.startJoint.connectedBody.GetComponent<Point>();
        Point end = bar.endJoint.connectedBody.GetComponent<Point>();

        if (start != null) start.connectedBars.Remove(bar);
        if (end != null) end.connectedBars.Remove(bar);

        gameManager.RefundBudget(bar.actualCost);

        Destroy(bar.gameObject);

        if (start != null && start.connectedBars.Count == 0 && start.runtime)
        {
            GameManager.AllPoints.Remove(start.pointId);
            Destroy(start.gameObject);
        }

        if (end != null && end.connectedBars.Count == 0 && end.runtime)
        {
            GameManager.AllPoints.Remove(end.pointId);
            Destroy(end.gameObject);
        }
    }

    private void DeleteCurrentBar()
    {
        if (gameManager.GameOver() || gameManager.winScene) return;
        Destroy(currentBar.gameObject);
        if (currentStartPoint.connectedBars.Count == 0 && currentStartPoint.runtime) Destroy(currentStartPoint.gameObject);
        if (currentEndPoint.connectedBars.Count == 0 && currentEndPoint.runtime) Destroy(currentEndPoint.gameObject);

        // cost bar preview
        gameManager.myUIManager.HideBarCostPreview();
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

        //cost bar preview
        gameManager.myUIManager.HideBarCostPreview();

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
