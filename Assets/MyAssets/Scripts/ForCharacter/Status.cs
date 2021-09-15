using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各種能力値
/// </summary>
public class Status : MonoBehaviour
{
    /// <summary>
    /// 走行速度5[m/s]超過の時のベースになる旋回角度[deg/s]
    /// </summary>
    const float PERFORMANCE_ROTATE_SPEED_BY_RUN = 360.0f;
    /// <summary>
    /// 敏捷性[rapid]:500の時の走行最高速[m/s]
    /// </summary>
    const float PERFORMANCE_RATE_500_MAX_SPEED = 7.0f;
    /// <summary>
    /// 敏捷性[rapid]:1500の時の走行最高速[m/s]
    /// </summary>
    const float PERFORMANCE_RATE_1500_MAX_SPEED = 9.0f;
    /// <summary>
    /// 技術力[Technique]:500の時の、最高速走行時の最高旋回速度倍率
    /// </summary>
    const float PERFORMANCE_RATE_500_MAX_ROTATE_SPEED_RATE = 0.5f;
    /// <summary>
    /// 技術力[Technique]:1500の時の、最高速走行時の最高旋回速度倍率
    /// </summary>
    const float PERFORMANCE_RATE_1500_MAX_ROTATE_SPEED_RATE = 1.5f;
    /// <summary>
    /// 直接攻撃[attack]:500の時の走行加速度[m/s²]
    /// </summary>
    const float PERFORMANCE_RATE_500_ACCELERATION = 12.0f;
    /// <summary>
    /// 直接攻撃[attack]:1500の時の走行加速度[m/s²]
    /// </summary>
    const float PERFORMANCE_RATE_1500_ACCELERATION = 16.0f;
    /// <summary>
    /// 直接防御[defense]:500の時の自然減速度[m/s²]
    /// </summary>
    const float PERFORMANCE_RATE_500_DECELERATION = -12.0f;
    /// <summary>
    /// 直接防御[defense]:1500の時の自然減速度[m/s²]
    /// </summary>
    const float PERFORMANCE_RATE_1500_DECELERATION = -16.0f;
    /// <summary>
    /// 敏捷性[rapid]:500の時のジャンプ初速度[m/s]
    /// </summary>
    const float PERFORMANCE_RATE_500_JUMP_FIRST_SPEED = 5.5f;
    /// <summary>
    /// 敏捷性[rapid]:1500の時のジャンプ初速度[m/s]
    /// </summary>
    const float PERFORMANCE_RATE_1500_JUMP_FIRST_SPEED = 7.7f;




    [Header("基本の情報")]
    /// <summary>
    /// キャラクター名(英語)
    /// </summary>
    [SerializeField, Tooltip("キャラクター名(英語)")]
    string characterNameEng = "";
    /// <summary>
    /// キャラクター名(日本語)
    /// </summary>
    [SerializeField, Tooltip("キャラクター名(日本語)")]
    string characterNameJpn = "";
    /// <summary>
    /// キャラクターの全長
    /// </summary>
    [SerializeField, Tooltip("キャラクターの全長(m)")]
    float height = 1.0f;
    /// <summary>
    /// キャラクターの胴囲半径
    /// </summary>
    [SerializeField, Tooltip("キャラクターの胴囲半径(m)")]
    float width = 1.0f;
    /// <summary>
    /// キャラクターの重さ
    /// </summary>
    [SerializeField, Tooltip("キャラクターの重さ(kg)")]
    float weight = 1.0f;
    /// <summary>
    /// 視点位置
    /// </summary>
    [SerializeField, Tooltip("キャラクターの視点の場所")]
    GameObject eyePoint = default;


    [Header("技能数値")]
    /// <summary>
    /// 最大HP
    /// </summary>
    [SerializeField, Range(500,2000), Tooltip("最大HP")]
    short maxHP = 1000;
    /// <summary>
    /// 直接攻撃
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("直接攻撃")]
    short attack = 1000;
    /// <summary>
    /// 直接防御
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("直接防御")]
    short defense = 1000;
    /// <summary>
    /// 間接攻撃
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("間接攻撃")]
    short magic = 1000;
    /// <summary>
    /// 間接防御
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("間接防御")]
    short shield = 1000;
    /// <summary>
    /// 敏捷性
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("敏捷性")]
    short rapid = 1000;
    /// <summary>
    /// 技術力
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("技術力")]
    short technique = 1000;
    /// <summary>
    /// 調子
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("調子")]
    short luck = 1000;


    [Header("補助技能数値")]
    /// <summary>
    /// 現在HP
    /// </summary>
    [SerializeField, Range(500, 2000), Tooltip("現在HP")]
    short nowHP = 1000;
    /// <summary>
    /// 最大MP
    /// </summary>
    [SerializeField, Tooltip("最大MP")]
    short maxMP = 1000;
    /// <summary>
    /// 現在MP
    /// </summary>
    [SerializeField, Tooltip("現在MP")]
    short nowMP = 1000;
    /// <summary>
    /// 最高歩行速度
    /// </summary>
    [SerializeField, Tooltip("最高歩行速度(m/s)")]
    float maxWalkSpeed = 1.0f;
    /// <summary>
    /// 最高走行速度
    /// </summary>
    [SerializeField, Tooltip("最高走行速度(m/s)")]
    float maxRunSpeed = 1.0f;
    /// <summary>
    /// 旋回最高速度
    /// </summary>
    [SerializeField, Tooltip("旋回最高速度(deg/s)")]
    float maxRotateSpeed = 1.0f;
    /// <summary>
    /// 走行加速度
    /// </summary>
    [SerializeField, Tooltip("走行加速度(m/s^2)")]
    float runAcceleration = 1.0f;
    /// <summary>
    /// 走行減速度
    /// </summary>
    [SerializeField, Tooltip("走行減速度(m/s^2)")]
    float runDeceleration = 1.0f;
    /// <summary>
    /// ジャンプ初速度
    /// </summary>
    [SerializeField, Tooltip("ジャンプ初速度(m/s)")]
    float jumpFirstSpeed = 1.0f;
    /// <summary>
    /// 180°旋回の時は左方向に旋回する = True
    /// </summary>
    [SerializeField, Tooltip("180°旋回の時は左方向に旋回する = True")]
    bool isTurnLeft = false;



    /// <summary>
    /// マーキング可能な距離およびCPの視界距離
    /// </summary>
    [SerializeField]
    float lockMaxRange = 1.0f;
    /// <summary>
    /// 通常コンボ近接攻撃の標準射程距離
    /// </summary>
    [SerializeField]
    float comboCommonProximityRange = 1.0f;
    /// <summary>
    /// 通常コンボ近接攻撃の遠めの射程距離
    /// </summary>
    [SerializeField]
    float comboFarProximityRange = 1.0f;
    /// <summary>
    /// 重力ベクトル
    /// </summary>
    [SerializeField]
    Vector3 gravitySize = new Vector3(0.0f, -9.8f, 0.0f);


    [Header("各種フラグメント")]
    /// <summary>
    /// 接地しているか
    /// </summary>
    [SerializeField]
    bool isGrounded = false;
    /// <summary>
    /// ジャンプ中か
    /// </summary>
    [SerializeField]
    bool isJumping = false;

    /// <summary>
    /// コマンド実行中か
    /// </summary>
    [SerializeField]
    bool isCommandRunning = false;
    /// <summary>
    /// ダメージを受けてひるんでいる状態か
    /// </summary>
    [SerializeField]
    bool isDamaging = false;

    /// <summary>
    /// 照準システム
    /// </summary>
    AimSystemForPlayer aim = default;

    /// <summary>
    /// 客観的に見た速度
    /// </summary>
    [SerializeField]
    float resultSpeed = 0.0f;

    /* プロパティ */
    public string CharacterNameEng { get => characterNameEng; set => characterNameEng = value; }
    public string CharacterNameJpn { get => characterNameJpn; set => characterNameJpn = value; }
    public float Height { get => height; set => height = value; }
    public float Width { get => width; set => width = value; }
    public float Weight { get => weight; set => weight = value; }
    public GameObject EyePoint { get => eyePoint; set => eyePoint = value; }
    public short MaxHP { get => maxHP; set => maxHP = value; }
    public short Attack { get => attack; set => attack = value; }
    public short Defense { get => defense; set => defense = value; }
    public short Magic { get => magic; set => magic = value; }
    public short Shield { get => shield; set => shield = value; }
    public short Rapid { get => rapid; set => rapid = value; }
    public short Technique { get => technique; set => technique = value; }
    public short Luck { get => luck; set => luck = value; }
    public short NowHP { get => nowHP; set => nowHP = value; }
    public short MaxMP { get => maxMP; set => maxMP = value; }
    public short NowMP { get => nowMP; set => nowMP = value; }
    public float MaxWalkSpeed { get => maxWalkSpeed; set => maxWalkSpeed = value; }
    public float MaxRunSpeed { get => maxRunSpeed; set => maxRunSpeed = value; }
    public float MaxRotateSpeed { get => maxRotateSpeed; set => maxRotateSpeed = value; }
    public float RunAcceleration { get => runAcceleration; set => runAcceleration = value; }
    public float RunDeceleration { get => runDeceleration; set => runDeceleration = value; }
    public float JumpFirstSpeed { get => jumpFirstSpeed; set => jumpFirstSpeed = value; }
    public bool IsTurnLeft { get => isTurnLeft; set => isTurnLeft = value; }
    public float LockMaxRange { get => lockMaxRange; set => lockMaxRange = value; }
    public float ComboCommonProximityRange { get => comboCommonProximityRange; set => comboCommonProximityRange = value; }
    public float ComboFarProximityRange { get => comboFarProximityRange; set => comboFarProximityRange = value; }
    public Vector3 GravitySize { get => gravitySize; set => gravitySize = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsCommandRunning { get => isCommandRunning; set => isCommandRunning = value; }
    public bool IsDamaging { get => isDamaging; set => isDamaging = value; }
    public float ResultSpeed { get => resultSpeed; set => resultSpeed = value; }
    public AimSystemForPlayer Aim { get => aim; set => aim = value; }
    



    /// <summary>
    /// 技能数値が500または1500の時の補助技能数値から、技能数値に対する補助技能数値を算出
    /// </summary>
    /// <param name="inStatus">技能数値</param>
    /// <param name="st500">技能数値が500の時の補助技能数値</param>
    /// <param name="st1500">技能数値が1500の時の補助技能数値</param>
    /// <returns>補助技能数値</returns>
    float calculateParameter500to1500(short inStatus, float st500, float st1500)
    {
        //傾き
        float tilt = (st1500 - st500) / 1000;
        //切片
        float segment = st1500 - (1500 * tilt);

        //傾き、切片、現在の基本ステータスより補助技能数値を算出
        return (inStatus * tilt) + segment;
    }

    /// <summary>
    /// 補助技能数値を決定
    /// </summary>
    void submitSubParameter()
    {
        //最大MP = (間接攻撃 ＋ 間接防御) ÷ 10
        maxMP = (short)((magic + shield) / 10);

        //現在HPを最大値に
        if (nowHP > maxHP) nowHP = maxHP;
        //現在MPを最大値に
        if (nowMP > maxMP) nowMP = maxMP;

        //最高速を敏捷値より算出
        maxRunSpeed = calculateParameter500to1500(rapid
            , PERFORMANCE_RATE_500_MAX_SPEED
            , PERFORMANCE_RATE_1500_MAX_SPEED);

        //加速度を直接攻撃より算出
        runAcceleration = calculateParameter500to1500(attack
            , PERFORMANCE_RATE_500_ACCELERATION
            , PERFORMANCE_RATE_1500_ACCELERATION);

        //自然減速度を直接防御より算出
        runDeceleration = calculateParameter500to1500(defense
            , PERFORMANCE_RATE_500_DECELERATION
            , PERFORMANCE_RATE_1500_DECELERATION);

        //ジャンプ初速度を敏捷値より算出
        jumpFirstSpeed = calculateParameter500to1500(rapid
            , PERFORMANCE_RATE_500_JUMP_FIRST_SPEED
            , PERFORMANCE_RATE_1500_JUMP_FIRST_SPEED);

        //走行時の旋回角度を技術力より算出
        maxRotateSpeed = PERFORMANCE_ROTATE_SPEED_BY_RUN
                           * calculateParameter500to1500(technique
                           , PERFORMANCE_RATE_500_MAX_ROTATE_SPEED_RATE
                           , PERFORMANCE_RATE_1500_MAX_ROTATE_SPEED_RATE);

    }

    

    // Start is called before the first frame update
    void Start()
    {
        //補助技能数値を決定
        submitSubParameter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
