using UnityEngine;

public class CameraSystem : MonoBehaviour {
    [SerializeField] private GameSystem system;
    [SerializeField] private Player player;

    private float speed;

    private void Update() {
        //Follow the player object
        if(!system.GetPause() && !player.IsDead()) {
            transform.position = new Vector3(0, player.GetPlayerPostion().y - 1, -10);
            return;
        }

        if(player.IsDead()) {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1), speed * Time.deltaTime);
            return;
        }
    }

    public void ChangeSpeed(float speed) {
        this.speed = speed;
    }
}
