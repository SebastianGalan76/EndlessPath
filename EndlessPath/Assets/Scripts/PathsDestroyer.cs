using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PathsDestroyer : MonoBehaviour
{
    public GameSystem game;

    //Destroy old path and load next path
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "pathMaterial")
        {
            game.loadNextPaths();
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
