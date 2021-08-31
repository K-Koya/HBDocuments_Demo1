using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 照準用キャンバス
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
    /// 地面レイヤ
    /// </summary>
    [SerializeField]
    LayerMask groundLayer = default;



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
        if (Physics.Raycast(status.EyePoint.transform.position, mainCamera.transform.forward, out rayhitGround, status.LockMaxRange, groundLayer))
        {
            //確認できたら該当座標を保存
            rayhitPos = rayhitGround.point;
        }
        else
        {
            //確認できなければ、最大射程距離を参照
            rayhitPos = status.EyePoint.transform.position + mainCamera.transform.forward * status.LockMaxRange;
        }

        //照準を配置
        transform.position = rayhitPos;
    }
}
