using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static int CurrentRunScore;
    public static int Highscore {
        get {
            return PlayerPrefs.GetInt("Highscore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Highscore", value);
            PlayerPrefs.Save();
        }
    }
}