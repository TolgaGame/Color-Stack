using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    #region Variables

    [SerializeField] LevelData[] levelDatas;

    LevelData currentLevelData;

    #endregion

    #region Other Methods

    public void SpawnLevel(int index)
    {
        if (index > levelDatas.Length - 1)
            index = GetRandomLevel();

        currentLevelData = levelDatas[index];
        Instantiate(currentLevelData.levelPrefab);
        PlayerPrefs.SetInt("LevelIndex", index);
    }

    int GetRandomLevel()
    {
        int index = Random.Range(0, levelDatas.Length);

        int lastLevel = PlayerPrefs.GetInt("LevelIndex");
        if (index == lastLevel)
            return GetRandomLevel();
        else
            return index;
    }

    #endregion
}
