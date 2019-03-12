using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public static Warp S;
    public bool isAct;

    public List<GameObject> warpList = new List<GameObject>();

    private void Awake()
    {
        S = this;
        warpList.Add(GameObject.Find("Warp/0"));
        warpList.Add(GameObject.Find("Warp/1"));
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WarpPlayer(int n)
    {
        GameObject player = GameObject.Find("Player");
        Player.S.isWarp = true;

        if (n % 2 == 0)
        {            
            player.transform.position = warpList[n + 1].transform.position;
        }
        else
        {
            player.transform.position = warpList[n - 1].transform.position;
        }
    }
}
