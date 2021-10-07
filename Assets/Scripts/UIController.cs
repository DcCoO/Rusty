using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonMonoBehaviour<UIController>
{
    [SerializeField] GameObject hammerScreen;
    [SerializeField] RectTransform crosshair;
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
}
