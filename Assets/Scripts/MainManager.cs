using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]

public class MainManager : MonoBehaviour

{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text Highscore;
    public Text Playername;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);


        if (SceneManager.GetActiveScene().name=="main")
        {
            ScoreText.gameObject.SetActive(true);
            ScoreText.text = $"Score : " + StoredData.Instance.playerName + ": 0";
        } else if (SceneManager.GetActiveScene().name=="menu")
        {
            ScoreText.gameObject.SetActive(false);
            
        }

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        Highscore.text = "Best Score: " + StoredData.Instance.highScoreName + ": " + StoredData.Instance.highscoreCount;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : "+StoredData.Instance.playerName+" "+m_Points;
    }

   

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(m_Points>StoredData.Instance.highscoreCount)
        {
            StoredData.Instance.highscoreCount = m_Points;
            StoredData.Instance.highScoreName = StoredData.Instance.playerName;
            Highscore.text = "Best Score: " + StoredData.Instance.highScoreName + ": " + StoredData.Instance.highscoreCount;
            StoredData.Instance.SaveHighscore();
        }
    }

    public void SartGame()
    {
        StoredData.Instance.playerName = Playername.text.ToString();
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public void OpenMenu()
    {
        StoredData.Instance.playerName = "";
        SceneManager.LoadScene(0);
        
    }
    

   

}
