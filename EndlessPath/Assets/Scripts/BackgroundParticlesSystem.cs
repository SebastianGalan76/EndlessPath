using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BackgroundParticlesSystem : MonoBehaviour
{
    public GameObject ParticlePrefab;
    public GameObject[] Particle;
    public Sprite[] ParticlesImage;

    private int[] particleIDHistory = new int[5];

    public float minXPos, maxXPos;
    public float minYPos, maxYPos;

    public float minScale, maxScale;

    //The amount of particles on the screen
    private int particleNumber = 9;

    private void Start()
    {
        generateParticles();
    }

    public void generateParticles()
    {
        //Destroy exist particles
        for (int i = 0; i < Particle.Length; i++)
        {
            Destroy(Particle[i].gameObject);
        }

        changeParticlesImage();

        Particle = new GameObject[particleNumber];
        for (int i = 0; i < particleNumber; i++) {
            GameObject particle = Instantiate(ParticlePrefab);
            particle.transform.parent = gameObject.transform;

            Particle[i] = particle;

            //Sets initial position of particle
            particle.transform.localPosition = getNextPosition();

            //Sets initial scale of particle
            float scale = getScale();
            particle.transform.localScale = new Vector3(scale, scale);

            //Rotation object, if the image of the particle is not a company logo
            if (particleIDHistory[0] != 6) { 
                //Sets random rotation
                int randomRotation = Random.Range(0, 360);
                particle.transform.rotation = new Quaternion(0, 0, randomRotation, 360);
            }

            //Sets random color alfa
            particle.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)Random.Range(10, 25));

            particle.GetComponent<Particle>().ParticlesObject = gameObject;
            particle.GetComponent<Particle>().setSpeed(Random.Range(0.05f, 0.1f));
            particle.GetComponent<Particle>().setNextPosition(getNextPosition());
            particle.GetComponent<Particle>().setNextScale(getScale());
        }
    }
    public Vector3 getNextPosition() {
        float posX = Random.Range(minXPos, maxXPos);
        float posY = Random.Range(minYPos, maxYPos);

        return new Vector3(posX, posY);
    }
    public float getScale() {
        return Random.Range(minScale, maxScale);
    }

    private void changeParticlesImage() {
        RandomAgain:
        int randomParticleID = Random.Range(0, ParticlesImage.Length);
        if (!checkHistory(randomParticleID)) {
            goto RandomAgain;
        }

        ParticlePrefab.GetComponent<SpriteRenderer>().sprite= ParticlesImage[randomParticleID];

        for (int i = particleIDHistory.Length-1; i > 0; i--)
        {
            particleIDHistory[i] = particleIDHistory[i-1];
            
        }

        particleIDHistory[0] = randomParticleID;

        bool checkHistory(int value) {
            for (int i = 0; i < particleIDHistory.Length; i++)
            {
                if (value == particleIDHistory[i] && value != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
