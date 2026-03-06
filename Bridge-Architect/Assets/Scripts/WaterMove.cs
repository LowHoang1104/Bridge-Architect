using UnityEngine;

public class WaterMove : MonoBehaviour
{
    public float speed = 0.3f;
    private Material mat;
    private float offset;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
    }

    void Update()
    {
        offset += speed * Time.deltaTime;
        if (offset > 1f)
            offset -= 1f;

        mat.mainTextureOffset = new Vector2(offset, 0);
    }
}