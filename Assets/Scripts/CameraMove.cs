using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{


    Vector3 initialPosition;
    public Vector2 maxShift;
    public float offsetFactor;

    private void Start()
    {
        SetInitialPosition();
    }

    public void SetInitialPosition()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        //CONVERT MOUSE COORDINATES FROM SCREEN TO WORLD POSITION
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        //GET DIFFERENCE BETWEEN PLAYER AND MOUSE COORDINATES

        Vector3 difference;
        difference.x = mouseWorldPosition.x - transform.position.x;
        difference.y = mouseWorldPosition.y - transform.position.y;

        float newXPosition = Mathf.Clamp(transform.position.x + (difference.x / offsetFactor), initialPosition.x - maxShift.x, initialPosition.x + maxShift.x);
        float newYPosition = Mathf.Clamp(transform.position.y + (difference.y / offsetFactor), initialPosition.y - maxShift.y, initialPosition.y + maxShift.y);
        transform.position = Vector3.Lerp(transform.position, new Vector3(newXPosition, newYPosition, transform.position.z), Time.deltaTime);


    }
}
