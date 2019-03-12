using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player S;
    public float movePower;
    public bool isForest;
    public bool isWarp;
    public List<GameObject> triggerObjList = new List<GameObject>();
    public GameObject triggerNPC;
    public GameObject triggerEnemy;

    public GameObject TextUIObj;

    public int sortingOrder;
    private SpriteRenderer sprite;
    private Transform playerHeart;
    public int warpNum;
    
   
    string playerName;
    
    enum State
    {
        Idle,
        Text,
        Battle,
    }

    State myState;

    private void Awake()
    {
        S = this;

        movePower = 6.0f;
        isForest = false;
        isWarp = false;

        sortingOrder = 5;

        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = sortingOrder;

        playerHeart = transform.Find("heart");
        
        myState = State.Idle;
        playerName = "Siz";
        
    }
    
    void Update()
    {
        // 플레이어의 위치를 가리킴 (회전)
        PlayerPositionHeart();

        // 근처에 NPC가 있다면 G키로 상호작용
        if (Input.GetKeyDown(KeyCode.G) && triggerNPC != null && myState != State.Text)
            StartCoroutine(InteractionWithNPC());
    }

    private void FixedUpdate()
    {
        if(myState == State.Idle)
            MoveControl();
    }

    void MoveControl()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
            moveVelocity = Vector3.up;
        else if (Input.GetAxisRaw("Vertical") < 0)
            moveVelocity = Vector3.down;

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 풀숲에 들어간다면 사운드 재생
        if (other.CompareTag("Forest") &&
            !triggerObjList.Contains(other.gameObject))
        {
            // 플레이어를 수풀 레이어 뒤로 가게 바꿈
            sprite.sortingOrder = -sortingOrder;
            // 사운드 재생
            SoundSc.S.PlayEntranceSound();

            triggerObjList.Add(other.gameObject);            
        }

        if (other.CompareTag("NPC"))
        {
            triggerNPC = other.gameObject;
        }

        if(other.CompareTag("Enemy"))
        {
            myState = State.Battle;
            GameObject canv = GameObject.Find("FixedCanvas");
            triggerEnemy = canv.transform.Find("RayImage").gameObject;
            StartCoroutine("EnemyBattle");
        }

        // 워프 포탈 사용
        else if (other.CompareTag("Warp"))
        {
            if (isWarp == false)
            {
                warpNum = int.Parse(other.name);
                Warp.S.WarpPlayer(warpNum);
            }
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if(triggerObjList.Contains(other.gameObject))
        {
            triggerObjList.Remove(other.gameObject);

            // 숲 오브젝트가 남아있지 않다면 플레이어 레이어를 되돌린다.
            if(triggerObjList.Count == 0)
            {
                sprite.sortingOrder = sortingOrder;
            }
        }

        if(other.CompareTag("NPC"))
        {
            triggerNPC = null;
        }

        // 워프 포탈 사용 가능상태로 변경
        else if (other.CompareTag("Warp"))
        {
            if (isWarp == true)
            {
                string warpStr = warpNum.ToString();
                if (other.name != warpStr) isWarp = false;
            }
        }
    }

    private void PlayerPositionHeart()
    {
        playerHeart.Rotate(Vector3.up, 1.0f, Space.Self);
    }


    IEnumerator InteractionWithNPC()
    {
        myState = State.Text;
        TextUIObj.SetActive(true);
        string qStr = triggerNPC.GetComponent<NPC>().QuestText();
        
        // 화자별로 분류
        char sp = '+';        
        string[] whoStr = qStr.Split(sp);

        int count = 0;
        while (count != whoStr.Length)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                sp = ',';
                string[] textStr = whoStr[count].Split(sp);
                Debug.Log(textStr[0]);

                if (textStr[0] == "End")
                {
                    TextUIObj.SetActive(false);
                    myState = State.Idle;
                    yield break;
                }

                else if (textStr[0] == "Player")
                {
                    PlayerOn();
                    
                }
                else
                {
                    NpcOn();
                }

                GameObject textObj = GameObject.Find("FixedCanvas/TextUI/TextImage/Text");
                textObj.GetComponent<Text>().text = textStr[1];

                count++;
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }

    // TODO 후에 NPC 다양해지면 NPC 번호를 넣어준다
    void PlayerOn()
    {
        GameObject temp = GameObject.Find("FixedCanvas/TextUI/Player/PlayerOff");
        temp.SetActive(false);
        temp = GameObject.Find("FixedCanvas/TextUI/Player/PlayerOn");
        temp.SetActive(true);


        temp = GameObject.Find("FixedCanvas/TextUI/Npc/NpcOff");
        temp.SetActive(true);
        temp = GameObject.Find("FixedCanvas/TextUI/Npc/NpcOn");
        temp.SetActive(false);

        temp = GameObject.Find("FixedCanvas/TextUI/Player/Name/Text");
        temp.GetComponent<Text>().text = "뉴비 " + playerName;
    }

    void NpcOn()
    {
        GameObject temp = GameObject.Find("FixedCanvas/TextUI/Player/PlayerOff");
        temp.SetActive(true);
        temp = GameObject.Find("FixedCanvas/TextUI/Player/PlayerOn");
        temp.SetActive(false);

        temp = GameObject.Find("FixedCanvas/TextUI/Npc/NpcOff");
        temp.SetActive(false);
        temp = GameObject.Find("FixedCanvas/TextUI/Npc/NpcOn");
        temp.SetActive(true);
    }

    IEnumerator EnemyBattle()
    {
        yield return new WaitForSeconds(0.2f);
        SoundSc.S.EnemyDetection();
        triggerEnemy.SetActive(true);
        yield return null;
    }
}

