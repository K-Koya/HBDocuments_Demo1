using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;    // Timeline をスクリプトからコントロールするために必要

/// <summary>
/// タイムラインシーケンスの開始・終了を補助するスクリプト
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class TimelineController : MonoBehaviour
{
    /// <summary>
    /// 該当のPlayableDirector
    /// </summary>
    PlayableDirector pd = default;

    [SerializeField, Tooltip("タイムライン開始時に、非アクティブ化するオブジェクト")]
    GameObject[] onStartEnableObjects = default;
    [SerializeField, Tooltip("タイムライン開始時に、アクティブ化するオブジェクト")]
    GameObject[] onStartAbleObjects = default;
    [SerializeField, Tooltip("タイムライン終了時に、非アクティブ化するオブジェクト")]
    GameObject[] onEndEnableObjects = default;
    [SerializeField, Tooltip("タイムライン終了時に、アクティブ化するオブジェクト")]
    GameObject[] onEndAbleObjects = default;


    // Start is called before the first frame update
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
        //Timeline開始時にアクティブ状態を変更するメソッドを決定
        pd.played += OnStart;
        //Timeline終了時にアクティブ状態を変更するメソッドを決定
        pd.stopped += OnEnd;

        //playOnAwakeのものなら、playedデリゲートが走らないため、明示的に実行
        if (pd.playOnAwake) OnStart(pd);
        //playOnAwakeでないものなら、Timeline開始時に出現させるオブジェクトを非アクティブにする
        else Array.ForEach(onStartAbleObjects, o => o.SetActive(false));
    }


    private void OnStart(PlayableDirector pd)
    {
        if (this.pd != pd) return;

        Array.ForEach(onStartEnableObjects, o => o.SetActive(false));
        Array.ForEach(onStartAbleObjects, o => o.SetActive(true));
    }

    private void OnEnd(PlayableDirector pd)
    {
        if (this.pd != pd) return;

        Array.ForEach(onEndEnableObjects, o => o.SetActive(false));
        Array.ForEach(onEndAbleObjects, o => o.SetActive(true));
    }
}
