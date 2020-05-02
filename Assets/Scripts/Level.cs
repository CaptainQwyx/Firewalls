using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] int breakableBlocks = 0;  // Serialized for debugging purposes

    // Cached references
    SceneLoader sceneLoader = null;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void IncrementBreakableBlocks()
    {
        breakableBlocks++;
    }

    public void BlockBroken()
    {
        breakableBlocks--;
        if (breakableBlocks <= 0)
        {
            sceneLoader.LoadNextScene();
        }
    }
}
