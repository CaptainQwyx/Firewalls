using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] AudioClip breakSound = null;
    [SerializeField] GameObject blockSparklesVFX = null;
    [SerializeField] int maxHits = 0;
    [SerializeField] Sprite[] healthSprites = null;

    // Cached references
    Level level = null;
    GameSession gameSession = null;

    // state
    [SerializeField] int currentHits = 0;   // only Serialized to view in inspector

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        level = FindObjectOfType<Level>();

        if (tag == "Breakable")
        {
            level.IncrementBreakableBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = Math.Min(currentHits, healthSprites.Length);
        GetComponent<SpriteRenderer>().sprite = healthSprites[spriteIndex];
    }

    private void DestroyBlock()
    {
        gameSession.AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        level.BlockBroken();
        TriggerSparklesVFX();
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
