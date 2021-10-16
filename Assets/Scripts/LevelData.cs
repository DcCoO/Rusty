using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public GameObject meshPrefab;
    public Texture2D albedo, mask, normal;
    public float metallic, smoothness;
    public float rustyPercentage;
}
