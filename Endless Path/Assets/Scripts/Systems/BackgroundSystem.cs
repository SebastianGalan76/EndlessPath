using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSystem : MonoBehaviour {
    [SerializeField] private new BackgroundParticlesSystem particleSystem;

    [SerializeField] private Image[] borders;
    [SerializeField] private Image[] imagesUI;

    [SerializeField] private SpriteRenderer effect;

    private GameColor currentColor;
    private GameColor previousColor;

    private SpriteRenderer background;
    private Animator animator;

    private List<GameColor> gameColors;

    //Speed of changing background color.
    private float speed = 2f;
    private float startTime;

    private int changeColorPhase;

    private void Awake() {
        background = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        gameColors = new List<GameColor> {
            new GameColor("#5193E9FF", "#5193E9C8"),
            new GameColor("#48CCE7FF", "#6DC6DFC8"),
            new GameColor("#3FD66BFF", "#6CDF9EC8"),
            new GameColor("#92D755FF", "#B7DD6FC8"),
            new GameColor("#DCD95BFF", "#DCDA70C8"),
            new GameColor("#F38B45FF", "#EC975FC8"),
            new GameColor("#E85949FF", "#E77365C8"),
            new GameColor("#D05ABAFF", "#CE7EC1C8"),
            new GameColor("#9753E5FF", "#A165E7C8")
        };

        currentColor = gameColors.First();
    }

    private void Update() {
        if(changeColorPhase == 1) {
            ChangeEffect();
        }
        if(changeColorPhase == 2) {
            ChangeBackground();
        }

        void ChangeEffect() {
            float t = (Time.time - startTime) * speed;

            if(t >= 1) {
                t = 1;
            }
            if(t > 0.8 && changeColorPhase == 1) {
                changeColorPhase = 2;
            }

            effect.color = Color.Lerp(previousColor.mainColor, currentColor.mainColor, t);
        }
        void ChangeBackground() {
            float t = (Time.time - startTime) * 1f;

            if(t >= 1) {
                changeColorPhase = 0;
                t = 1;
            }

            Color color = Color.Lerp(previousColor.mainColor, currentColor.mainColor, t);

            background.color = color;
            ChangeBorderColor(color);
        }
    }

    public void ChangeBackgroundColor() {
        previousColor = currentColor;
        currentColor = GetNewColor();
        ChangeUIColor();

        startTime = Time.time;
        changeColorPhase = 1;

        animator.Play("ChangeColor");
        
        GameColor GetNewColor() {
            GameColor newColor;
            do {
                newColor = gameColors[Random.Range(0, gameColors.Count)];
            } while(newColor.Equals(previousColor));

            return newColor;
        }
    }

    public void ChangeParticlesVisibility(bool value) {
        gameObject.transform.Find("BackgroundParticles").gameObject.SetActive(value);
    }

    public void ChangeParticles() {
        particleSystem.GenerateParticles();
    }


    private void ChangeBorderColor(Color color) {
        foreach(Image i in borders) {
            i.color = color;
        }
    }
    private void ChangeUIColor() {
        imagesUI[0].color = currentColor.lightColor;

        for(int i = 1;i < imagesUI.Length;i++) {
            imagesUI[i].color = currentColor.mainColor;
        }
    }
}
