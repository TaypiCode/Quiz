using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class QuestScriptable : ScriptableObject
{
    [ScriptableObjectId] public string itemId;
    [SerializeField] private string _questText;
    [SerializeField] private string[] _answers = new string[4];
    [SerializeField] private int _rightAnswerId = 0;

    public string QuestText { get => _questText;  }
    public string[] Answers { get => _answers;  }
    public int RightAnswerId { get => _rightAnswerId; }
}