using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{

    public float cost = 1f;
    public float maxLength = 1f;
    public Vector2 startPosition;
    public SpriteRenderer barSpriteRender;

    public BoxCollider2D boxCollider;
    public HingeJoint2D startJoint;
    public HingeJoint2D endJoint;

    float startJointCurrentLoad = 0f;
    float endJointCurrentLoad = 0f;
    MaterialPropertyBlock propBlock;
    public float actualCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Time.timeScale == 1) UpdateMaterial();
    }

    void OnMouseEnter()
    {
        barSpriteRender.color = Color.yellow;
    }

    void OnMouseExit()
    {
        barSpriteRender.color = Color.white;
    }

    public void UpdateCreatingBar(Vector2 toPosition)
    {
        transform.position = (toPosition + startPosition) / 2;

        Vector2 dir = toPosition - startPosition;
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        float lengthRoad = dir.magnitude;
        barSpriteRender.size = new Vector2(lengthRoad, barSpriteRender.size.y);

        boxCollider.size = barSpriteRender.size;

        actualCost = lengthRoad * cost;
    }

    public void UpdateMaterial()
    {
        if (startJoint != null) startJointCurrentLoad = startJoint.reactionForce.magnitude / startJoint.breakForce;
        if (endJoint != null) endJointCurrentLoad = endJoint.reactionForce.magnitude / endJoint.breakForce;
        float maxLoad = Mathf.Max(startJointCurrentLoad, endJointCurrentLoad);

        propBlock = new MaterialPropertyBlock();
        barSpriteRender.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_Load", maxLoad);
        barSpriteRender.SetPropertyBlock(propBlock);
    }
}
