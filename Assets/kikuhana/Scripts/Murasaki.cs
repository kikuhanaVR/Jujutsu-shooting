
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Murasaki : UdonSharpBehaviour
{
    [SerializeField] private AudioClip[] bidgAudio; //4つの建物の音
    [SerializeField] private AudioClip peopleAudio; //人の音
    [SerializeField] private AudioClip[] enemyAudio; //3つの敵の音
    AudioSource bldgAudioSource;
    ScoreManager scoreManager;
    void Start()
    {
        bldgAudioSource = GameObject.Find("ExplodeSound").GetComponent<AudioSource>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // buildingに当たると建物を消す
        if (other.gameObject.name == "building")
        {
            //建物を非活性化
            other.gameObject.SetActive(false);

            //建物の音を0~3の乱数でランダムに鳴らす
            int random = Random.Range(0, 4);
            bldgAudioSource.PlayOneShot(bidgAudio[random]);

            //人の音を10%の確率で鳴らす
            int random2 = Random.Range(0, 10);
            if (random2 == 0)
            {
                bldgAudioSource.PlayOneShot(peopleAudio);
            }

            //スコアを加算
            scoreManager.score += scoreManager.scorePerBuilding;
        }

        // Enemyに当たると敵を消す
        if (other.gameObject.name == "Enemy")
        {
            //敵を非活性化
            other.gameObject.SetActive(false);

            //敵の音を0~2の乱数でランダムに鳴らす
            int random = Random.Range(0, 3);
            bldgAudioSource.PlayOneShot(enemyAudio[random]);

            //スコアを加算
            scoreManager.score += scoreManager.scorePerEnemy;
        }

    }
}
