using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public class Path : MonoBehaviour
{
    [SerializeField] private GameObject decorations;
    [SerializeField] private GameObject coins;

    public void GenerateDecorations(GameObject decorationPrefab, GameObject shadowPrefab) {
        if(decorations == null) { return; }

        int decorationAmount = GetRandomAmount();

        int[] randomPositions = new int[decorationAmount];

        for (int i = 0; i < decorationAmount; i++) {
            randomPositions[i] = GetRandomPosition();
            int randomRotation = Random.Range(0, 360);

            GenerateDecoration(randomPositions[i], randomRotation);
        }

        int GetRandomAmount() {
            int randomValue = Random.Range(0, 100);

            if(randomValue < 30) {
                return 1;
            } else if(randomValue < 60) {
                return 2;
            }

            return 3;
        }
        int GetRandomPosition() {
            int randomPosition;
            do {
                randomPosition = Random.Range(1, decorations.transform.childCount + 1);
                
                for(int i = 0;i < decorationAmount;i++) {
                    if(randomPosition == randomPositions[i]) {
                        randomPosition = -1;
                    }
                }

            } while(randomPosition == -1);;

            return randomPosition;
        }
        void GenerateDecoration(int randomPosition, int randomRotation) {
            //Decoration
            InstantiateObject(decorationPrefab, false);

            //Shadow of decoration
            InstantiateObject(shadowPrefab, true);

            void InstantiateObject(GameObject objPrefab, bool objIsShadow) {
                GameObject obj = Instantiate(objPrefab);

                if(objIsShadow) {
                    obj.transform.position = decorations.transform.GetChild(randomPosition - 1).transform.position + new Vector3(0.1f, -0.1f, 0);
                } else {
                    obj.transform.position = decorations.transform.GetChild(randomPosition - 1).transform.position;
                }

                obj.transform.parent = transform;
                obj.transform.rotation = new Quaternion(0, 0, randomRotation, 360);
            }
        }
    }
    public void GenerateCoinsOrGift(GameObject coinPrefab, GameObject giftPrefab) {
        if (coins == null) { return; }

        int randomAmount = GetRandomAmount();

        if(randomAmount == 0) { return; }

        if(randomAmount == -1) {
            InstantiateObject(giftPrefab, "Gift", 0);

            return;
        }

        int[] randomPositions = new int[randomAmount];

        for (int i = 0; i < randomAmount; i++)
        {
            randomPositions[i] = GetRandomPosition();

            InstantiateObject(coinPrefab, "Coin", randomPositions[i]);
        }

        int GetRandomAmount() {
            int randomValue = Random.Range(0, 100);
            if(randomValue < 10) {
                return 2;
            } else if(randomValue > 60) {
                return 1;
            }
            else if(randomValue > 97) {
                return -1;
            }

            return 0;
        }
        int GetRandomPosition() {
            int randomPosition;

            do {
                randomPosition = Random.Range(1, coins.transform.childCount + 1);

                for(int i = 0;i < randomAmount;i++) {
                    if(randomPosition == randomPositions[i]) {
                        randomPosition = -1;
                    }
                }
            } while(randomPosition == -1);

            return randomPosition;
        }
        void InstantiateObject(GameObject objPrefab, string objName, int position) {
            GameObject obj = Instantiate(objPrefab);

            obj.name = objName;
            obj.transform.position = coins.transform.GetChild(position - 1).transform.position;
            obj.transform.parent = transform;
        }
    }
    
    public float GetPathLength()
    {
        return transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void StopAnimation() {
        if (gameObject.GetComponent<Animator>()) {
            gameObject.GetComponent<Animator>().speed = 0f;
        }
    }
}
