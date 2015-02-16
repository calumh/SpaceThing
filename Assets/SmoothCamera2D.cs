//Code modified from http://answers.unity3d.com/questions/29183/2d-camera-smooth-follow.html

using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{

    public float dampTime = 0f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = destination;
        }

    }
}