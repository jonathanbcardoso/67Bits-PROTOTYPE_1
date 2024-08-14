using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// ConfigManager.cs
public class ConfigManager : MonoBehaviour
{
    [Header("                               ==== Config ====")]
    [SerializeField] private GameObject btnConfig;
    [SerializeField] private GameObject btnSound;

    [Header("                               ==== FPS ====")]
    [SerializeField] private GameObject btnFPS;
    [SerializeField] private TextMeshProUGUI textFPS;
    //[SerializeField] private TextMeshProUGUI textTargetFrameRate;
    //[SerializeField] private TextMeshProUGUI textScreenRefreshRate;
    [SerializeField] private GameObject panelFPS;
    [SerializeField] private List<Sprite> spriteIconList;
    [NonSerialized] private float deltaTime;
    [NonSerialized] private int currentFPS;

    // Start is called before the first frame update
    private void Start()
    {
        btnConfig.SetActive(true);
        btnSound.SetActive(false);
        btnFPS.SetActive(false);
    }
    void Update()
    {
        //targetFrameRate.text = "FR:" + Application.targetFrameRate.ToString();
        //screenRefreshRate.text = "RR:" + Math.Round(Screen.currentResolution.refreshRateRatio.value);

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        textFPS.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }

    public void ClickDeactivateSound()
    {
        btnSound.GetComponent<Animation>().Play("BUTTON_INCREASE_SIZE_EFFECT");

        if (AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void ClickOpenSubMenu()
    {
        btnConfig.GetComponent<Animation>().Play("BUTTON_ROTATE_SIZE_EFFECT");
        StartCoroutine(this.OpenSubMenu());
    }

    private IEnumerator OpenSubMenu()
    {
        yield return new WaitForSeconds(0.1f);

        if (btnSound.activeSelf == false)
        {
            btnFPS.SetActive(true);
            btnSound.SetActive(true);
        }
        else
        {
            btnFPS.SetActive(false);
            btnSound.SetActive(false);
        }
    }

    public void ClickChangeFPS()
    {
        btnFPS.GetComponent<Animation>().Play("BUTTON_INCREASE_SIZE_EFFECT");
        currentFPS += 1;

        switch (currentFPS)
        {
            case 1:
                Application.targetFrameRate = 15;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[0];
                panelFPS.SetActive(true);
                break;
            case 2:
                Application.targetFrameRate = 30;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[1];
                break;
            case 3:
                Application.targetFrameRate = 60;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[2];
                break;
            case 4:
                currentFPS = 0;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[3];
                panelFPS.SetActive(false);
                break;
            default:
                break;
        }
    }

}