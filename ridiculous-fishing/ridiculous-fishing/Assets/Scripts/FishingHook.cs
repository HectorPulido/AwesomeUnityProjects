using System.Collections.Generic;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    public enum HookState
    {
        Descending,
        AscendingWithFish,
        ReturningToSurface
    }

    [Header("Hook Settings")]
    public float descendSpeed = 5f;
    public float ascendSpeed = 3f;
    public float returnSpeed = 8f;
    public float horizontalMoveSpeed = 2f;

    [Header("References")]
    public FollowCamera cameraFollower;
    public FishInstancer fishSpawner;
    public GameObject aimingScope;

    [Header("State")]
    public HookState currentState = HookState.Descending;

    private Rigidbody2D rb;
    private readonly List<Fish> caughtFishes = new();
    private float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // If hook is above water and was ascending with fish, transition to return state
        if (transform.position.y > 0 && currentState == HookState.AscendingWithFish)
        {
            currentState = HookState.ReturningToSurface;
            aimingScope.SetActive(true);
            cameraFollower.enabled = false;
            fishSpawner.enabled = false;
            rb.linearVelocity = new Vector2(0, returnSpeed);
            rb.gravityScale = 1f;

            foreach (var fish in caughtFishes)
            {
                fish.Released();
            }
        }

        if (transform.position.y < 0 && currentState == HookState.ReturningToSurface)
        {
            currentState = HookState.Descending;
            aimingScope.SetActive(false);
            cameraFollower.enabled = true;
            fishSpawner.enabled = true;
            rb.gravityScale = 0f;
        }
    }

    private void FixedUpdate()
    {
        float horizontalVelocity = horizontalInput * horizontalMoveSpeed;

        switch (currentState)
        {
            case HookState.Descending:
                rb.linearVelocity = new Vector2(horizontalVelocity, -Mathf.Abs(descendSpeed));
                break;
            case HookState.AscendingWithFish:
                rb.linearVelocity = new Vector2(horizontalVelocity, ascendSpeed);
                break;
                // No movement handling for ReturningToSurface – it's done once in Update.
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == HookState.ReturningToSurface)
            return;

        if (!other.CompareTag("Fish"))
            return;
        var fish = other.GetComponent<Fish>();
        fish.Fished(transform);
        caughtFishes.Add(fish);

        if (currentState != HookState.Descending)
            return;
        currentState = HookState.AscendingWithFish;
        fishSpawner.enabled = false;
    }
}
