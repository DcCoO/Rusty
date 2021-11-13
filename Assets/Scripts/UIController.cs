using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonMonoBehaviour<UIController>
{
    [SerializeField] GameObject lowerMenu;
    [SerializeField] RectTransform crosshair;    
    [SerializeField] GameObject progressPanel;

    [Header("Outlines")]
    [SerializeField] Outline hoseOutline;
    [SerializeField] Outline laserOutline;
    [SerializeField] Outline sprayOutline;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLowerMenuState(bool state)
    {
        lowerMenu.SetActive(state);
    }

    public void SetCrosshairState(bool state)
    {
        crosshair.gameObject.SetActive(state);
    }
    public void MoveCrosshair(Vector2 position, Vector2 offset)
    {
        crosshair.anchoredPosition = position + offset;
    }

    public void SetProgressPanelState(bool state)
    {
        progressPanel.SetActive(state);
    }

    public void SetHose()
    {
        CleanController.instance.SetTool(Tool.HOSE);
        hoseOutline.enabled = true;
        laserOutline.enabled = false;
        sprayOutline.enabled = false;
    }

    public void SetLaser()
    {
        CleanController.instance.SetTool(Tool.LASER);
        hoseOutline.enabled = false;
        laserOutline.enabled = true;
        sprayOutline.enabled = false;
    }

    public void SetSpray()
    {
        CleanController.instance.SetTool(Tool.SPRAY);
        hoseOutline.enabled = false;
        laserOutline.enabled = false;
        sprayOutline.enabled = true;
    }
    

}
