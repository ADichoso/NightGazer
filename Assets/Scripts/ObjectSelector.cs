using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class ObjectSelector : MonoBehaviour
{
    #region Singleton
    public static ObjectSelector sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of ObjectSelector has been detected");
        }
    }
    #endregion


    public Camera mainCamera;
    public GameObject greenflashVFXObject;
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }


    void Update()
    {
        if (!UIController.sharedInstance.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            { // if left button pressed...
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {


                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log(hit.transform.name);
                        // the object identified by hit.transform was clicked
                        // do whatever you want

                        if (hit.transform.GetComponent<SelectableObject>() != null)
                        {
                            string selectable_object_name = hit.transform.GetComponent<SelectableObject>().object_name;
                            if (LevelController.sharedInstance.activeLevel.goalObjectsNames.Contains(selectable_object_name))
                            {
                                hit.transform.GetComponent<SelectableObject>().selectObject();

                                //remove the object from the list
                                LevelController.sharedInstance.markAsChecked(selectable_object_name);
                                LevelController.sharedInstance.activeLevel.goalObjectsNames.Remove(selectable_object_name);

                                //Play the correct sound
                                SoundController.sharedInstance.playSound(SoundController.sharedInstance.correctAnswerSound, false);


                                greenflashVFXObject.transform.position = hit.transform.position;
                                greenflashVFXObject.GetComponent<VisualEffect>().Play();

                                if (LevelController.sharedInstance.activeLevel.goalObjectsNames.Count == 0)
                                {
                                    LevelController.sharedInstance.NextLevel();
                                }
                            }
                            else
                            {
                                SoundController.sharedInstance.playSound(SoundController.sharedInstance.wrongAnswerSound, false);

                                TimeController.sharedInstance.decreaseTimer(10);
                            }
                        }
                        else
                        {
                            SoundController.sharedInstance.playSound(SoundController.sharedInstance.wrongAnswerSound, false);

                            TimeController.sharedInstance.decreaseTimer(10);
                        }

                    }
                }
            }
        }
    }
}