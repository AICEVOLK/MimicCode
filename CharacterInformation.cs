using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInformation : MonoBehaviour {
    public string name_J; // キャラクターの名前（日本語表示）
    public string name_E; // キャラクターの名前（英語表示）
    public Sprite icon; // キャラクター立ち絵表示
    public Sprite random_reverse; // 2Pがランダムを選んだ時のイメージ
    public bool reverse; // 反転するかどうか
    public Sprite another_color; // 2Pカラーのイメージ
}
