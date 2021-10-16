using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : SingletonMonoBehaviour<LevelController>
{
    const string realLevelID = "RealLevel";
    const string showLevelID = "ShowLevel";


    int realLevel
    {
        get { return PlayerPrefs.GetInt(realLevelID, 0); }
        set { PlayerPrefs.GetInt(realLevelID, value); }
    }
    int showLevel
    {
        get { return PlayerPrefs.GetInt(showLevelID, 0); }
        set { PlayerPrefs.GetInt(showLevelID, value); }
    }
    [SerializeField] LevelData[] levels;

    protected override void Awake()
    {
        base.Awake();
        LoadLevel();
    }
    void Start()
    {
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel()
    {
        Instantiate(levels[0].LevelPrefab, Vector3.zero, Quaternion.identity);
        //RustyBreakController.instance.percent = levels[0].rustyPercentage;
        RustyBreakController.instance.Setup();
    }

    public void FinishLevel()
    {
        realLevel++;
        showLevel++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
