using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] AudioClip breakSound = null;
    [SerializeField] GameObject blockSparklesVFX = null;
    [SerializeField] Sprite[] damageSprites = null;

    // Cached references
    Level level = null;
    GameSession theGameSession = null;
    SpriteRenderer mySpriteRenderer = null;

    // state
    [SerializeField] int currentHits = 0;   // only Serialized to view in inspector

    private void Start()
    {
        theGameSession = FindObjectOfType<GameSession>();
        level = FindObjectOfType<Level>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

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

        // Determine if we have another damage level to show
        int spriteIndex = currentHits - 1;
        if ( (damageSprites != null) && (spriteIndex < damageSprites.Length) )
        {
            mySpriteRenderer.sprite = damageSprites[spriteIndex];
        }
        else
        {
            DestroyBlock();
        }
    }

    private void DestroyBlock()
    {
        // modify state
        theGameSession.AddToScore();
        level.BlockBroken();

        // update view
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        TriggerSparklesVFX();
        Destroy(gameObject);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
