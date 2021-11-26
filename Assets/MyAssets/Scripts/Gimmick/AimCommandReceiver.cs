using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//照準を合わせた際に発生するコマンドを取り扱う
public class AimCommandReceiver : MonoBehaviour
{
    [SerializeField, Tooltip("コマンド名")]
    string commandName = "起動する";

    [SerializeField, Tooltip("コマンド入力が許可される距離")]
    float reactionDistance = 1.0f;

    [SerializeField, Tooltip("ボタンを押すことで実行されるメソッドをアタッチ")]
    UnityEvent reactionMethod = default;


    public string CommandName { get => commandName; }
    public float ReactionDistance { get => reactionDistance; }
    

    /// <summary>
    /// 照準コマンドを実行する
    /// </summary>
    public void RunReaction()
    {
        reactionMethod.Invoke();
    }
}
