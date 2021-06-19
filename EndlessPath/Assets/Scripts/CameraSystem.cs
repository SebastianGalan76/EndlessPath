using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform player;
    public GameSystem system;
    public float playerSpeed;

    private void Update()
    {
        //Follow the player object
        if (!system.getPause() && !system.getDead())
        {
            transform.position = new Vector3(0, player.position.y-1, -10);
        }

        if (system.getDead())
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1), playerSpeed * Time.deltaTime);
        }
    }
}
