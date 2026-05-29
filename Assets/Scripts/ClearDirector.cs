using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClearDirector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    GameObject gameDirector;
    GameObject clearTimeText;
    string clearTime;
    int minute;
    float second;
    string timeString;
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        clearTimeText = GameObject.Find("ClearTime");
        this.clearTime = gameDirector.GetComponent<GameDirector>().clearTime;
        //minute = (int)(this.clearTime/60);
        //second = this.clearTime%60;
        //timeString = string.Format("{0:D2}:{1:F2}",minute,second);

    }
    void Update()
    {
        clearTimeText.GetComponent<TextMeshProUGUI>().text = this.clearTime;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (gameDirector != null) 
            {
                Destroy(gameDirector); // 이제 볼일 끝났으니 GameScene에서 넘어온 껍데기를 파괴해서 청소합니다.
            }
            SceneManager.LoadScene("StartScene");
        }
    }
}
