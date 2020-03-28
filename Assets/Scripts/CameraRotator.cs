﻿using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float Tilt;

    void Awake()
    {
        RotateAroundOriginBy(Tilt);
    }

    public void OnValidate()
    {
        RotateAroundOriginBy(Tilt);
    }

    private void RotateAroundOriginBy(float tilt)
    {
        GetComponent<Camera>().transform.position = 10 * Vector3.back;
        GetComponent<Camera>().transform.rotation = Quaternion.identity;
        GetComponent<Camera>().transform.RotateAround(Vector3.zero, Vector3.right, tilt);
    }
}
