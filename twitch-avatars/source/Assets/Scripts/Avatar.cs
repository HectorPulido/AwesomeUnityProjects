using System.Collections;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Security;

public class Avatar : MonoBehaviour
{
    [Header("Avatar settings")]
    public TMP_Text avatarName;

    [Header("Avatar aparience")]
    public SpriteRenderer avatarSprite;
    public SpriteRenderer accessorySprite;

    [Header("Commands")]
    public GameObject heart;

    [Header("Avatar movement")]
    public float maxDistanceToMove = 0.25f;
    public float maxTimeToMove = 1f;
    public float velocity = 1f;
    public float jumpForce = 1f;
    public Bumping bumping;
    public GameObject ghost;
    public Collider2D collider2D;
    public GameObject knife;

    [Header("Avatar emotions")]
    public Sprite[] emotionalSprites;
    public SpriteRenderer emotionalSpriteRenderer;
    public SpriteRenderer emotionalGhostSpriteRenderer;
    public int EmotionalCount => emotionalSprites.Length;

    [Header("UI")]
    public Transform canvas;
    public GameObject[] objectsToIgnore;
    public float canvasSeparation = 0.75f;
    public float raycastXOffset = 0.5f;
    public float raycastDistance = 50f;
    public float raycastYOffset = 5.5f;

    private float canvasInitialY;

    private float targetMovement;
    private Rigidbody2D rb;
    private bool canMove = true;

    [HideInInspector]
    public Vector3 originalScale;

    private bool IsAlive { get { return !ghost.activeSelf; } }
    private bool IsHidden { get { return !bumping.gameObject.activeInHierarchy; } }

    [ContextMenu("SetGhost")]
    public void SetGhost()
    {
        SetGhost(!ghost.activeSelf);
    }

    public void SetKnife(bool active)
    {
        knife.SetActive(active);
    }

    public void SetGhost(bool active)
    {
        if (IsHidden){
            return;
        }

        if (active)
        {
            ConnectTest.singleton.HandleDie(this);
        }

        rb.gravityScale = active ? 0 : 1;
        collider2D.enabled = !active;

        ghost.SetActive(active);
        bumping.gameObject.SetActive(!active);
    }

    public void SetHidden(bool active)
    {
        bumping.gameObject.SetActive(!active);
    }

    public void SetEmotion(int emotionIndex)
    {
        emotionalSpriteRenderer.sprite = emotionalSprites[emotionIndex];
        emotionalGhostSpriteRenderer.sprite = emotionalSprites[emotionIndex];
    }

    public void SetColor(string color)
    {
        if (!ConfigData.gameData.colors.ContainsKey(color))
        {
            return;
        }
        avatarSprite.color = ConfigData.gameData.colors[color];
        ConfigData.saveData.AddColor(name, color);
    }

    public void SetName(string name)
    {
        this.name = name;
        avatarName.text = name;
    }

    public void Setup(string name)
    {
        // Set data
        SetName(name);
        SetSprites();

        // Initial config
        originalScale = transform.localScale;
        canvasInitialY = canvas.transform.localPosition.y;

        // Start cycles
        StartCoroutine(NameCheckCycle());
        StartCoroutine(MoveCycle());
        StartCoroutine(EmotionalCycle());

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    public void RandomizeSprites()
    {
        ConfigData.saveData.ClearUser(name);
        SetSprites();
    }

    private void SetSprites()
    {
        // Set avatar sprites
        var avatarSpriteInt = ConfigData.saveData.GetAvatar(name);
        if (avatarSpriteInt == null)
        {
            avatarSpriteInt = Random.Range(0, ConfigData.gameData.defaultAvatar.Length);
            ConfigData.saveData.AddAvatar(name, avatarSpriteInt.Value);
        }

        var avatarSpriteTexture = ConfigData.gameData.defaultAvatar[avatarSpriteInt.Value];
        if (ConfigData.gameData.avatars.ContainsKey(name))
        {
            avatarSpriteTexture = ConfigData.gameData.avatars[name];
        }

        var accessorySpriteInt = ConfigData.saveData.GetAccessory(name);
        if (accessorySpriteInt == null)
        {
            accessorySpriteInt = Random.Range(0, ConfigData.gameData.defaultAvatarAccessory.Length);
            ConfigData.saveData.AddAccessory(name, accessorySpriteInt.Value);
        }

        var accessorySpriteTexture = ConfigData.gameData.defaultAvatarAccessory[accessorySpriteInt.Value];

        avatarSprite.sprite = ConfigData.Texture2DToSprite(avatarSpriteTexture, 350, new Vector2(0.5f, 0));
        accessorySprite.sprite = ConfigData.Texture2DToSprite(accessorySpriteTexture, 350, new Vector2(0.5f, 0));

        // Set avatar color randomly
        var colorStr = ConfigData.saveData.GetColor(name);
        if (colorStr == null)
        {
            var colorKeys = ConfigData.gameData.colors.Keys.ToArray();
            colorStr = colorKeys[Random.Range(0, colorKeys.Length)];
        }
        SetColor(colorStr);
    }

    private IEnumerator EmotionalCycle()
    {
        while (true)
        {
            SetEmotion(Random.Range(0, EmotionalCount));
            yield return new WaitForSeconds(Random.Range(5, 20));
        }
    }

    private IEnumerator MoveCycle()
    {
        while (true)
        {
            targetMovement = transform.position.x + Random.Range(-maxDistanceToMove, maxDistanceToMove);
            float time = Random.Range(0, maxTimeToMove);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator NameCheckCycle()
    {
        while (true)
        {
            var moveCanvas = false;

            var raycastXVOffset = Vector3.left * raycastXOffset;
            var raycastYVOffset = Vector3.up * raycastYOffset;
            var raycastStart = transform.position + raycastYVOffset;

            //debug the raycast
            Debug.DrawRay(raycastStart, Vector2.down * raycastDistance, Color.red);
            Debug.DrawRay(raycastStart - raycastXVOffset, Vector2.down * raycastDistance, Color.red);
            Debug.DrawRay(raycastStart + raycastXVOffset, Vector2.down * raycastDistance, Color.red);
            Debug.DrawRay(raycastStart - raycastXVOffset * 0.5f, Vector2.down * raycastDistance, Color.red);
            Debug.DrawRay(raycastStart + raycastXVOffset * 0.5f, Vector2.down * raycastDistance, Color.red);
            // create a raycast from up the avatar to the ground
            RaycastHit2D[] hits1 = Physics2D.RaycastAll(raycastStart - raycastXVOffset * 0.5f, Vector2.down, raycastDistance);
            RaycastHit2D[] hits2 = Physics2D.RaycastAll(raycastStart + raycastXVOffset * 0.5f, Vector2.down, raycastDistance);
            RaycastHit2D[] hits3 = Physics2D.RaycastAll(raycastStart - raycastXVOffset, Vector2.down, raycastDistance);
            RaycastHit2D[] hits4 = Physics2D.RaycastAll(raycastStart + raycastXVOffset, Vector2.down, raycastDistance);
            RaycastHit2D[] hits5 = Physics2D.RaycastAll(raycastStart, raycastXVOffset, raycastDistance);

            var hits = hits1.Concat(hits2).Concat(hits3).Concat(hits4).Concat(hits5).ToArray();

            var maxHeigth = float.MinValue;

            for (int i = 0; i < hits.Length; i++)
            {
                if (objectsToIgnore.Contains(hits[i].collider.gameObject))
                {
                    continue;
                }
                if (hits[i].collider.transform.position.x >= canvas.position.x)
                {
                    continue;
                }

                moveCanvas = true;
                Debug.DrawLine(raycastStart, hits[i].point, Color.green);

                if (hits[i].point.y > maxHeigth)
                {
                    maxHeigth = hits[i].point.y;
                }
            }

            if (moveCanvas)
            {
                canvas.position = new Vector2(canvas.position.x, maxHeigth + canvasSeparation);
            }
            else
            {
                canvas.localPosition = new Vector2(canvas.localPosition.x, canvasInitialY);
            }

            yield return null;
        }
    }

    public void Jump()
    {
        if (!canMove)
        {
            return;
        }

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void ShowHeart()
    {
        heart.SetActive(true);
        StartCoroutine(WaitForCallback.WaitForSecondsCallback(5f, () => heart.SetActive(false)));
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -100)
        {
            transform.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }

        if (!IsAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!canMove)
        {
            return;
        }

        bumping.moving = Mathf.Abs(rb.velocity.x) > 0.2;
        bumping.transform.eulerAngles = new Vector3(bumping.transform.eulerAngles.x, rb.velocity.x > 0 ? 180 : 0, bumping.transform.eulerAngles.z);

        if (Mathf.Abs(transform.position.x - targetMovement) > 0.1f)
        {
            var direction = Mathf.Sign(targetMovement - transform.position.x);
            rb.velocity = new Vector2(direction * velocity, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Explosion()
    {
        canMove = false;
        StartCoroutine(WaitForCallback.WaitForSecondsCallback(5f, () => canMove = true));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Knifu"))
        {
            SetGhost(true);
        }
    }

}
