using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Camera mainCamera;
    public float rotationIncrement = 9f;
    public float zoomIncrement = 0.25f;

    float yRotation;
    float currentZoom;

    public float rotationDuration = 1f;
    public float zoomDuration = 1f;
    public void rotate(int direction)
    {
        //Rotate the camera around whenever yRotation has an input (to the left when A, to the right when D)
        yRotation += rotationIncrement * direction;

        //Clamp the xRotation
        yRotation = Mathf.Clamp(yRotation, -45, 45);

        StartCoroutine(LerpRotation());
        //Rotate the whole player along the y axis
    }

    Quaternion newRotation;
    IEnumerator LerpRotation()
    {
        float timeElapsed = 0;

        
        while (timeElapsed < rotationDuration)
        {
            newRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yRotation, 0), timeElapsed / rotationDuration);

            transform.rotation = newRotation;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        newRotation = Quaternion.Euler(0, yRotation, 0);
    }

    float newZoom;
    IEnumerator LerpZoom()
    {
        float timeElapsed = 0;


        while (timeElapsed < zoomDuration)
        {
            newZoom = Mathf.Lerp(mainCamera.orthographicSize, currentZoom, timeElapsed / zoomDuration);

            mainCamera.orthographicSize = newZoom;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        newZoom = currentZoom;
    }
    public void changeZoom(int direction)
    {
        currentZoom = mainCamera.orthographicSize;


        Debug.Log(currentZoom);

        currentZoom += zoomIncrement * direction;

        Debug.Log(currentZoom);

        currentZoom = Mathf.Clamp(currentZoom, 1, 5);

        StartCoroutine(LerpZoom());

    }
}
