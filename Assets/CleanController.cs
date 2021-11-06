﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanController : SingletonMonoBehaviour<CleanController>
{
    public Tool currentTool = Tool.NONE;
    public LayerMask mask;

    [Header("Textures")]
    [SerializeField] Texture2D dirtMask;
    [SerializeField] Texture2D scratchMask;
    [SerializeField] Texture2D brushTex;
    [SerializeField] Texture2D idTex;

    [Header("Percentage")]
    public float percent;
    public float maxPossiblePercentage;
    public float currentPercent;

    Camera cam;
    int whitePixels, blackPixels;
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
        ProgressPanel.instance.SetPercentage(0);
    }

    public void SetTool(Tool tool)
    {
        currentTool = tool;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ResetMask();
        if (currentTool == Tool.NONE) return;

        if (Input.GetMouseButtonDown(0)) UIController.instance.SetCrosshairState(true);
        if (Input.GetMouseButton(0))
        {
            if (currentTool == Tool.HOSE) HoseUpdate();
            else if (currentTool == Tool.LASER) LaserUpdate();
            else if (currentTool == Tool.SPRAY) SprayUpdate();
        }
        else
        {
            UIController.instance.SetCrosshairState(false);
            Hose.instance.SetState(false);
            Laser.instance.SetState(false);
            Spray.instance.SetState(false);
        }
    }


    void HoseUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            Hose.instance.SetState(true);
            Hose.instance.LookAt(hit.point);
            Vector2 textureCoord = hit.textureCoord;

            int x = (int)(textureCoord.x * dirtMask.width);
            int y = (int)(textureCoord.y * dirtMask.height);

            int xOffset = x - (brushTex.width / 2), yOffset = y - (brushTex.height / 2);

            for (int i = 0; i < brushTex.width; ++i)
            {
                for (int j = 0; j < brushTex.height; ++j)
                {
                    Color brush = brushTex.GetPixel(i, j);
                    Color mask = dirtMask.GetPixel(xOffset + i, yOffset + j);
                    Color albedo = idTex.GetPixel(xOffset + i, yOffset + j);
                    if (albedo.r > 0.3f) continue;

                    float prod = mask.g * brush.g;
                    if (mask.g > 0.05 && prod < 0.05f && idTex.GetPixel(xOffset + i, yOffset + j) != Color.black) ++blackPixels;
                    dirtMask.SetPixel(xOffset + i, yOffset + j, new Color(prod, prod, prod));
                }
            }
            dirtMask.Apply();
        }
        else
        {
            Hose.instance.SetState(false);
        }
        percent = (float)blackPixels / whitePixels;
        currentPercent = Mathf.Min(1, percent / maxPossiblePercentage);
    }

    void LaserUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            Laser.instance.SetState(true);
            Laser.instance.LookAt(hit.point, hit.normal);
            Vector2 textureCoord = hit.textureCoord;

            int x = (int)(textureCoord.x * dirtMask.width);
            int y = (int)(textureCoord.y * dirtMask.height);

            int xOffset = x - (brushTex.width / 2), yOffset = y - (brushTex.height / 2);

            for (int i = 0; i < brushTex.width; ++i)
            {
                for (int j = 0; j < brushTex.height; ++j)
                {
                    Color brush = brushTex.GetPixel(i, j);
                    Color mask = dirtMask.GetPixel(xOffset + i, yOffset + j);
                    Color albedo = idTex.GetPixel(xOffset + i, yOffset + j);
                    if (albedo.r < 0.3f) continue;

                    float prod = mask.g * brush.g;
                    if (mask.g > 0.05 && prod < 0.05f && albedo != Color.black) ++blackPixels;
                    dirtMask.SetPixel(xOffset + i, yOffset + j, new Color(prod, prod, prod));
                }
            }
            dirtMask.Apply();
        }
        else
        {
            Laser.instance.SetState(false);
        }
        percent = (float)blackPixels / whitePixels;
        currentPercent = Mathf.Min(1, percent / maxPossiblePercentage);
    }

    void SprayUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            Spray.instance.SetState(true);
            Spray.instance.LookAt(hit.point);
            Vector2 textureCoord = hit.textureCoord;

            int x = (int)(textureCoord.x * dirtMask.width);
            int y = (int)(textureCoord.y * dirtMask.height);

            int xOffset = x - (brushTex.width / 2), yOffset = y - (brushTex.height / 2);

            for (int i = 0; i < brushTex.width; ++i)
            {
                for (int j = 0; j < brushTex.height; ++j)
                {
                    Color brush = brushTex.GetPixel(i, j);

                    Color dirt = dirtMask.GetPixel(xOffset + i, yOffset + j);
                    Color scratch = scratchMask.GetPixel(xOffset + i, yOffset + j);
                    if (dirt.r > 0.25f || dirt.g > 0.25f || dirt.b > 0.25f) continue;

                    float color = Mathf.Min(brush.g, scratch.g);
                    //if (mask.g > 0.05 && prod < 0.05f && idTex.GetPixel(xOffset + i, yOffset + j) != Color.black) ++blackPixels;
                    scratchMask.SetPixel(xOffset + i, yOffset + j, new Color(color, color, color));
                }
            }
            scratchMask.Apply();
        }
        else
        {
            Spray.instance.SetState(false);
        }
        percent = (float)blackPixels / whitePixels;
        currentPercent = Mathf.Min(1, percent / maxPossiblePercentage);
    }







    public void ResetMask()
    {
        int paintPixels = 0;
        for (int i = 0; i < dirtMask.width; ++i)
        {
            for (int j = 0; j < dirtMask.height; ++j)
            {
                dirtMask.SetPixel(i, j, Color.white);
                scratchMask.SetPixel(i, j, Color.white);
                if (idTex.GetPixel(i, j) != Color.black) paintPixels += 2;
            }
        }
        whitePixels = paintPixels;
        print($"Total pixels to paint = {whitePixels}");
        dirtMask.Apply();
        scratchMask.Apply();
    }
}
