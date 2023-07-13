using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FlashlightController : MonoBehaviour
{
    #region Singleton
    public static FlashlightController sharedInstance;

    void Awake()
    {
        sharedInstance = this;
        if (this != sharedInstance)
        {
            Debug.Log("Warning! More than 1 instance of FlashlightController has been detected");
        }
    }
    #endregion

    [SerializeField] private Slider flashlightPowerSlider;

    private float _flashlightPower;
    public float flashlightPower
    {
        get { return _flashlightPower; }
        set
        {
            if (_flashlightPower == value) return;

            _flashlightPower = value;

            OnVariableChange(flashlightPowerSlider, _flashlightPower);
        }
    }
    public delegate void OnVariableChangeDelegate(Slider slider, float newVal);
    public event OnVariableChangeDelegate OnVariableChange;
    public void SetSliderValue(Slider slider, float stamina)
    {
        slider.value = stamina;
    }

    [SerializeField] private GameObject flashlightObject;

    [SerializeField] public bool isUsingFlashlight = false;
    [SerializeField] private float powerConsumptionRate = 3f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float flashlightZDistance;
    [SerializeField] private TextMeshProUGUI flashlightButtonText;

    private void Start()
    {
        OnVariableChange += SetSliderValue;
        flashlightPower = 1f;
    }


    private void Update()
    {
        if (!UIController.sharedInstance.isPaused)
        {
            if (Input.GetButtonDown("Flashlight") && flashlightPower > 0)
            {
                toggleFlashlight();
            }


            if (isUsingFlashlight && flashlightPower > 0)
            {
                Vector3 mousePos = Input.mousePosition;

                flashlightObject.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, flashlightZDistance));
                flashlightObject.transform.localRotation = mainCamera.transform.rotation;
                flashlightPower -= powerConsumptionRate * Time.deltaTime;

                if (flashlightPower <= 0)
                {
                    isUsingFlashlight = false;
                    turnOffFlashlight();
                    return;
                }
            }
        }

      
    }


    public void toggleFlashlight()
    {
        SoundController.sharedInstance.playSound(SoundController.sharedInstance.flashlightClick, false);
        isUsingFlashlight = !isUsingFlashlight;

        if (isUsingFlashlight)
        {
            
            useFlashlight();
        }
        else
        {
            
            turnOffFlashlight();
        }
    }



    void useFlashlight()
    {
        flashlightButtonText.text = "ON";
        flashlightObject.SetActive(true);
    }

    void turnOffFlashlight()
    {
        flashlightButtonText.text = "OFF";
        flashlightObject.SetActive(false);
    }
}
