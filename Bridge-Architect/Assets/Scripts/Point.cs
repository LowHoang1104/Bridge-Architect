using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Point : MonoBehaviour
{
    public bool runtime = true;
    public Rigidbody2D rb;
    public Vector2 pointId;
    public List<Bar> connectedBars = new();

    // Start is called before the first frame update
    void Start()
    {
        if (!runtime)
        {
            rb.bodyType = RigidbodyType2D.Static;
            pointId = transform.position;
            if (!GameManager.AllPoints.ContainsKey(pointId)) GameManager.AllPoints.Add(pointId, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!runtime)
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                transform.position = Vector3Int.RoundToInt(transform.position);
            }
        }
    }
}
