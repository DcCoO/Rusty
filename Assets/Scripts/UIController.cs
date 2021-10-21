using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonMonoBehaviour<UIController>
{
    [SerializeField] GameObject hammerScreen;
    [SerializeField] RectTransform crosshair;    
    [SerializeField] GameObject progressPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHammerScreen(bool state)
    {
        hammerScreen.SetActive(state);
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

}
