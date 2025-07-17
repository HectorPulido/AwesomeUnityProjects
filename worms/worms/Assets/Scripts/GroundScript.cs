using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GroundScript : MonoBehaviour
{
    public Texture2D baseTexture;
    public float WidthWorld
    {
        get
        {
            if (widthWorld == 0)
                widthWorld = sr.bounds.size.x;
            return widthWorld;
        }

    }
    public float HeightWorld
    {
        get
        {
            if (heightWorld == 0)
                heightWorld = sr.bounds.size.y;
            return heightWorld;
        }

    }
    public int WidthPixel
    {
        get
        {
            if (widthPixel == 0)
                widthPixel = sr.sprite.texture.width;

            return widthPixel;
        }
    }
    public int HeightPixel
    {
        get
        {
            if (heightPixel == 0)
                heightPixel = sr.sprite.texture.height;

            return heightPixel;
        }
    }

    private Texture2D cloneTexture;
    private SpriteRenderer sr;

    private float widthWorld, heightWorld;
    private int widthPixel, heightPixel;

    private BoxCollider2D boxCol;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cloneTexture = Instantiate(baseTexture);

        if (cloneTexture.format != TextureFormat.ARGB32)
            Debug.LogWarning("Texture must be ARGB32");
        if (cloneTexture.wrapMode != TextureWrapMode.Clamp)
            Debug.LogWarning("wrapMode must be Clamp");

        UpdateTexture();
        boxCol = gameObject.AddComponent<BoxCollider2D>();
        boxCol.compositeOperation = Collider2D.CompositeOperation.Merge;
    }

    private void MakeAHole(CircleCollider2D col)
    {
        print(string.Format("{0},{1},{2},{3}", WidthPixel, HeightPixel, WidthWorld, heightWorld));

        Vector2Int c = World2Pixel(col.bounds.center);
        int r = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld);

        int px, nx, py, ny, d;
        for (int i = 0; i <= r; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - i * i));
            for (int j = 0; j <= d; j++)
            {
                px = c.x + i;
                nx = c.x - i;
                py = c.y + j;
                ny = c.y - j;

                cloneTexture.SetPixel(px, py, Color.clear);
                cloneTexture.SetPixel(nx, py, Color.clear);
                cloneTexture.SetPixel(px, ny, Color.clear);
                cloneTexture.SetPixel(nx, ny, Color.clear);
            }
        }
        cloneTexture.Apply();
        UpdateTexture();

        float holeRadius = col.radius * Mathf.Max(col.transform.lossyScale.x, col.transform.lossyScale.y);
        var hole = gameObject.AddComponent<CircleCollider2D>();
        hole.compositeOperation = Collider2D.CompositeOperation.Difference;
        hole.compositeOrder = 10;
        hole.radius = holeRadius * 1.5f;

        Vector2 worldCenter = col.transform.TransformPoint(col.offset);
        Vector2 localCenter = boxCol.transform.InverseTransformPoint(worldCenter);
        hole.offset = localCenter - Vector2.up * 0.25f;
    }


    private void UpdateTexture()
    {
        sr.sprite = Sprite.Create(cloneTexture,
                            new Rect(0, 0, cloneTexture.width, cloneTexture.height),
                            new Vector2(0.5f, 0.5f),
                            50f
                            );
    }

    private Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        var dx = (pos.x - transform.position.x);
        var dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Explosion"))
            return;
        var circularCollider = collision.GetComponent<CircleCollider2D>();
        if (!circularCollider)
            return;

        MakeAHole(circularCollider);
        Destroy(collision.gameObject, 0.1f);
    }

}
