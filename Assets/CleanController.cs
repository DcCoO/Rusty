using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] int totalPixelsToPaint, paintedPixels;

    [Header("UI")]
    [SerializeField] LayerMask uiMask;

    Camera cam;
    float screenHeight;

    bool isOverUI, canUseTools;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Setup(Texture2D id, int pixelsToPaint = 0)
    {
        if(pixelsToPaint != 0) totalPixelsToPaint = pixelsToPaint;
        idTex = id;
        Setup();
    }

    public void Setup()
    {
        ResetMask();
        screenHeight = Screen.height;
        Rotator.instance.SetAutomaticRotation(true);
        paintedPixels = 0;
        ProgressPanel.instance.SetPercentage(0);
    }

    public void SetTool(Tool tool)
    {
        currentTool = tool;
    }

    void Update()
    {
        isOverUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetKeyDown(KeyCode.A)) ClearMask();
        if (Input.GetKeyDown(KeyCode.T)) ResetMaskAsync();
        if (Input.GetKeyDown(KeyCode.R)) ResetMask();
        if (Input.GetKeyDown(KeyCode.S)) print("TOTAL PAINTED PIXELS = " + paintedPixels);
        if (currentTool == Tool.NONE) return;
        

        if (Input.GetMouseButtonDown(0) && !isOverUI)
        {
            UIController.instance.SetCrosshairState(true);
            UIController.instance.SetLowerMenuState(false);
            canUseTools = true;
        }
        if (Input.GetMouseButton(0) && canUseTools)
        {
            if (currentTool == Tool.HOSE) HoseUpdate();
            else if (currentTool == Tool.LASER) LaserUpdate();
            else if (currentTool == Tool.SPRAY) SprayUpdate();
            ProgressPanel.instance.SetPercentage(percent);
        }
        else
        {
            UIController.instance.SetCrosshairState(false);
            UIController.instance.SetLowerMenuState(true);
            Hose.instance.SetState(false);
            Laser.instance.SetState(false);
            Spray.instance.SetState(false);
            canUseTools = false;
        }
    }


    void HoseUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            UIController.instance.SetLowerMenuState(false);
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
                    if (mask.g > 0.05 && prod < 0.05f && idTex.GetPixel(xOffset + i, yOffset + j) != Color.black) ++paintedPixels;
                    dirtMask.SetPixel(xOffset + i, yOffset + j, new Color(prod, prod, prod));
                }
            }
            dirtMask.Apply();
        }
        else
        {
            Hose.instance.SetState(false);
        }
        percent = Mathf.Min(1, (float)paintedPixels / totalPixelsToPaint);
    }

    void LaserUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            UIController.instance.SetLowerMenuState(false);
            Laser.instance.SetState(true);
            Laser.instance.LookAt(hit.point, hit.normal);
            Vector2 textureCoord = hit.textureCoord;

            int x = (int)(textureCoord.x * dirtMask.width);
            int y = (int)(textureCoord.y * dirtMask.height);

            if (Random.value < 0.1f) print($"pos = ({x}, {y})");

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
                    if (mask.g > 0.05 && prod < 0.05f && albedo != Color.black) ++paintedPixels;
                    dirtMask.SetPixel(xOffset + i, yOffset + j, new Color(prod, prod, prod));
                }
            }
            dirtMask.Apply();
        }
        else
        {
            Laser.instance.SetState(false);
        }
        percent = Mathf.Min(1, (float)paintedPixels / totalPixelsToPaint);
    }

    void SprayUpdate()
    {
        float heightPercent = 0.15f;
        UIController.instance.MoveCrosshair(Input.mousePosition, screenHeight * heightPercent * Vector2.up);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition + screenHeight * heightPercent * Vector3.up), out RaycastHit hit, 5, mask))
        {
            UIController.instance.SetLowerMenuState(false);
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
                    if (scratch.g > 0.05 && color < 0.05f) ++paintedPixels;
                    scratchMask.SetPixel(xOffset + i, yOffset + j, new Color(color, color, color));
                }
            }
            scratchMask.Apply();
        }
        else
        {
            Spray.instance.SetState(false);
        }
        percent = Mathf.Min(1, (float)paintedPixels / totalPixelsToPaint);
    }







    public void ResetMask()
    {
        //int paintPixels = 0;
        for (int i = 0; i < dirtMask.width; ++i)
        {
            for (int j = 0; j < dirtMask.height; ++j)
            {
                dirtMask.SetPixel(i, j, Color.white);
                scratchMask.SetPixel(i, j, Color.white);
                //if (idTex.GetPixel(i, j) != Color.black) paintPixels += 2;
            }
        }
        //totalPixelsToPaint = paintPixels;
        //print($"Total pixels to paint = {totalPixelsToPaint}");
        dirtMask.Apply();
        scratchMask.Apply();
    }

    public void ClearMask()
    {
        for (int i = 0; i < dirtMask.width; ++i)
        {
            for (int j = 0; j < dirtMask.height; ++j)
            {
                dirtMask.SetPixel(i, j, Color.black);
                scratchMask.SetPixel(i, j, Color.black);
            }
        }
        dirtMask.Apply();
        scratchMask.Apply();
    }

    public void ResetMaskAsync()
    {
        StartCoroutine(ResetMaskAsyncRoutine());
    }

    IEnumerator ResetMaskAsyncRoutine()
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
            if (i % 32 == 0) yield return null;
        }
        totalPixelsToPaint = paintPixels;
        print($"Total pixels to paint = {totalPixelsToPaint}");
        dirtMask.Apply();
        scratchMask.Apply();
    }
}
