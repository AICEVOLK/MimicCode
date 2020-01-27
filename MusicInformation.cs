using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicInformation : MonoBehaviour {

    public AudioClip clip; // 曲を入れる
    public Sprite stage; // ステージ背景画像を入れる
    public int stg_num; // パネル番号
    public GameObject Stages;
    public MusicSceneController script;
    public Animator animator;

    // Start is called before the first frame update
    void Start() {
        Stages = GameObject.Find("/Canvas/Manager");
        script = Stages.GetComponent<MusicSceneController>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (script.stg_num == stg_num) {
            animator.SetBool("scale", true);
        } else {
            animator.SetBool("scale", false);
        }
    }

}
