using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothing; //dampening
    public float maxX;

    Vector3 offset;

    public float lowY= 0;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;

        
    }

    public void setNewTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        if (transform.position.y < lowY) transform.position = new Vector3(transform.position.x, lowY, transform.position.z);

        if (transform.position.x > maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
    }
}
