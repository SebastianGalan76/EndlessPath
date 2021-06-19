using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public GameObject ParticlesObject;

    private float speed;
    private Vector3 nextPosition;
    private float nextScale;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, nextPosition, speed * Time.deltaTime);

        if (objectIsNearNextPosition()) {
            nextPosition = ParticlesObject.GetComponent<BackgroundParticlesSystem>().getNextPosition();
        }

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(nextScale, nextScale), speed * Time.deltaTime);

        if (objectIsNearNextScale()) {
            nextScale = ParticlesObject.GetComponent<BackgroundParticlesSystem>().getScale();
        }

    }

    //Returns true, when the particle object is near final movement position.
    private bool objectIsNearNextPosition() {
        if (Mathf.Abs(transform.localPosition.x - nextPosition.x) < 0.9f && Mathf.Abs(transform.localPosition.y - nextPosition.y) < 0.9f) {
            return true;
        }
        return false;
    }

    //Returns true, when the particle scale is almost equal to the final scale.
    private bool objectIsNearNextScale() {
        if (Mathf.Abs(transform.localScale.x - nextScale) < 0.25f) {
            return true;
        }
        return false;
    }

    public void setSpeed(float value) {
        speed = value;
    }
    public void setNextPosition(Vector3 value) {
        nextPosition = value;
    }
    public void setNextScale(float value) {
        nextScale = value;
    }
}
