using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面に描画する照準器を操作する
/// </summary>
public class AimDrawer : MyMonoBehaviour
{
    /// <summary>
    /// 照準器の描画位置を計算するコンポーネント
    /// </summary>
    AimMovement aimMovement = default;

    [Header("UIコンポーネント")]
    /// <summary>
    /// 照準画像表示用コンポーネント
    /// </summary>
    [SerializeField, Tooltip("照準画像表示用コンポーネント")]
    Image aimImage = default;
    /// <summary>
    /// 最大射程距離まで距離表示を行ってくれるテキスト
    /// </summary>
    [SerializeField, Tooltip("距離表示用テキストコンポーネント")]
    Text distanceText = default;

    [Header("照準画像用スプライト")]
    /// <summary>
    /// 射程外の時の照準画像
    /// </summary>
    [SerializeField, Tooltip("射程外の時の照準画像")]
    Sprite aimSpriteOutOfRange = default;
    /// <summary>
    /// 近接攻撃範囲外の時の照準画像
    /// </summary>
    [SerializeField, Tooltip("近接攻撃範囲外の時の照準画像")]
    Sprite aimSpriteOutOfProximity = default;
    /// <summary>
    /// 近接攻撃範囲内の時の照準画像
    /// </summary>
    [SerializeField, Tooltip("近接攻撃範囲内の時の照準画像")]
    Sprite aimSpriteWithinProximity = default;

    [Header("各射程距離におけるシンボルカラー")]
    /// <summary>
    /// 射程外の時に使用するシンボルカラー
    /// </summary>
    [SerializeField, Tooltip("射程外の時に使用するシンボルカラー")]
    Color colorOutOfRange = default;
    /// <summary>
    /// 近接攻撃範囲外の時に使用するシンボルカラー
    /// </summary>
    [SerializeField, Tooltip("近接攻撃範囲外の時に使用するシンボルカラー")]
    Color colorOutOfProximity = default;
    /// <summary>
    /// 近接攻撃範囲内の時に使用するシンボルカラー
    /// </summary>
    [SerializeField, Tooltip("近接攻撃範囲内の時に使用するシンボルカラー")]
    Color colorWithinProximity = default;



    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        aimMovement = GetComponentInParent<AimMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPausing) return;

        //距離実数値を表示
        distanceText.text = aimMovement.Distance.ToString("F2") + "m";

        //距離の識別値に応じて、距離実数値のテキストカラーの設定および照準スプライトと色を指定
        switch (aimMovement.DistType)
        {
            case DistanceType.OutOfRange:
                {
                    distanceText.color = colorOutOfRange;
                    aimImage.sprite = aimSpriteOutOfRange;
                    aimImage.color = colorOutOfRange;
                    break;
                }
            case DistanceType.OutOfProximity:
                {
                    distanceText.color = colorOutOfProximity;
                    aimImage.sprite = aimSpriteOutOfProximity;
                    aimImage.color = colorOutOfProximity;
                    break;
                }
            case DistanceType.WithinProximity:
                {
                    distanceText.color = colorWithinProximity;
                    aimImage.sprite = aimSpriteWithinProximity;
                    aimImage.color = colorWithinProximity;
                    break;
                }
        }

    }
}
