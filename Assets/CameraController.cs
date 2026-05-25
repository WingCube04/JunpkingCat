using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    float checkPlayerPos;
    int floor=0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.player = GameObject.Find("climbCat");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position;
        Vector3 cameraPos = transform.position;

        if (playerPos.y>4f+(10f*floor))
        {
            transform.Translate(0, 10f, 0);
            floor ++;
        }
        else if (playerPos.y<-6f+(10f*floor))
        {
            transform.Translate(0, -10f, 0);
            floor --;
        }
    }
}
