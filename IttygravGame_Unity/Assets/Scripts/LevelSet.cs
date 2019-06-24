using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IttyGrave/LevelSet")]
public class LevelSet : ScriptableObject {

    public LevelRef[] LevelSequence;
    private int index = 0;

    public LevelRef GetNextLevel()
    {
        index = (index + 1)% LevelSequence.Length;
        return LevelSequence[index];
    }

    public LevelRef GetLevel(int index)
    {
        LevelRef level = null;
        if (index < LevelSequence.Length)
        {
            level = LevelSequence[index];
            this.index = index;
        }
        return level;
    }

    public LevelRef GetCurrentLevel()
    {
        return LevelSequence[index];
    }

    
}
