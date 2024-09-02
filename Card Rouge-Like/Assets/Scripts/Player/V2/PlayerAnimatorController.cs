using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [System.Serializable]
    public class CosmeticDataWrapper
    {
        public CosmeticData cosmeticData;
        public SpriteRenderer spriteRenderer;
        public int currentFrame;
        public Sprite[] currentAnimation;
        public bool[] currentFlipX;
    }

    public List<CosmeticDataWrapper> cosmetics = new List<CosmeticDataWrapper>();

    public float framesPerSecond = 10f; // Number of frames per second

    private float timer;

    private PlayerMovement2 playerMovement;

    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement2>();
        timer = 0f;

        foreach (var cosmetic in cosmetics)
        {
            cosmetic.currentFrame = 0;
            UpdateCurrentAnimation(cosmetic);
        }
    }

    void Update()
    {
        foreach (var cosmetic in cosmetics)
        {
            UpdateCurrentAnimation(cosmetic);
        }

        timer += Time.deltaTime;

        if (timer >= 1f / framesPerSecond)
        {
            timer -= 1f / framesPerSecond;

            foreach (var cosmetic in cosmetics)
            {
                if (cosmetic.currentAnimation == null || cosmetic.currentAnimation.Length == 0) continue;

                cosmetic.currentFrame = (cosmetic.currentFrame + 1) % cosmetic.currentAnimation.Length;
                cosmetic.spriteRenderer.sprite = cosmetic.currentAnimation[cosmetic.currentFrame];
                cosmetic.spriteRenderer.flipX = cosmetic.currentFlipX[cosmetic.currentFrame];
            }
        }
    }

    private void UpdateCurrentAnimation(CosmeticDataWrapper cosmetic)
    {
        Vector2 movement = playerMovement.movement;
        Vector2 lastMovement = new Vector2(playerMovement.LastHorizontal, playerMovement.LastVertical);

        if (movement == Vector2.zero)
        {
            if (lastMovement.x > 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.idleRight;
                cosmetic.currentFlipX = cosmetic.cosmeticData.idleRightFlipX;
            }
            else if (lastMovement.x < 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.idleLeft;
                cosmetic.currentFlipX = cosmetic.cosmeticData.idleLeftFlipX;
            }
            else if (lastMovement.y > 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.idleUp;
                cosmetic.currentFlipX = cosmetic.cosmeticData.idleUpFlipX;
            }
            else if (lastMovement.y < 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.idleDown;
                cosmetic.currentFlipX = cosmetic.cosmeticData.idleDownFlipX;
            }
        }
        else
        {
            if (movement.x > 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.runRight;
                cosmetic.currentFlipX = cosmetic.cosmeticData.runRightFlipX;
            }
            else if (movement.x < 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.runLeft;
                cosmetic.currentFlipX = cosmetic.cosmeticData.runLeftFlipX;
            }
            else if (movement.y > 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.runUp;
                cosmetic.currentFlipX = cosmetic.cosmeticData.runUpFlipX;
            }
            else if (movement.y < 0)
            {
                cosmetic.currentAnimation = cosmetic.cosmeticData.runDown;
                cosmetic.currentFlipX = cosmetic.cosmeticData.runDownFlipX;
            }
        }
    }
}
