using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject giftPrefab;

    [SerializeField] private GameObject pathsContainer;

    [SerializeField] private GameObject[] pathPrefabs;

    [SerializeField] private GameObject[] decorations;
    [SerializeField] private GameObject[] shadows;

    private int[] pathHistory;
    private float nextPathYPos;

    public void StartGame() {
        nextPathYPos = 0;
        pathHistory = new int[5];

        RemoveAllPaths();

        for(int i = 0;i < 10;i++) {
            LoadNextPaths();
        }
    }

    public void LoadNextPaths() {
        int pathIndex = GetRandomPathIndex();

        ChangePathHistory();

        GameObject obj = Instantiate(pathPrefabs[pathIndex]);
        obj.transform.parent = pathsContainer.transform;
        obj.transform.position = new Vector3(0, nextPathYPos);

        Path path = obj.GetComponent<Path>();

        nextPathYPos += path.GetPathLength();

        //Instantiates a random path decoration object
        int randomDecoration = Random.Range(0, decorations.Length);

        path.GenerateDecorations(decorations[randomDecoration], shadows[randomDecoration]);
        path.GenerateCoinsOrGift(coinPrefab, giftPrefab);

        int GetRandomPathIndex() {
            int randomValue;

            do {
                randomValue = Random.Range(1, pathPrefabs.Length);

                if(nextPathYPos == 0) { randomValue = 0; }
                if(!CheckPathHistory(randomValue)) {
                    randomValue = -1;
                }

            } while(randomValue == -1);

            return randomValue;

            bool CheckPathHistory(int value) {
                for(int i = 0;i < pathHistory.Length;i++) {
                    if(value == pathHistory[i] && value != 0) {
                        return false;
                    }
                }
                return true;
            }
        }
        void ChangePathHistory() {
            for(int i = pathHistory.Length - 1;i > 0;i--) {
                pathHistory[i] = pathHistory[i - 1];
            }

            pathHistory[0] = pathIndex;
        }
    }

    public void RemoveAllPaths() {
        for(int i = 0;i < pathsContainer.transform.childCount;i++) {
            Destroy(pathsContainer.transform.GetChild(i).gameObject);
        }

        pathHistory = new int[5];
    }
}
