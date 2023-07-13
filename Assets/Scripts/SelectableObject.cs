using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public string object_name;
  

    public void selectObject()
    {
        Debug.Log("YOU FOUND THE " + object_name);
    }

}
