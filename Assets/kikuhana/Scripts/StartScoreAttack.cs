
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class StartScoreAttack : UdonSharpBehaviour
{
    //スコアアタックシステム
    //BGMの制御もここで行う
    //グローバル同期処理を行い、観戦できるようにする

    public AudioClip _scoreAttackBGM;
    public GameObject EnemyObject;
    public GameObject RetrySwith;
    public Text countdownText;
    public Text scoreText;
    public AudioClip[] countdownVoice;
    public AudioClip finishAudio;
    public const int limitTime = 60;
    public const float countDownTime = 5.0f;
    [NonSerialized] public bool isScoreAttack = false;
    [UdonSynced] public int syncScore = 0;

    AudioSource _audioSource;
    float timer = countDownTime;
    int countdownVoiceIndex = 0;

    void Start()
    {
        _audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        EnemyObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    void Update()
    {
        // 開始前カウントダウン処理
        if (countdownText.gameObject.activeSelf)
        {
            if (timer > 0)
            {
                // 1秒ごとにカウントダウンする
                timer -= Time.deltaTime;
                countdownText.text = String.Format("{0:0.00}",timer);
            }
            else
            {
                countdownText.gameObject.SetActive(false);
            }
        }
    }

    public override void Interact()
    {
        if (isScoreAttack)
        {
            // スコアアタック中は何もしない
            return;
        }

        // カウントダウンを開始する, グローバル同期処理を行う:オーナーでなければオーナーになる
        if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(StartCountdown));
    }

    public void StartCountdown()
    {
        // スコアアタックを開始する
        isScoreAttack = true;

        // 再試行用：スコアを非表示
        scoreText.gameObject.SetActive(false);

        // カウントダウンを開始する
        countdownText.gameObject.SetActive(true);

        // カウントダウン音声を1秒ごとに再生する
        foreach (var audio in countdownVoice)
        {
            SendCustomEventDelayedSeconds(nameof(PlayCountdownVoice), Array.IndexOf(countdownVoice, audio));
        }

        // スコアアタックを開始する
        SendCustomEventDelayedSeconds("StartScoreAttackSystem", 5);
    }

    public void PlayCountdownVoice()
    {
        // カウントダウン音声を再生する
        _audioSource.PlayOneShot(countdownVoice[countdownVoiceIndex]);
        countdownVoiceIndex++;
    }

    public void StartScoreAttackSystem()
    {
        // スコアを表示する
        scoreText.gameObject.SetActive(true);

        // スコアを初期化する
        ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.score = 0;

        // スコアアタック用のBGMを再生する
        _audioSource.Stop();
        _audioSource.PlayOneShot(_scoreAttackBGM);

        // 敵の出現を開始する
        EnemyObject.SetActive(true);

        // Retry()を呼び出す:壊れた建物を復活させる
        RetrySwitch retrySwitch = RetrySwith.GetComponent<RetrySwitch>();
        retrySwitch.Retry();

        // 終了時の処理を呼び出す
        SendCustomEventDelayedSeconds(nameof(EndScoreAttackSystem), limitTime);
    }

    public void EndScoreAttackSystem()
    {
        // スコアアタックを終了する
        isScoreAttack = false;

        // カウントダウン用変数を初期化する
        timer = countDownTime;
        countdownVoiceIndex = 0;

        // スコアアタック用のBGMを停止
        _audioSource.Stop();

        // 終了音声を再生する
        _audioSource.PlayOneShot(finishAudio);

        // 敵の出現を停止する
        EnemyObject.SetActive(false);

        // スコアを記録する関数をオーナーのみ呼び出しする
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(ScoreSync));
    }

    public void ScoreSync()
    {
        // オーナーでスコアを記録、Allでオーナーのスコアを他プレイヤーと同期する
        ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        syncScore = scoreManager.score;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(FinishScoreSync));
    }

    public void FinishScoreSync()
    {
        ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.FinishScore = syncScore;
    }
}
