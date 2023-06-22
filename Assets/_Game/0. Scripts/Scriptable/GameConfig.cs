using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// I Am not Using It In This Game, 
/// But I Put It Here Because When I Will Extend The Game I Will Probably
/// Use It.
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public int numOfStudentsInRooms;
    [SerializeField] int maxStudent;
    public int maxStudentCapacity
    {
        get { return maxStudent; }
    }
    //public List<ThemeMaterial> themes;
    //public Material boarderMaterial;
    //public Material groundMaterial;

    //public void Init()
    //{
    //    int themeIndex = UnityEngine.Random.Range(0, themes.Count - 1);
    //    SetScene(themeIndex);
    //}

    //public void SetScene(int themeIndex)
    //{
    //    boarderMaterial.color = themes[themeIndex].boarderColor;
    //    boarderMaterial.SetColor("_HColor", themes[themeIndex].highlightColor);
    //    boarderMaterial.SetColor("_SColor", themes[themeIndex].shadowColor);
    //    groundMaterial.color = themes[themeIndex].backgroundColor;
    //}
}

[Serializable]
public struct ThemeMaterial
{
    public Color backgroundColor;
    public Color boarderColor;
    public Color highlightColor;
    public Color shadowColor;
}
