﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UIを扱う場合に必要
using UnityEngine.SceneManagement; // シーン移動するのに必要

public class CharacterSceneController : MonoBehaviour
{
    public GameObject counter; // 子オブジェクトにキャラクターがあるオブジェクトを取得
    public GameObject[] Panels; // キャラクターを配列するための入れ物
    public GameObject cursor1; // 1Pのカーソル
    public GameObject cursor2; // 2Pのカーソル
    public Sprite cursor1_img; // 1Pのカーソルのイメージ
    public Sprite cursor2_img; // 2Pのカーソルのイメージ
    public Sprite cursor_mix; // 1Pと2Pのカーソルが重なったときのイメージ
    public GameObject Player1; // 1Pのキャラ位置
    public GameObject Player2; // 2Pのキャラ位置

    /*
        p1_numとp2_numは一番大事
        この二つは絶対消すな
    */
    public static int p1_num; // 1Pの数値
    public static int p2_num; // 2Pの数値
 
    public Text name1; // 1Pの名前
    public Text name2; // 2Pの名前
    public Text name1_shadow; // 1Pの名前の影
    public Text name2_shadow; // 2Pの名前の影

    public static Sprite p1_img; // 1Pのキャラクターイメージ
    public static Sprite p2_img; // 2Pのキャラクターイメージ

    public CharacterInformation script1; // 1Pのキャラクター情報
    public CharacterInformation script2; // 2Pのキャラクター情報

    public GameObject dots1; // 1Pのドット
    public GameObject dots2; // 2Pのドット
    public GameObject bubble1; // 1Pのフキダシ
    public GameObject bubble2; // 2Pのフキダシ

    public Animator dot_anim1; // 1Pのドットアニメーション
    public Animator dot_anim2; // 2Pのドットアニメーション
    public Animator bubble_anim1; // 1Pの吹き出しアニメーション
    public Animator bubble_anim2; // 2Pの吹き出しアニメーション

    public bool bool1; // 1Pがキャラクターを選択したかどうか
    public bool bool2; // 2Pがキャラクターを選択したかどうか

    public AudioSource audioSource;
    public AudioClip select; // 左右移動音
    public AudioClip decision; // 決定音
    public AudioClip color_change; // カラー変更音
    public AudioClip cancel; // キャンセル音

    public Animator UImotion; // シーン移動の時のアニメーション

    public static bool initialize; // 初期化するかどうか

    public static bool another_color_p1; // 1Pのキャラクターの色を変更する
    public static bool another_color_p2; // 2Pのキャラクターの色を変更する

    // Start is called before the first frame update
    void Start() {
        counter = GameObject.Find ("/Canvas/Manager/CharacterSelectScene/Panels");
        Panels = new GameObject[counter.transform.childCount];
        for (int i = 0; i < counter.transform.childCount; i++) {
            Panels[i] = counter.transform.GetChild(i).gameObject;
        }
        
        if (initialize == true) { // 既に初期化していたら設定を保持する
            p1_num = MusicSceneController.p1_num();
            p2_num = MusicSceneController.p2_num();
        } else { // 初期化していなかったら値を入れる（初期化する）
            // initialize = true;
            p1_num = 0;
            p2_num = Panels.Length - 1;
        }

        cursor1.transform.position = Panels[p1_num].transform.position;
        cursor2.transform.position = Panels[p2_num].transform.position;
        

        dot_anim1 = dots1.GetComponent<Animator>(); // 1Pのドットのアニメーションを取得
        dot_anim2 = dots2.GetComponent<Animator>(); // 2Pのドットのアニメーションを取得

        bubble_anim1 = bubble1.GetComponent<Animator>();
        bubble_anim2 = bubble2.GetComponent<Animator>();

    }

    public void NameText() {
        name1.text = Panels[p1_num].transform.name;
        name2.text = Panels[p2_num].transform.name;
        
        name1_shadow.text = name1.text;
        name2_shadow.text = name2.text;
    }

    public void CharacterImage() {
        script1 = Panels[p1_num].GetComponent<CharacterInformation>();
        script2 = Panels[p2_num].GetComponent<CharacterInformation>();
        
        if (another_color_p1 == true) {
            p1_img = script1.another_color;
        } else {
            p1_img = script1.icon;
        }

        if (another_color_p2 == true) {
            if (script2.reverse == true) {
                p2_img = script2.random_reverse;
            } else {
                p2_img = script2.another_color;
            }
        } else {
            if (script2.reverse == true) {
                p2_img = script2.random_reverse;
            } else {
                p2_img = script2.icon;
            }

        }

        Player1.GetComponent<Image>().sprite = p1_img;
        // if (Panels[random_num].transform.position = cursor2.transform.position) {}
        Player2.GetComponent<Image>().sprite = p2_img;
    }

    public void CharacterIcon() { // キャラクターアイコンの明暗
        for (int i = 0; i < counter.transform.childCount; i++) {
            if (cursor1.transform.position == Panels[i].transform.position || cursor2.transform.position == Panels[i].transform.position) {
                Panels[i].GetComponent<Image>().color = Color.white;
            } else {
                Panels[i].GetComponent<Image>().color = Color.gray;
            }
        }

    }

    public void P1() {
        if (Input.GetKeyDown(KeyCode.A)) {
            if (bool1 == false) {
                p1_num -= 1;
                if (p1_num >= 0) {
                    another_color_p1 = false;
                    cursor1.transform.position = Panels[p1_num].transform.position;
                    audioSource.PlayOneShot(select);
                }

                if (p1_num < 0) {
                    p1_num = 0;
                }
                
            }
            
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            if (bool1 == false) {
                p1_num += 1;
                if (p1_num <= counter.transform.childCount - 1) {
                    another_color_p1 = false;
                    cursor1.transform.position = Panels[p1_num].transform.position;
                    audioSource.PlayOneShot(select);
                }

                if (p1_num > counter.transform.childCount - 1) {
                    p1_num = counter.transform.childCount - 1;
                }

            }
            
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            if (bool1 == false) {
                if (another_color_p1 == false) {
                    another_color_p1 = true;
                } else {
                    another_color_p1 = false;
                }
                audioSource.PlayOneShot(color_change);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            if (bool1 == false) {
                Debug.Log("Player1が選んだキャラは" + script1.name_J);
                bool1 = true;
                dot_anim1.SetBool("ok", true);
                audioSource.PlayOneShot(decision);
            } else {
                Debug.Log("Player1がキャラクターを選び直しました");
                bool1 = false;
                dot_anim1.SetBool("ok", false);
                audioSource.PlayOneShot(cancel);
            }
            
        }
    }

    public void P2() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (bool2 == false) {
                p2_num -= 1;
                if (p2_num >= 0) {
                    another_color_p2 = false;
                    cursor2.transform.position = Panels[p2_num].transform.position;
                    audioSource.PlayOneShot(select);
                }

                if (p2_num < 0) {
                    p2_num = 0;
                }

            }
            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (bool2 == false) {
                p2_num += 1;
                if (p2_num <= counter.transform.childCount - 1) {
                    another_color_p2 = false;
                    cursor2.transform.position = Panels[p2_num].transform.position;
                    audioSource.PlayOneShot(select);
                }

                if (p2_num > counter.transform.childCount - 1) {
                    p2_num = counter.transform.childCount - 1;
                }

            }
            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (bool2 == false) {
                if (another_color_p2 == false) {
                    another_color_p2 = true;
                } else {
                    another_color_p2 = false;
                }
                audioSource.PlayOneShot(color_change);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (bool2 == false) {
                Debug.Log("Player2が選んだキャラは" + script2.name_J);
                bool2 = true;
                dot_anim2.SetBool("ok", true);
                audioSource.PlayOneShot(decision);
            } else {
                Debug.Log("Player2がキャラクターを選びなおしました");
                bool2 = false;
                dot_anim2.SetBool("ok", false);
                audioSource.PlayOneShot(cancel);
            }
            
        }
    }

    public void CurosrImage() {
        if (p1_num == p2_num) {
            cursor1.GetComponent<Image>().sprite = cursor_mix;
            cursor2.GetComponent<Image>().sprite = cursor_mix;
        } else {
            cursor1.GetComponent<Image>().sprite = cursor1_img;
            cursor2.GetComponent<Image>().sprite = cursor2_img;
        }
    }

    // Update is called once per frame
    void Update() {
        NameText();
        CharacterIcon();
        CharacterImage();
        P1();
        P2();
        CurosrImage();

        if (bool1 == true && bool2 == true) {
            Debug.Log("曲選択へ");
            bubble_anim1.SetBool("go", true);
            bubble_anim2.SetBool("go", true);
            UImotion.SetBool("change", true);
            initialize = true;
            // audioSource.volume *= 1 - Time.deltaTime * 2.0f; // もしかしてこれいらない…？(´・ω・`)
            Invoke("ChangeScene", 1.5f);
        }
    }

    void ChangeScene() {
        SceneManager.LoadScene("MusicSelectScene");
    }
    
    public static Sprite p1_image() {
        return p1_img;
    }

    public static Sprite p2_image() {
        return p2_img;
    }

    public static int p1_number() {
        return p1_num;
    }

    public static int p2_number() {
        return p2_num;
    }

    public static bool p1_color() {
        return another_color_p1;
    }

    public static bool p2_color() {
        return another_color_p2;
    }

}
