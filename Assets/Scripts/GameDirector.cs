using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    public string clearTime;
    float currentTime;
    bool isTimerRunning;
    GameObject time;

    int minute;
    int seconds;
    int milliseconds;
    string timeString;

    public GameObject menuPanel;

    void Awake()
    {
        // 이 스크립트가 붙어있는 오브젝트(gameObject)는 씬이 바뀌어도 파괴하지 말라는 유니티 정석 명령입니다.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.time = GameObject.Find("Time");
        //clearTime = 0f;
        clearTime = "";
        currentTime = 0f;
        isTimerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this == null || time == null) 
        {
            return; 
        }
        if(isTimerRunning)
        {
            currentTime += Time.deltaTime;
        }
        minute = (int)(currentTime/60);
        seconds = (int)(currentTime % 60);
        milliseconds = (int)((currentTime%1)*100);
        timeString = string.Format("{0:D2}:{1:D2}.{2:D2}", minute, seconds, milliseconds);
        this.time.GetComponent<TextMeshProUGUI>().text = timeString;
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = menuPanel.activeSelf;

            menuPanel.SetActive(!isActive);

            if(!isActive == true)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void GameClear()
    {
        isTimerRunning = false;
        clearTime = timeString;
        SceneManager.LoadScene("ClearScene");
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnToStartScene()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
        SceneManager.LoadScene("StartScene");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
    }
}
