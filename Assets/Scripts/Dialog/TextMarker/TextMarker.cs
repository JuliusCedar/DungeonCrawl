using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMarker : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        StartCoroutine(Go());
    }

    public IEnumerator Go()
    {
        float distance = 0;
        float counter = 0;
        Vector3 previousPos = target.position;
        while (counter < lifetime) {
            counter += Time.deltaTime;
            distance += Time.deltaTime * speed;
            if (target != null)
            {
                gameObject.transform.position = target.position + new Vector3(0, distance, 0) + offset;
                previousPos = target.position;
            }
            else
            {
                gameObject.transform.position = previousPos + new Vector3(0, distance, 0) + offset;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
