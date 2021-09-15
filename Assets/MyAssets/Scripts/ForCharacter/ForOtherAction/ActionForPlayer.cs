using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

/// <summary>
/// プレイヤー向け、攻撃・回復等、行動制御コンポーネント
/// </summary>
[RequireComponent(typeof(Status))]
public class ActionForPlayer : MyMonoBehaviour
{
    /// <summary>
    /// 入力情報を持つコンポーネント
    /// </summary>
    MyInputManager input = default;

    /// <summary>
    /// キャラクターの情報が格納されたコンポーネント
    /// </summary>
    Status status = default;

    /// <summary>
    /// キャラクターのアニメーションを制御するコンポーネント
    /// </summary>
    Animator animator = default;



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        status = GetComponent<Status>();
        input = FindObjectOfType<MyInputManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
