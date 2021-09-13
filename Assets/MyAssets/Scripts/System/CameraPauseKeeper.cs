using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Chronos;

/// <summary>
/// Cinemachineを含め、カメラ動作を一時停止させるコンポーネント
/// </summary>
[RequireComponent(typeof(Timeline))]
public class CameraPauseKeeper : MyMonoBehaviour
{
    /// <summary>
    /// ポーズ実施によりCinemachine等を非アクティブにするメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent cameraPauseActivate = default;
    /// <summary>
    /// ポーズ解除によりCinemachine等をアクティブにするメソッドを定義する
    /// </summary>
    [SerializeField]
    UnityEvent cameraPauseInactivate = default;

    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ状態によりCinemachine等を、アクティブ・非アクティブにするメソッドを実行
        if (IsPausing) cameraPauseActivate.Invoke();
        else cameraPauseInactivate.Invoke();
    }
}
