using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public GameObject LevelPrefab;
    public float rustyPercentage;
    public float maxCleanPercentage; //when clean percentage reach this value, means the object is fullu clean
}
