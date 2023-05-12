using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BackgroundParticlesSystem : MonoBehaviour
{
    [SerializeField] private float minXPos, maxXPos;
    [SerializeField] private float minYPos, maxYPos;

    [SerializeField] private float minScale, maxScale;
    
    [SerializeField] private Sprite[] images;
    [SerializeField] private GameObject particlePrefab;

    //The amount of particles on the screen
    private int amount = 9;

    private GameObject[] particles;
    private int[] particleIDHistory = { -1, -1, -1, -1, -1 };

    private void Start()
    {
        GenerateParticles();
    }

    public void GenerateParticles()
    {
        DestroyAllParticles();
        particles = new GameObject[amount];

        ChangeParticlesImage();

        for (int i = 0; i < amount; i++) {
            GameObject particle = Instantiate(particlePrefab);
            particle.transform.parent = gameObject.transform;

            particles[i] = particle;

            //Sets initial position of particle
            particle.transform.localPosition = GetRandomPosition();
            //Sets initial scale of particle
            particle.transform.localScale = GetRandomScale();
            //Sets random rotation
            particle.transform.rotation = GetRandomRotation();
            //Sets random color alfa
            particle.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)Random.Range(10, 25));

            particle.GetComponent<Particle>().Initialize(this, Random.Range(0.05f, 0.1f), GetRandomPosition(), GetRandomScale());
        }
    }
    public Vector3 GetRandomPosition() {
        return new Vector3(Random.Range(minXPos, maxXPos), Random.Range(minYPos, maxYPos));
    }
    public Vector3 GetRandomScale() {
        float scale = Random.Range(minScale, maxScale);
        return new Vector3(scale, scale);
    }

    private Quaternion GetRandomRotation() {
        return new Quaternion(0, 0, Random.Range(0, 360), 360);
    }

    private void DestroyAllParticles() {
        if(particles==null) return;

        for(int i = 0;i < particles.Length;i++) {
            if(particles[i] != null) {
                Destroy(particles[i].gameObject);
            }
        }
    }

    private void ChangeParticlesImage() {
        int randomImageIndex;
        do {
            randomImageIndex = Random.Range(0, images.Length);
        } while(CheckHistory(randomImageIndex));

        particlePrefab.GetComponent<SpriteRenderer>().sprite= images[randomImageIndex];

        for (int i = particleIDHistory.Length-1; i > 0; i--)
        {
            particleIDHistory[i] = particleIDHistory[i-1];
        }

        particleIDHistory[0] = randomImageIndex;

        bool CheckHistory(int value) {
            for (int i = 0; i < particleIDHistory.Length; i++)
            {
                if (value == particleIDHistory[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
