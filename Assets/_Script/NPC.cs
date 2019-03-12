using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool isQuest;
    public int progressNum;

    private void Awake()
    {
        isQuest = true;
        progressNum = 0;
    }



    // 대사를 넘기자
    public string QuestText()
    {
        string[] textArr = {
            "Player, 끼룩끼룩끼룩끼룩\n끼룩끼룩 오늘 9시 10분 두키섬" +
            "+Player, 야호\n야호\n야호" +
            "+Npc, 퀘스트가 있었는데요" +
            "+Npc, 없었습니다" +
            "+Player, 끼룩끼룩" +
            "+Player, 끼룩끼룩끼룩끼룩\n끼룩끼룩끼룩\n끼룩끼룩\n끼룩" +
            "+End"
        };

        return textArr[progressNum];
    }
}