using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSystem : MonoBehaviour
{
    public GameObject[] UIBorders;
    public GameObject EffectObject;

    public GameObject[] UIObjectsWithColor;

    //List of available colors
    public Color32[] colors;
    public Color32[] lightColors;

    private Color32 currentColor;
    private int currentColorID = 0;

    //Speed of changing background color.
    private float speed = 2f;
    private float startTime;
    private int changeColorPhase;

    private void Start()
    {
        currentColorID = 0;
    }

    private void Update()
    {
        if (changeColorPhase==1) {
            changeColor();
        }
        if (changeColorPhase == 2) {
            changeBackground();
        }

        void changeBackground() {
            float t = (Time.time - startTime) * 1f;

            if (t >= 1)
            {
                changeColorPhase = 0;
                t = 1;
            }

            setColor(gameObject, Color32.Lerp(currentColor, colors[currentColorID], t));
            for (int i = 0; i < 2; i++) {
                setColor(UIBorders[i], Color32.Lerp(currentColor, colors[currentColorID], t));
            }
        }
        void changeColor() {
            float t = (Time.time - startTime) * speed;

            if (t >= 1) {
                t = 1;
            }
            if (t > 0.8 && changeColorPhase == 1) {
                changeColorPhase = 2;
            }

            setColor(EffectObject, Color32.Lerp(currentColor, colors[currentColorID], t));
        }
    }

    public void changeColor() {
        currentColorID = getColorID();

        startTime = Time.time;
        currentColor = getCurrentColor();
        changeColorPhase = 1;

        GetComponent<Animator>().Play("ChangeColor");
        changeUIColor();

        int getColorID() {
            RandomAgain:
            int rand = Random.Range(0, colors.Length);
            if (rand == currentColorID || Mathf.Abs(currentColorID-rand)<=4) {
                goto RandomAgain;
            }

            return rand;
        }
    }

    public void changeUIColor() {
        setColor(UIObjectsWithColor[0], lightColors[currentColorID]);
        for (int i = 1; i < UIObjectsWithColor.Length; i++) {
            setColor(UIObjectsWithColor[i], colors[currentColorID]);
        }
    }

    private void setColor(GameObject obj, Color32 color) {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            obj.GetComponent<SpriteRenderer>().color = color;
        }
        else if(obj.GetComponent<Image>()!=null){
            obj.GetComponent<Image>().color = color;
        }
    }
    public Color32 getCurrentColor() {
        return gameObject.GetComponent<SpriteRenderer>().color;
    }
    public Color32 getColorWithID(bool lightColor) {
        if (!lightColor)
        {
            return colors[currentColorID];
        }
        else {
            return lightColors[currentColorID];
        }
    }
    public void setActiveParticles(bool value)
    {
        gameObject.transform.Find("BackgroundParticles").gameObject.SetActive(value);
    }
}
