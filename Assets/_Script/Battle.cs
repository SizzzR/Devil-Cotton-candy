using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public static Battle S;
    public bool isBattle;
    public bool isSelect;

    public int curPhase;
    int count;
    public List<GameObject> SelectLineList = new List<GameObject>();

    private void Awake()
    {
        S = this;
        isBattle = false;
        isSelect = false;
        curPhase = 0;
        count = 0;
        for (int i = 0; i < 3; i++)
        {
            SelectLineList.Add(transform.Find("BattleMap/BattleText/" + i).gameObject);
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if(isBattle)
        {

            if(isSelect)
            {

                if (Input.GetKeyDown(KeyCode.UpArrow)) 
                {
                    SelectLineList[count].SetActive(false);

                    if (count == 0)
                        count = 3;
                  
                    SelectLineList[count - 1].SetActive(true);

                    count -= 1;
                }

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    SelectLineList[count].SetActive(false);

                    if (count == 2)
                        count = -1;

                    SelectLineList[count + 1].SetActive(true);

                    count += 1;
                }

                else if(Input.GetKeyDown(KeyCode.Return))
                {
                    SelectLineList[count].SetActive(false);
                    
                    curPhase += 1;
                    TextSet();                   
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                   
                    TextSet();
                }
            }
        }

    }

    public void Init()
    {
        transform.Find("BattleMap").gameObject.SetActive(true);
        TextSet();

        isBattle = true;
    }

    void TextSet()
    {
        Text text = transform.Find("BattleMap/BattleText/TextL").GetComponent<Text>();
        text.text = GetText(curPhase);
    }

    string GetText(int n)
    {
        switch(n)
        {
            case 0:
                {
                    count = 0;
                    SelectLineList[0].SetActive(true);
                    SelectLineList[1].SetActive(false);
                    SelectLineList[2].SetActive(false);
                    isSelect = true;
                    return "▷     공격하기\n▷       춤추기\n▷     당황하기";
                }

            case 1:
                if(count == 0)
                {
                    SelectLineList[0].SetActive(true);
                    return "▷   도토리 던지기\n▷   돈까스 던지기\n▷   수타면 던지기";
                }
                else if (count == 1)
                {
                    isSelect = false;
                    curPhase = 0;
                    return "춤을 췄다.\n회의감에 잠겨 " + Player.S.name + "은 77 데미지를 입었다!...";
                }
                else if(count == 2)
                {
                    isSelect = false;
                    curPhase = 0;
                    return Player.S.name + " 은(는) 당황했다!\n아무일도 일어나지않았다.";
                }
                else
                    return "끼룩 1";


            case 2:
                {
                    if (count == 0)
                    {
                        isSelect = false;
                        curPhase = 0;
                        return Player.S.name + " 은(는) 도토리를 던졌다!\n";
                    }
                    else if (count == 1)
                    {
                        isSelect = false;
                        curPhase = 0;
                        return Player.S.name + " 은(는) 돈까스를 던졌다!\n";
                    }
                    else if (count == 2)
                    {
                        isSelect = false;
                        curPhase = 0;
                        return Player.S.name + " 은(는) 수타면을 던졌다!\n";
                    }
                    else
                        return "끼룩 2";
                }
              
            default:
                return "끼룩";
        }
    }
}
