using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustCleanController : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask mask;
    [SerializeField] Texture2D rustyMaskTex, brushTex;

    public float percent;
    int greenPixels, blackPixels;
    float screenHeight;
    void Start()
    {
        cam = Camera.main;
        ResetMask();
        screenHeight = Screen.height;
        Rotator.instance.SetAutomaticRotation(true);
        greenPixels = (rustyMaskTex.width * rustyMaskTex.height);
        blackPixels = 0;
    }

    void Update()
    {
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

                print(brushTex.width);

                for(int i = 0; i < brushTex.width; ++i)
                {
                    for(int j = 0; j < brushTex.height; ++j)
                    {
                        Color brush = brushTex.GetPixel(i, j);
                        Color mask = rustyMaskTex.GetPixel(xOffset + i, yOffset + j);
                        float prod = mask.g * brush.g;
                        if (mask.g > 0.05 && prod == 0) ++blackPixels;
                        rustyMaskTex.SetPixel(xOffset + i, yOffset + j, new Color(0, prod , 0));
                    }
                }
                rustyMaskTex.Apply();
            }
            else
            {
                Hose.instance.SetState(false);
            }
            percent = (float)blackPixels / (float)greenPixels;
        }
        else
        {
            UIController.instance.SetCrosshairState(false);
            Hose.instance.SetState(false);
        }
    }

    public void ResetMask()
    {
        for (int i = 0; i < rustyMaskTex.width; ++i)
        {
            for (int j = 0; j < rustyMaskTex.height; ++j)
            {
                rustyMaskTex.SetPixel(i, j, Color.green);
            }
        }
        rustyMaskTex.Apply();
    }
}
