using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー用の照準移動処理
/// </summary>
public class AimMovement : MyMonoBehaviour
{
    /// <summary>
    /// メインカメラ用タグ
    /// </summary>
    [SerializeField]
    string mainCameraTag = "MainCamera";
    /// <summary>
    /// メインカメラコンポーネント
    /// </summary>
    Camera mainCamera = default;
    /// <summary>
    /// プレイヤー用タグ
    /// </summary>
    [SerializeField]
    string playerTag = "Player";
    /// <summary>
    /// プレイヤーステータス
    /// </summary>
    Status status = default;
    /// <summary>
    /// 照準を合わせている対象のステータス
    /// </summary>
    Status focusedStatus = default;

    /// <summary>
    /// コマンド名
    /// </summary>
    string commandName = "";

    /// <summary>
    /// 照準器が把握できるレイヤ
    /// </summary>
    [SerializeField, Tooltip("照準を合わせられるオブジェクトのレイヤーを指定する")]
    LayerMask aimHitLayer = default;

    /// <summary>
    /// 照準までの距離の実数値
    /// </summary>
    float distance = 0.0f;
    /// <summary>
    /// 照準までの距離の識別
    /// </summary>
    DistanceType distanceType = DistanceType.OutOfRange;

    /* プロパティ */
    public float Distance { get => distance; }
    public DistanceType DistType { get => distanceType; }
    public Status FocusedStatus { get => focusedStatus; }
    public Status Status { get => status; }
    public string CommandName { get => commandName; set => commandName = value; }



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        mainCamera = GameObject.FindWithTag(mainCameraTag).GetComponent<Camera>();
        status = GameObject.FindWithTag(playerTag).GetComponent<Status>();
    }


    void FixedUpdate()
    {
        if (IsPausing) return;

        //地面レイ検索用クラス
        RaycastHit rayhitGround = default;
        //Rayの地面への接触点
        Vector3 rayhitPos = Vector3.zero;

        //プレイヤー位置からカメラ前方方向に地面を探索
        if (Physics.Raycast(status.EyePoint.transform.position, mainCamera.transform.forward, out rayhitGround, status.LockMaxRange, aimHitLayer))
        {
            //確認できたら該当座標を保存
            rayhitPos = rayhitGround.point;

            //(所持していれば)対象のステータスコンポーネントを取得
            focusedStatus = rayhitGround.transform.GetComponent<Status>();
            
            //照準位置までの実数距離から識別値を設定
            if (distance < status.ComboCommonProximityRange)
            {
                distanceType = DistanceType.WithinProximity;
            }
            else if (distance < status.LockMaxRange)
            {
                distanceType = DistanceType.OutOfProximity;
            }
        }
        else
        {
            //確認できなければ、最大射程距離を参照
            rayhitPos = status.EyePoint.transform.position + mainCamera.transform.forward * status.LockMaxRange;
            //照準までの距離の識別値を射程外に
            distanceType = DistanceType.OutOfRange;
        }

        //照準を配置
        transform.position = rayhitPos;

        //照準位置までの距離を計算(各プレイヤーの最大射程距離を限界値とする)
        distance = Vector3.Distance(transform.position, status.EyePoint.transform.position);
    }
}

/// <summary>
/// 照準までの距離の識別値
/// </summary>
public enum DistanceType : byte
{
    /// <summary>
    /// 射程外
    /// </summary>
    OutOfRange,
    /// <summary>
    /// 近接攻撃範囲外
    /// </summary>
    OutOfProximity,
    /// <summary>
    /// 近接攻撃範囲内
    /// </summary>
    WithinProximity
}
