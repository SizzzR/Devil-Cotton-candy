using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSc : MonoBehaviour
{
    public static SoundSc S;

    public AudioSource bush;
    public AudioSource enemyDetect;

    public AudioSource bgmGameland;
    public AudioSource bgmBattle;


    private void Awake()
    {
        S = this;
        BgmGameland();
    }

    public void PlayEntranceSound()
    {
        bush.Play();
    }

    public void EnemyDetection()
    {
        bgmGameland.Stop();
        enemyDetect.Play();

        StartCoroutine("BattleBGM");
    }

    public void BgmGameland()
    {
        bgmGameland.Play();
    }

    IEnumerator BattleBGM()
    {
        while(true)
        {
            if(!enemyDetect.isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
                bgmBattle.Play();

                Battle.S.Init();                         
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
