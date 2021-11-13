using UnityEngine;

public class LevelController : SingletonMonoBehaviour<LevelController>
{
    [Header("Object")]
    public GameObject objectPrefab;

    [Header("Material Settings")]
    public float hueValue;
    public Color metalColor;

    [Header("Workshop")]
    public Transform workshop;

    GameObject level;
    private static readonly int HueProperty = Shader.PropertyToID("_Hue");
    private static readonly int MetalColorProperty = Shader.PropertyToID("_MetalColor");

    void Start()
    {
        //LoadGame();
    }


    public void LoadGame()
    {
        level = Instantiate(objectPrefab, workshop);
        MeshRef meshRef = level.GetComponent<MeshRef>();
        meshRef.meshRenderer.material.SetFloat(HueProperty, hueValue);
        meshRef.meshRenderer.material.SetColor(MetalColorProperty, metalColor);

        CleanController.instance.Setup(meshRef.IDTexture, meshRef.pixelsToPaint);
    }

    void Update()
    {
        
    }
}
