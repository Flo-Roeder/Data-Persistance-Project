using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StoredData : MonoBehaviour
{

    public Text Playername;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadHighscore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static StoredData Instance;

    public string playerName;
    public string highScoreName;
    public int highscoreCount;
    private void Awake()
    {
        
    
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class SaveData
    {
        public string highScoreName;
        public int highscoreCount;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highScoreName = highScoreName;
        data.highscoreCount = highscoreCount;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScoreName = data.highScoreName;
            highscoreCount = data.highscoreCount;
        }
    }

}
