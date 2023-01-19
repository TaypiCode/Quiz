using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    private Save save = new Save();

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(){
        SaveProgress();
    }
#endif
    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {
        save.bestScore = _quest.BestScore;
        LeaderboardScript.SetLeaderboardValue(LeaderboardScript.Names.BestScore, _quest.BestScore);

        PlayerPrefs.SetString("SV", JsonUtility.ToJson(save));
        PlayerPrefs.Save();
    }
}
[Serializable]
public class Save
{
    public int bestScore;
}