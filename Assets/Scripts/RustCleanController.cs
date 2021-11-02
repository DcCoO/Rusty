using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustCleanController : SingletonMonoBehaviour<RustCleanController>
{
    public LayerMask mask;
    public bool working;

    [Header("Textures")]
    [SerializeField] Texture2D dirtMaskTex;
    [SerializeField] Texture2D ScratchMask;
    [SerializeField] Texture2D brushTex;
    [SerializeField] Texture2D albedoTex;

    [Header("Percentage")]
    public float percent;
    public float maxPossiblePercentage;
    public float currentPercent;

    Camera cam;
    int greenPixels, blackPixels;
    float screenHeight;


    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        cam = Camera.main;
        ResetMask();
        screenHeight = Screen.height;
        Rotator.instance.SetAutomaticRotation(true);
        blackPixels = 0;
        //ProgressPanel.instance.SetPercentage(0);
        working = true;
    }

    void Update()
    {
        if (!working) return;

        if (Input.GetKeyDown(KeyCode.S)) ResetMask();
        //if (Input.GetMouseButtonDown(0)) UIController.instance.SetCrosshairState(true);
        if (Input.GetMouseButton(0))
        {
            float heightPercent = 0.15f;
            //UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
            {
                Hose.instance.SetState(true);
                Hose.instance.LookAt(hit.point);
                Vector2 textureCoord = hit.textureCoord;

                int x = (int)(textureCoord.x * dirtMaskTex.width);
                int y = (int)(textureCoord.y * dirtMaskTex.height);

                int xOffset = x - (brushTex.width / 2), yOffset = y - (brushTex.height / 2);

                for(int i = 0; i < brushTex.width; ++i)
                {
                    for(int j = 0; j < brushTex.height; ++j)
                    {
                        Color brush = brushTex.GetPixel(i, j);
                        Color mask = dirtMaskTex.GetPixel(xOffset + i, yOffset + j);
                        float prod = mask.g * brush.g;
                        if (mask.g > 0.05 && prod < 0.05f && albedoTex.GetPixel(xOffset + i, yOffset + j) != Color.black) ++blackPixels;
                        dirtMaskTex.SetPixel(xOffset + i, yOffset + j, new Color(prod, prod , prod));
                    }
                }
                dirtMaskTex.Apply();
            }
            else
            {
                Hose.instance.SetState(false);
            }
            percent = (float)blackPixels / greenPixels;
            currentPercent = Mathf.Min(1, percent / maxPossiblePercentage);
            //ProgressPanel.instance.SetPercentage(currentPercent);
        }
        else
        {
            //UIController.instance.SetCrosshairState(false);
            Hose.instance.SetState(false);
        }
    }

    public void ResetMask()
    {
        int paintPixels = 0;
        for (int i = 0; i < dirtMaskTex.width; ++i)
        {
            for (int j = 0; j < dirtMaskTex.height; ++j)
            {
                dirtMaskTex.SetPixel(i, j, Color.white);
                if (albedoTex.GetPixel(i, j) != Color.black) paintPixels++;
            }
        }
        greenPixels = paintPixels;
        print($"Total pixels to paint = {greenPixels}");
        dirtMaskTex.Apply();
    }
}
