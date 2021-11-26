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
    [SerializeField, Tooltip("照準画像表示用コンポーネント")]
    Image aimImage = default;

    [SerializeField, Tooltip("距離表示用テキストコンポーネント")]
    Text distanceText = default;

    [Header("照準画像用スプライト")]
    [SerializeField, Tooltip("特に効果のあるオブジェクトに照準していない時の照準画像")]
    Sprite aimSpriteCommon = default;

    [SerializeField, Tooltip("調べられるオブジェクトに照準している時の照準画像")]
    Sprite aimSpriteAbleToSearch = default;

    [SerializeField, Tooltip("攻撃可能なオブジェクトに照準しているの時の照準画像")]
    Sprite aimSpriteAbleToAttack = default;

    [Header("各射程距離におけるシンボルカラー")]
    [SerializeField, Tooltip("射程外の時に使用するシンボルカラー")]
    Color colorOutOfRange = default;

    [SerializeField, Tooltip("近接攻撃範囲外の時に使用するシンボルカラー")]
    Color colorOutOfProximity = default;

    [SerializeField, Tooltip("近接攻撃範囲内の時に使用するシンボルカラー")]
    Color colorWithinProximity = default;

    [Header("入力ナビゲーションオブジェクト")]
    [SerializeField, Tooltip("照準コマンド表示オブジェクト")]
    GameObject aimCommandNav = default;

    /// <summary>
    /// 照準コマンドテキスト
    /// </summary>
    Text aimCommandName = default;

    [SerializeField, Tooltip("コンボ攻撃入力ナビゲーション用オブジェクト")]
    GameObject comboAttackNav = default;


    // Start is called before the first frame update
    void Start()
    {
        TimelineInit();
        aimMovement = GetComponentInParent<AimMovement>();
        aimCommandName = aimCommandNav.GetComponentInChildren<Text>();
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
                    aimImage.color = colorOutOfRange;
                    break;
                }
            case DistanceType.OutOfProximity:
                {
                    distanceText.color = colorOutOfProximity;
                    aimImage.color = colorOutOfProximity;
                    break;
                }
            case DistanceType.WithinProximity:
                {
                    distanceText.color = colorWithinProximity;
                    aimImage.color = colorWithinProximity;
                    break;
                }
        }

        if (aimMovement.CommandName != null)
        {
            aimCommandNav.SetActive(true);
            aimCommandName.text = aimMovement.CommandName;
            aimImage.sprite = aimSpriteAbleToSearch;
        }
        else
        {
            aimCommandNav.SetActive(false);
            aimImage.sprite = aimSpriteCommon;
        }

        if (aimMovement.FocusedStatus && aimMovement.FocusedStatus.CompareTag("Enemy"))
        {
            aimImage.sprite = aimSpriteAbleToAttack;
        }
    }
}
