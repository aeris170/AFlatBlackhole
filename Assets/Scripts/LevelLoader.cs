using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject ColorManager;

    public GameObject[] Levels;
    public int CurrentLevel;

    private GameObject currentLevel;

    private void Start()
    {
        LoadLevel(CurrentLevel);
    }

    private void LoadLevel(int i)
    {
        if (currentLevel != null) Destroy(currentLevel);
        if (i > Levels.Length - 1)
        {
            GameObject.Find("LevelCompleteText").GetComponent<Text>().enabled = false;
            GameObject.Find("NextLevelInText").GetComponent<Text>().enabled = false;
            GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
        }
        else
        {
            currentLevel = Instantiate(Levels[i]);
            ColorManager.GetComponent<ColorAssigner>().OnLevelLoad();
        }
    }

    public void NextLevel()
    {
        LoadLevel(++CurrentLevel);
    }
}
