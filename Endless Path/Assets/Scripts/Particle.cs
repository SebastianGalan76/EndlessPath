using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private new BackgroundParticlesSystem particleSystem;

    private float speed;
    private float lastChangeTime;

    private Vector3 currentPosition, nextPosition;
    private Vector3 currentScale, nextScale;

    public void Initialize(BackgroundParticlesSystem particleSystem, float speed, Vector3 nextPosition, Vector3 nextScale) {
        this.particleSystem = particleSystem;
        this.speed = speed;

        currentPosition = transform.localPosition;
        currentScale = transform.localScale;

        this.nextPosition = nextPosition;
        this.nextScale = nextScale;

        lastChangeTime = Time.time;
    }

    private void Update()
    {
        float t = (Time.time - lastChangeTime) * speed;

        if(t >= 1f) {
            lastChangeTime = Time.time;

            currentPosition = transform.localPosition;
            currentScale = transform.localScale;

            nextPosition = particleSystem.GetRandomPosition();
            nextScale = particleSystem.GetRandomScale();

            t = 0;
        }

        transform.localPosition = Vector3.Lerp(currentPosition, nextPosition, t);
        transform.localScale = Vector3.Lerp(currentScale, nextScale, t);
    }
}
