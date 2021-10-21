using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustCleanController : SingletonMonoBehaviour<RustCleanController>
{
    Camera cam;
    [SerializeField] LayerMask mask;
    [SerializeField] Texture2D rustyMaskTex, brushTex, albedoTex;

    public float percent;
    public float maxPossiblePercentage;
    public float currentPercent;
    int greenPixels, blackPixels;
    float screenHeight;

    public bool working;

    private void Start()
    {
        //Setup();
    }

    public void Setup()
    {
        cam = Camera.main;
        ResetMask();
        screenHeight = Screen.height;
        Rotator.instance.SetAutomaticRotation(true);
        blackPixels = 0;
        ProgressPanel.instance.SetPercentage(0);
        working = true;
    }

    void Update()
    {
        if (!working) return;

        if (Input.GetKeyDown(KeyCode.S)) ResetMask();
        if(Input.GetMouseButtonDown(0)) UIController.instance.SetCrosshairState(true);
        if (Input.GetMouseButton(0))
        {
            float heightPercent = 0.15f;
            UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
            {
                Hose.instance.SetState(true);
                Hose.instance.LookAt(hit.point);
                Vector2 textureCoord = hit.textureCoord;

                int x = (int)(textureCoord.x * rustyMaskTex.width);
                int y = (int)(textureCoord.y * rustyMaskTex.height);

                int xOffset = x - (brushTex.width / 2), yOffset = y - (brushTex.height / 2);

                for(int i = 0; i < brushTex.width; ++i)
                {
                    for(int j = 0; j < brushTex.height; ++j)
                    {
                        Color brush = brushTex.GetPixel(i, j);
                        Color mask = rustyMaskTex.GetPixel(xOffset + i, yOffset + j);
                        float prod = mask.g * brush.g;
                        if (mask.g > 0.05 && prod < 0.05f && albedoTex.GetPixel(xOffset + i, yOffset + j) != Color.black) ++blackPixels;
                        rustyMaskTex.SetPixel(xOffset + i, yOffset + j, new Color(0, prod , 0));
                    }
                }
                rustyMaskTex.Apply();
            }
            else
            {
                Hose.instance.SetState(false);
            }
            percent = (float)blackPixels / greenPixels;
            currentPercent = Mathf.Min(1, percent / maxPossiblePercentage);
            //UIController.instance.SetProgressPercentage(currentPercent);
            ProgressPanel.instance.SetPercentage(currentPercent);
        }
        else
        {
            UIController.instance.SetCrosshairState(false);
            Hose.instance.SetState(false);
        }
    }

    public void ResetMask()
    {
        int paintPixels = 0;
        for (int i = 0; i < rustyMaskTex.width; ++i)
        {
            for (int j = 0; j < rustyMaskTex.height; ++j)
            {
                rustyMaskTex.SetPixel(i, j, Color.green);
                if (albedoTex.GetPixel(i, j) != Color.black) paintPixels++;
            }
        }
        greenPixels = paintPixels;
        print($"Total pixels to paint = {greenPixels}");
        rustyMaskTex.Apply();
    }
}
