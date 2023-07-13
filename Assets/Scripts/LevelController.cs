using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LevelController : MonoBehaviour
{
    #region Singleton
    public static LevelController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of LevelController has been detected");
        }
    }
    #endregion

    public LevelData activeLevel;
    public LevelData[] Levels;

    public GameObject itemListParent;
    public GameObject itemListPrefab;
    public List<GameObject> itemList = new List<GameObject>();

    public Sprite checkMark;
    public Sprite crossMark;

    public TextMeshProUGUI levelTitle;


    public Animator lightAnimator;
    public CameraMove cameraMove;
    public int time_length = 45;
    // Start is called before the first frame update
    void Start()
    {
        InitializeLevels();

        StartLevel(0);
    }

    void InitializeLevels()
    {

        TimeController.sharedInstance.totalTimes = new float[Levels.Length];
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].levelBuilding.SetActive(false);
            Levels[i].selectableObjects = new List<SelectableObject>(Levels[i].levelBuilding.GetComponentsInChildren<SelectableObject>());
            Levels[i].goalObjectsNames = new List<string>();
        }
    }

    public void markAsChecked(string found_object_name)
    {
        foreach (GameObject itemListObject in itemList)
        {
            string object_name = itemListObject.GetComponentInChildren<TextMeshProUGUI>().text;

            if (found_object_name == object_name)
            {
                itemListObject.GetComponentInChildren<Image>().sprite = checkMark;
            }
        }
    }


    public void StartLevel(int level_index)
    {
        if (FlashlightController.sharedInstance.isUsingFlashlight) FlashlightController.sharedInstance.toggleFlashlight();
        FlashlightController.sharedInstance.flashlightPower = 1f;

        lightAnimator.SetTrigger("HideRoom");

        activeLevel = Levels[level_index];

        FlashlightController.sharedInstance.transform.position = 
            new Vector3(activeLevel.levelBuilding.transform.position.x,
            FlashlightController.sharedInstance.transform.position.y,
            FlashlightController.sharedInstance.transform.position.z);

        cameraMove.SetInitialPosition();
        int itemListCount;
        int itemListIndex = 0;

        while (activeLevel.goalObjectsNames.Count != activeLevel.numOfGoalObjects)
        {
            int random_index = Random.Range(0, activeLevel.selectableObjects.Count);

            string goal_object_name = activeLevel.selectableObjects[random_index].object_name;

            if (!activeLevel.goalObjectsNames.Contains(goal_object_name))
            {
                itemListCount = itemList.Count;
                activeLevel.goalObjectsNames.Add(goal_object_name);

                if (itemListIndex >= itemListCount)
                {
                    //Create a new item List instance
                    GameObject itemListInstance = Instantiate(itemListPrefab, itemListParent.transform);
                    itemListInstance.GetComponentInChildren<TextMeshProUGUI>().text = goal_object_name;
                    itemListInstance.GetComponentInChildren<Image>().sprite = crossMark;
                    itemList.Add(itemListInstance);
                }
                else
                {
                    //Just use an existing item list first
                    itemList[itemListIndex].GetComponentInChildren<TextMeshProUGUI>().text = goal_object_name;
                    itemList[itemListIndex].GetComponentInChildren<Image>().sprite = crossMark;
                }

                itemListIndex++;
            }
        }



        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].levelBuilding.SetActive(false);
        }

        Levels[level_index].levelBuilding.SetActive(true);
        levelTitle.text = Levels[level_index].level_name;

        TimeController.sharedInstance.BeginTimer(time_length);
        SoundController.sharedInstance.playBGM();

        UIController.sharedInstance.isPaused = false;
    }


    public void NextLevel()
    {

        lightAnimator.SetTrigger("ShowRoom");
        Debug.Log("Found all the objects, time to move to the next level.");

        SoundController.sharedInstance.stopBGM();
        for (int i = 0; i < Levels.Length; i++)
        {
            if (activeLevel.level_name == Levels[i].level_name)
            {
                TimeController.sharedInstance.EndTimer(i);

                if (i + 1 >= Levels.Length)
                {
                    UIController.sharedInstance.showEndScreen();
                }
                else
                {
                    UIController.sharedInstance.showLevelCompletePanel(i + 1);
                }

                break;
            }
        }

    }


    public void RestartLevel()
    {

        lightAnimator.SetTrigger("ShowRoom");

        activeLevel.goalObjectsNames.Clear();
        SoundController.sharedInstance.stopBGM();

        for (int i = 0; i < Levels.Length; i++)
        {
            if (activeLevel.level_name == Levels[i].level_name)
            {
                TimeController.sharedInstance.EndTimer(i);

                UIController.sharedInstance.showGameOverPanel(i);

                break;
            }
        }

    }
}

[System.Serializable]
public struct LevelData
{
    public GameObject levelBuilding;
    public string level_name;
    public int numOfGoalObjects;
    public List<SelectableObject> selectableObjects;
    public List<string> goalObjectsNames;
}