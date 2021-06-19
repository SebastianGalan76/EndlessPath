using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public class Path : MonoBehaviour
{
    //Object position
    public GameObject decorations;
    public GameObject coins;

    public void generateDecoration(GameObject decorationP, GameObject shadowP) {
        if(decorations == null) { return; }

        int randomQuantity = Random.Range(0, 100);
        if (randomQuantity < 30)
        {
            randomQuantity = 1;
        }
        else if (randomQuantity > 60)
        {
            randomQuantity = 3;
        }
        else {
            randomQuantity = 2;
        }

        int[] randomPositions = new int[randomQuantity];

        for (int i = 0; i < randomQuantity; i++) {
            RandomAgain:
            int randomPosition = Random.Range(1, decorations.transform.childCount + 1);
            for (int y = 0; y < randomQuantity; y++) {
                if (randomPosition == randomPositions[y]) {
                    goto RandomAgain;
                }
            }

            randomPositions[i] = randomPosition;

            //Object rotation
            int randomRotation = Random.Range(0, 360);

            //Decoration
            GameObject dec = Instantiate(decorationP);
            dec.transform.position = decorations.transform.GetChild(randomPosition - 1).transform.position;
            dec.transform.parent = transform;
            dec.transform.rotation = new Quaternion(0, 0, randomRotation, 360);

            //Shadow of decoration
            GameObject sha = Instantiate(shadowP);
            sha.transform.position = decorations.transform.GetChild(randomPosition - 1).transform.position + new Vector3(0.1f, -0.1f, 0);
            sha.transform.parent = transform;
            sha.transform.rotation = new Quaternion(0, 0, randomRotation, 360);
        }
    }
    public void generateCoinsOrGift(GameObject coin, GameObject gift) {
        if (coins == null) { return; }

        int randomQuantity = Random.Range(0, 100);
        if (randomQuantity < 10)
        {
            randomQuantity = 2;
        }
        else if (randomQuantity > 60) {
            randomQuantity = 1;
        }
        else if (randomQuantity == 60 || randomQuantity == 59) {
            Debug.Log("CreateGift!");
            GameObject giftObj = Instantiate(gift);
            giftObj.name = "Gift";
            giftObj.transform.position = coins.transform.GetChild(0).transform.position;
            giftObj.transform.parent = transform;
            return;
        }
        else
        {
            return;
        }

        int[] randomPositions = new int[randomQuantity];
        for (int i = 0; i < randomQuantity; i++)
        {
        RandomAgain:
            int randomPosition = Random.Range(1, coins.transform.childCount + 1);
            for (int y = 0; y < randomQuantity; y++)
            {
                if (randomPosition == randomPositions[y])
                {
                    goto RandomAgain;
                }
            }

            randomPositions[i] = randomPosition;

            GameObject coinObj = Instantiate(coin);
            coinObj.name = "Coin";
            coinObj.transform.position = coins.transform.GetChild(randomPosition - 1).transform.position;
            coinObj.transform.parent = transform;
        }
    }
    
    public float getPathLength()
    {
        return transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
    }
    public void stopAnimation() {
        if (gameObject.GetComponent<Animator>()) {
            gameObject.GetComponent<Animator>().speed = 0f;
        }
    }
}
