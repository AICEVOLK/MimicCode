using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* 
    このスクリプトは先生が書いたMusicSelectScriptを元に書いている
    そのスクリプトを参考にして書き換えて何とか動くようにしたい
 */

public class MusicSceneController : MonoBehaviour
{
    /* 
        ここから下は絶対触るな
     */
    public static Sprite p1_image;
    public static Sprite p2_image;

    public static int p1_number;
    public static int p2_number;

    public GameObject Player1;
    public GameObject Player2;
    /* 
        ここから下はいじって良い
     */

    public int startImage = 0; // 20191104追加
    public float smoothing = 0.2f; // 20191104追加

    public GameObject counter;
    public GameObject[] Stages;

    public static bool initialize;
    
    public GameObject BGMImage;
    public Text BGMtitle;
    public static Sprite stage; // ステージ背景 ゲーム画面に引き継ぐ
    public static int bgm_num; // ゲーム画面に引き継ぐ
    public int stg_num; // bgm_numと同じ、staticがなくなっただけ
    public AudioSource bgm;
    public MusicInformation script;
    public AudioClip clip; // 曲
    public AudioClip select; // 移動時の効果音

    public ListView list;
    public float interval;

    public void Awake() {
        /* 
            Awakeの中は絶対触るな
         */

        p1_image = CharacterSceneController.p1_image(); // 前シーンで選んだ1Pのキャラクター情報
        p2_image = CharacterSceneController.p2_image(); // 前シーンで選んだ2Pのキャラクター情報

        Player1 = GameObject.Find("/Canvas/Manager/Players/Player1"); // ゲームオブジェクトを探す
        Player2 = GameObject.Find("/Canvas/Manager/Players/Player2"); // ゲームオブジェクトを探す

        Player1.GetComponent<Image>().sprite = p1_image; // 1Pが前シーンで選んだキャラクターイメージを代入
        Player2.GetComponent<Image>().sprite = p2_image; // 2Pが前シーンで選んだキャラクターイメージを代入

        p1_number = CharacterSceneController.p1_number();
        p2_number = CharacterSceneController.p2_number();
    }

    // Start is called before the first frame update
    void Start() {
        counter = GameObject.Find ("/Canvas/Manager/MusicSelectScene/Stages");
        
        if (initialize == true) { // 初期化するかどうか
            Debug.Log ("既に初期化されています。");
        } else {
            Debug.Log ("初期化しました。");
            bgm_num = 0;
        }
        Stages = new GameObject[counter.transform.childCount]; // 子オブジェクトを取得
        for (int i = 0; i < counter.transform.childCount; i++) {
            Stages[i] = counter.transform.GetChild(i).gameObject;
        }
        script = Stages[bgm_num].GetComponent<MusicInformation>(); // 取得した子オブジェクトを収納する

        stage = script.stage;
        BGMImage.GetComponent<Image>().sprite = stage;

        BGMtitle.text = Stages[bgm_num].transform.name;

        clip = script.clip; // BGM取得
        bgm.clip = clip;
        Debug.Log(script.clip);
        bgm.Play(); // BGM流す

        interval = list.interval; // パネル同士の間の間隔

        counterTarget = counter.transform.position;
        
    }

    public void Setting() {

        /* 
            今のところ出来てるからSetting()は触らないでほしい
         */
         
        if (bgm.clip == script.clip) { // BGMを鳴らす
            script = Stages[bgm_num].GetComponent<MusicInformation>();
            clip = script.clip;
            bgm.clip = clip;
            bgm.Play();
        }

        BGMtitle.text = Stages[bgm_num].transform.name;

        stage = script.stage;
        BGMImage.GetComponent<Image>().sprite = stage;

        stg_num = bgm_num;

    }

    Vector3 counterTarget = Vector3.zero; // 滑らかに移動するために必要な初期位置

    // Update is called once per frame
    void Update() {
        Debug.Log(bgm.clip);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (bgm_num > 0) {
                Debug.Log ("左に移動");
                bgm.PlayOneShot(select); // 移動時の効果音
                bgm_num -= 1;
                counterTarget.x += interval; // 滑らかに移動するために必要
                Setting();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if (bgm_num < Stages.Length - 1) {
                Debug.Log ("右に移動");
                bgm.PlayOneShot(select); // 移動時の効果音
                bgm_num += 1;
                counterTarget.x -= interval; // 滑らかに移動するために必要
                Setting();
            }
            
        }

        counter.transform.position = Vector3.Lerp(counter.transform.position, counterTarget, Time.deltaTime / 0.1f); // 滑らかに移動するために必要
        // おいLerp！貴様がどれだけ難しかったことかあああああああああああああああああ

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            /* 
                注意
                キャラ選択画面から曲選択画面への遷移アニメ中に
                Sキーを押すと遷移し終わってないのに
                ゲーム画面に遷移してしまうので
                ここでそれを直せるといいなぁ
             */
            SceneManager.LoadScene("MainGame"); // ここをあとでゲーム画面に遷移するように変える
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            /* 
                注意
                キャラ選択画面から曲選択画面への遷移アニメ中に
                Wキーを押すと遷移し終わってないのに
                キャラ選択画面に戻ってしまうので
                ここでそれを直せるといいなぁ
             */
            SceneManager.LoadScene("CharacterSelectScene"); // キャラ選択に戻る
        }

        Debug.Log(script.clip);
    }

    /* 
        AudioClip musicがゲーム画面に引き継がれたら成功
        追記：できたよ
     */

    public static int music() { // ゲーム画面に引き継ぐためのスクリプト
        return bgm_num;
    }

    /* 
        これより下絶対いじるな
     */

    public static Sprite p1_img() {
        return p1_image;
    }

    public static Sprite p2_img() {
        return p2_image;
    }

    public static int p1_num() {
        return p1_number;
    }

    public static int p2_num() {
        return p2_number;
    }

}
