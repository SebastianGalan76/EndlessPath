using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Slider slider;

    private Player player;

    private float deltaX;
    private Touch touch;
    private Vector2 touchPos;

    private float playerSpeed = 2.25f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    void Update() {
        if(player.IsDead() || player.system.GetPause()) {
            return;
        }

        MoveForward();

        if(Application.platform != RuntimePlatform.Android) {
            transform.position = new Vector2(slider.value, transform.position.y);
        } else {
            if(Input.touchCount == 1) {
                touch = Input.GetTouch(0);

                touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                switch(touch.phase) {
                    case TouchPhase.Began:
                        deltaX = touchPos.x - transform.position.x;
                        break;
                    case TouchPhase.Moved:
                        if(touchPos.x - deltaX > -3.8f && touchPos.x - deltaX < 3.8f) {
                            transform.position = new Vector2(touchPos.x - deltaX, transform.position.y);
                        } else {
                            player.Die();
                        }
                        break;
                    case TouchPhase.Ended:
                        deltaX = 0;
                        break;
                }
            }
        }

        void MoveForward() {
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1), playerSpeed * Time.deltaTime);
        }
    }

    //Changes the speed of the player
    public void ChangePlayerSpeed(bool increase, float value) {
        if(increase) { playerSpeed += value; } else {
            playerSpeed = value;
        }
    }
    public void ResetSlider() {
        slider.value = 0;
    }

    public float GetPlayerSpeed() {
        return playerSpeed;
    }
}
