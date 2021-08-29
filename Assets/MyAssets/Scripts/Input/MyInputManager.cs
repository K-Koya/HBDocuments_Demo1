using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入力状態
/// </summary>
public enum PushType : byte
{
    /// <summary>
    /// 未入力
    /// </summary>
    noPush = 0,
    /// <summary>
    /// 1回押し
    /// </summary>
    onePush = 1,
    /// <summary>
    /// 1回長押し
    /// </summary>
    longPush = 2,
    /// <summary>
    /// 2度押し
    /// </summary>
    doublePush = 3
};


/// <summary>
/// 操作入力管理クラス
/// </summary>
public class MyInputManager : MonoBehaviour
{
    /// <summary>
    /// Inputmanager上の、ジャンプに当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameJump = "Jump";
    /// <summary>
    /// Inputmanager上の、決定に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameSubmit = "Submit";
    /// <summary>
    /// Inputmanager上の、キャンセルに当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCancel = "Cancel";
    /// <summary>
    /// Inputmanager上の、メニューに当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameMenu = "Menu";
    /// <summary>
    /// Inputmanager上の、カメラズームフラグに当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraZoomFlag = "CameraZoomFlag";
    /// <summary>
    /// Inputmanager上の、カメラズームのキー入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraZoomKey = "CameraZoomKey";
    /// <summary>
    /// Inputmanager上の、カメラズームのスクロール入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraZoomWheel = "CameraZoomWheel";
    /// <summary>
    /// Inputmanager上の、移動の前後のキー入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameMoveVerticalKey = "MoveVerticalKey";
    /// <summary>
    /// Inputmanager上の、移動の左右のキー入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameMoveHorizontalKey = "MoveHorizontalKey";
    /// <summary>
    /// Inputmanager上の、移動の前後のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameMoveVerticalStick = "MoveVerticalStick";
    /// <summary>
    /// Inputmanager上の、移動の左右のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameMoveHorizontalStick = "MoveHorizontalStick";
    /// <summary>
    /// Inputmanager上の、カメラスイングの上下のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraSwingVerticalStick = "CameraSwingVerticalStick";
    /// <summary>
    /// Inputmanager上の、カメラスイングの左右のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraSwingHorizontalStick = "CameraSwingHorizontalStick";
    /// <summary>
    /// Inputmanager上の、カメラスイングの上下のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraSwingVerticalMouse = "CameraSwingVerticalMouse";
    /// <summary>
    /// Inputmanager上の、カメラスイングの左右のスティック入力に当たるボタン名
    /// </summary>
    [SerializeField]
    string buttonNameCameraSwingHorizontalMouse = "CameraSwingHorizontalMouse";



    /// <summary>
    /// 入力:ジャンプ
    /// </summary>
    InputKind jump = new InputKind();
    /// <summary>
    /// 入力:決定
    /// </summary>
    InputKind submit = new InputKind();
    /// <summary>
    /// 入力:キャンセル
    /// </summary>
    InputKind cancel = new InputKind();
    /// <summary>
    /// 入力:メニュー
    /// </summary>
    InputKind menu = new InputKind();
    /// <summary>
    /// 入力:カメラズームを行うフラグボタン
    /// </summary>
    InputKind cameraZoomFlag = new InputKind();


    /// <summary>
    /// 入力:カメラズーム・スクロール入力キー入力情報
    /// </summary>
    InputKind cameraZoomKey = new InputKind();
    /// <summary>
    /// 入力:カメラズーム・スクロール入力マウスホイール情報
    /// </summary>
    InputKind cameraZoomScroll = new InputKind();


    /// <summary>
    /// 入力:移動
    /// </summary>
    InputKind move = new InputKind();
    /// <summary>
    /// 入力:カメラスイング
    /// </summary>
    InputKind cameraSwing = new InputKind();


    /*
    /// <summary>
    /// スマートフォン等のタッチによるバーチャル入力を管理するゲームオブジェクト
    /// </summary>
    [SerializeField]
    GameObject virtualInput = default;
    /// <summary>
    /// スマートフォン等のタッチによるバーチャル入力を使うか
    /// </summary>
    [SerializeField]
    bool isUseVirtualInput = false;
    
    /// <summary>
    /// バーチャル入力のポーズ入力に使うButtonEvent
    /// </summary>
    [SerializeField]
    ButtonEvent virtualButtonPause = default;
    /// <summary>
    /// バーチャル入力のジャンプ入力に使うButtonEvent
    /// </summary>
    [SerializeField]
    ButtonEvent virtualButtonJump = default;
    /// <summary>
    /// バーチャル入力の移動入力に使うJoystick
    /// </summary>
    [SerializeField]
    Joystick virtualStickMove = default;
    /// <summary>
    /// バーチャル入力のカメラスイング入力に使うJoystick
    /// </summary>
    [SerializeField]
    Joystick virtualStickCameraSwing = default;
    */


    /* プロパティ */
    public InputKind Jump { get => jump; }
    public InputKind Submit { get => submit; }
    public InputKind Cancel { get => cancel; }
    public InputKind Menu { get => menu; }
    public InputKind CameraZoomFlag { get => cameraZoomFlag; }
    public InputKind CameraZoomKey { get => cameraZoomKey; }
    public InputKind CameraZoomScroll { get => cameraZoomScroll; }
    public InputKind Move { get => move; }
    public InputKind CameraSwing { get => cameraSwing; }


    //public bool IsUseVirtualInput { get => isUseVirtualInput; }




    // Start is called before the first frame update
    void Start()
    {
        jump.Inputs.Add(new KeyClickButtonInput(buttonNameJump));
        //jump.Inputs.Add(new VirtualButtonInput(virtualButtonJump));
        submit.Inputs.Add(new KeyClickButtonInput(buttonNameSubmit));
        cancel.Inputs.Add(new KeyClickButtonInput(buttonNameCancel));
        menu.Inputs.Add(new KeyClickButtonInput(buttonNameMenu));
        //menu.Inputs.Add(new VirtualButtonInput(virtualButtonPause));
        //zoomFlag.Inputs.Add(new KeyClickButtonInput(BUTTON_NAME_ZOOM_FLAG));

        cameraZoomScroll.Inputs.Add(new MouseWheelInput(buttonNameCameraZoomWheel));
        cameraZoomScroll.Inputs.Add(new MouseWheelInput(buttonNameCameraZoomKey));

        move.Inputs.Add(new MouseMoveStickInput(buttonNameMoveVerticalKey, buttonNameMoveHorizontalKey));
        move.Inputs.Add(new MouseMoveStickInput(buttonNameMoveVerticalStick, buttonNameMoveHorizontalStick));
        //move.Inputs.Add(new VirtualStickInput(virtualStickMove));
        cameraSwing.Inputs.Add(new MouseMoveStickInput(buttonNameCameraSwingVerticalMouse, buttonNameCameraSwingHorizontalMouse));
        cameraSwing.Inputs.Add(new MouseMoveStickInput(buttonNameCameraSwingVerticalStick, buttonNameCameraSwingHorizontalStick));
        //cameraSwing.Inputs.Add(new VirtualStickInput(virtualStickCameraSwing));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //バーチャル入力オブジェクトを隠す
        virtualInput.SetActive(isUseVirtualInput);

        //バーチャル入力を使わない場合、無効にする
        AbstractInput.IsUseVirtualInput = isUseVirtualInput;
        */

        jump.management();
        submit.management();
        cancel.management();
        menu.management();

        cameraZoomScroll.management();

        move.management();
        cameraSwing.management();
    }
}




/// <summary>
/// ジャンプ、攻撃等の入力の種類
/// </summary>
public class InputKind
{
    /// <summary>
    /// 現在の入力状態
    /// </summary>
    PushType nowPushType = PushType.noPush;
    /// <summary>
    /// ボタンを押した直後の情報取得
    /// </summary>
    PushType nowPushDown = PushType.noPush;
    /// <summary>
    /// ボタンを離した直後の情報取得
    /// </summary>
    PushType nowPushUp = PushType.noPush;

    /// <summary>
    /// 現在の2D入力角
    /// </summary>
    Vector2 axis2d = Vector2.zero;



    /// <summary>
    /// 本入力種に対応する入力媒体リスト
    /// </summary>
    List<AbstractInput> inputs = new List<AbstractInput>();


    /*プロパティ*/
    public PushType NowPushType { get => nowPushType; set => nowPushType = value; }
    public PushType NowPushDown { get => nowPushDown; set => nowPushDown = value; }
    public PushType NowPushUp { get => nowPushUp; set => nowPushUp = value; }
    public Vector2 Axis2d { get => axis2d; set => axis2d = value; }
    public List<AbstractInput> Inputs { get => inputs; set => inputs = value; }

    /// <summary>
    /// 入力情報を読み取る
    /// </summary>
    public void management()
    {
        //フレーム毎に初期化
        nowPushType = PushType.noPush;
        nowPushDown = PushType.noPush;
        nowPushUp = PushType.noPush;
        axis2d = Vector2.zero;


        foreach (AbstractInput input in Inputs)
        {
            //入力情報を受け取る
            input.management();

            //複数の媒体のボタン入力を、優先度(enumをintにキャストした値)が高い順に反映
            nowPushType = (PushType)Mathf.Max((int)nowPushType, (int)input.NowPushType);
            nowPushDown = (PushType)Mathf.Max((int)nowPushDown, (int)input.NowPushDown);
            nowPushUp = (PushType)Mathf.Max((int)nowPushUp, (int)input.NowPushUp);

            //スティック等軸管理の入力を、傾きの大きい順に反映
            axis2d = axis2d.sqrMagnitude < input.Axis2d.sqrMagnitude ? input.Axis2d : axis2d;
        }
    }
}



/// <summary>
/// 入力管理用の抽象クラス
/// キー・マウス・ボタン・スティック・タッチ各操作媒体用に派生させること
/// </summary>
abstract public class AbstractInput
{
    /// <summary>
    /// バーチャル入力を利用するかのフラグ
    /// </summary>
    protected static bool isUseVirtualInput = true;


    /// <summary>
    /// AXIS処理における大きな入力値の減少を未入力とみなすマージン
    /// </summary>
    protected const float BUTTON_AXIS_DUMP_MARGIN = 0.3f;



    /// <summary>
    /// 長押し受付時間
    /// </summary>
    static float longPushMargin = 0.2f;
    /// <summary>
    /// 2度押し受付時間
    /// </summary>
    static float doublePushMargin = 0.2f;
    /// <summary>
    /// 同一の入力角とする角度差の最大値
    /// </summary>
    static float nearAxisMargin = 20.0f;

    /// <summary>
    /// InputManager上の名前
    /// </summary>
    protected string name = "";
    /// <summary>
    /// 2軸の入力の場合もう一つのInputManager上の名前
    /// </summary>
    protected string name2　="";
    /// <summary>
    /// 現在の入力状態
    /// </summary>
    protected PushType nowPushType = PushType.noPush;
    /// <summary>
    /// ボタンを押した直後の情報取得
    /// </summary>
    protected PushType nowPushDown = PushType.noPush;
    /// <summary>
    /// ボタンを離した直後の情報取得
    /// </summary>
    protected PushType nowPushUp = PushType.noPush;
    /// <summary>
    /// 入力時間
    /// </summary>
    protected float count = 0.0f;


    /// 現在の垂直入力角
    /// </summary>
    protected float vAxis = 0.0f;
    /// <summary>
    /// 現在の水平入力角
    /// </summary>
    protected float hAxis = 0.0f;
    /// <summary>
    /// 現在の2D入力角
    /// </summary>
    protected Vector2 axis2d = Vector2.zero;

    /// <summary>
    /// フレーム前の2D入力角
    /// </summary>
    protected Vector2 beforeAxis = Vector2.zero;



    /*プロパティ*/
    public static bool IsUseVirtualInput { get => isUseVirtualInput; set => isUseVirtualInput = value; }
    public static float LongPushMargin { get => longPushMargin; set => longPushMargin = value; }
    public static float DoublePushMargin { get => doublePushMargin; set => doublePushMargin = value; }
    public static float NearAxisMargin { get => nearAxisMargin; set => nearAxisMargin = value; }
    public PushType NowPushType { get => nowPushType; set => nowPushType = value; }
    public PushType NowPushDown { get => nowPushDown; set => nowPushDown = value; }
    public PushType NowPushUp { get => nowPushUp; set => nowPushUp = value; }
    public Vector2 Axis2d { get => axis2d; set => axis2d = value; }
    


    /// <summary>
    /// ボタン操作のマネジメント
    /// </summary>
    /// <param name="isInput"></param>
    protected void buttonManagement(bool isInput)
    {
        //押したか否か
        if (isInput)
        {
            //現在の押下状況
            switch (nowPushType)
            {
                case PushType.noPush:
                    {
                        if (count < 0.0f)
                        {
                            nowPushType = PushType.doublePush;

                        }
                        else
                        {
                            nowPushType = PushType.onePush;

                            count += Time.deltaTime;
                        }

                        break;
                    }
                case PushType.onePush:
                    {
                        if (count > LongPushMargin) nowPushType = PushType.longPush;

                        count += Time.deltaTime;

                        break;
                    }
                default:
                    {
                        break;
                    }

            }

        }
        else
        {
            switch (nowPushType)
            {
                case PushType.noPush:
                    {
                        if (count < 0.0f) count += Time.deltaTime;

                        break;
                    }
                case PushType.onePush:
                    {
                        nowPushType = PushType.noPush;

                        count = -DoublePushMargin;

                        break;
                    }
                default:
                    {
                        nowPushType = PushType.noPush;

                        count = 0.0f;

                        break;
                    }
            }
        }
    }

    /// <summary>
    /// 入力情報を読み取る抽象メソッド
    /// キー・マウス・ボタン・スティック・タッチ各操作用に派生させ入力管理
    /// </summary>
    abstract public void management();
}
/// <summary>
/// キー・クリック・ボタン用の入力管理クラス
/// </summary>
public class KeyClickButtonInput : AbstractInput
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">InputManager上の名前</param>
    public KeyClickButtonInput(string name) : base()
    {
        this.name = name;
    }

    /// <summary>
    /// キー・クリック・ボタンの入力管理
    /// </summary>
    override public void management()
    {
        if (name == null || name.Length == 0) return;

        nowPushUp = PushType.noPush;
        if (Input.GetButtonUp(name))
            nowPushUp = nowPushType;

        buttonManagement(Input.GetButton(name));

        nowPushDown = PushType.noPush;
        if (Input.GetButtonDown(name))
            nowPushDown = nowPushType;
    }
}

/// <summary>
/// マウス移動・スティック用の入力管理クラス
/// </summary>
public class MouseMoveStickInput : AbstractInput
{
    /// <summary>
    /// コンストラクタ
    /// name:縦軸 name2:横軸
    /// </summary>
    /// <param name="name">InputManager上の名前縦軸</param>
    /// <param name="name2">InputManager上の名前横軸</param>
    public MouseMoveStickInput(string name, string name2) : base()
    {
        this.name = name;
        this.name2 = name2;
    }

    /// <summary>
    /// マウス移動・スティックの入力管理
    /// </summary>
    override public void management()
    {
        //GetAxisより角度値を取得
        //名称が定義されていない場合はreturn
        if (name == null || name.Length == 0
            || name2 == null || name2.Length == 0)
        {
            return;
        }
        else
        {
            vAxis = Input.GetAxis(name);
            hAxis = Input.GetAxis(name2);
        }

        //操作入力フラグ
        bool isInput = false;

        //hv操作からaxis2dを算出するが、長さが1を超えるときは1に補正
        axis2d = new Vector2(hAxis, vAxis);
        if (axis2d.sqrMagnitude > 1.0f)
        {
            axis2d = axis2d.normalized;
            hAxis = axis2d.x;
            vAxis = axis2d.y;
        }

        //入力動作があるかを判定()
        if (axis2d.sqrMagnitude > 0.0f)
        {
            if ((beforeAxis.sqrMagnitude - axis2d.sqrMagnitude) < Mathf.Pow(BUTTON_AXIS_DUMP_MARGIN, 2))
            {
                isInput = true;

                //axis2dとbeforeAxisより角度差を算出
                //その差がマージンを超えていれば、長押しおよび2度押し
                float diff = Vector2.Angle(axis2d, beforeAxis);
                if (diff > NearAxisMargin)
                {
                    count = 0.0f;
                }
            }

            beforeAxis = axis2d;
        }

        nowPushUp = PushType.noPush;
        if (Input.GetButtonUp(name) || Input.GetButtonUp(name2))
            nowPushUp = nowPushType;

        buttonManagement(isInput);

        nowPushDown = PushType.noPush;
        if (Input.GetButtonDown(name) || Input.GetButtonDown(name2))
            nowPushDown = nowPushType;
    }

    
}
/// <summary>
/// マウスホイール用の入力管理クラス
/// </summary>
public class MouseWheelInput : AbstractInput
{

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">InputManager上の名前</param>
    public MouseWheelInput(string name) : base()
    {
        this.name = name;
    }

    /// <summary>
    /// マウスホイールの入力管理
    /// </summary>
    override public void management()
    {
        //名称が定義されていない場合はreturn
        if (name == null || name.Length == 0) return;

        //GetAxisより角度値を取得
        vAxis = Input.GetAxis(name);
        axis2d = new Vector2(0.0f, vAxis);

        //操作入力フラグ
        bool isInput = false;

        //入力動作があるかを判定()
        if (Input.GetButton(name))
        {
            if (vAxis >= 0.0f)
            {
                if (beforeAxis.y < 0.0f) count = 0.0f;
                beforeAxis.y = vAxis;
            }
            else
            {
                if (beforeAxis.y > 0.0f) count = 0.0f;
                beforeAxis.y = vAxis;
            }
            isInput = true;
        }

        buttonManagement(isInput);
    }
}

/*
/// <summary>
/// バーチャルボタン用の入力管理クラス
/// </summary>
public class VirtualButtonInput : AbstractInput
{
    /// <summary>
    /// 対称となるバーチャルボタン
    /// </summary>
    ButtonEvent button = default;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">InputManager上の名前</param>
    public VirtualButtonInput(ButtonEvent button) : base()
    {
        this.button = button;
    }

    /// <summary>
    /// キー・クリック・ボタンの入力管理
    /// </summary>
    override public void management()
    {
        //バーチャル入力を行わない、またはbuttonがなければ処理せず通過
        if (!isUseVirtualInput || button == null || button == default) return;

        nowPushUp = PushType.noPush;
        if (button.IsUp())
            nowPushUp = nowPushType;

        buttonManagement(button.IsPressed());

        nowPushDown = PushType.noPush;
        if (button.IsDown())
            nowPushDown = nowPushType;
    }
}



/// <summary>
/// バーチャルスティック用の入力管理クラス
/// </summary>
public class VirtualStickInput : AbstractInput
{
    /// <summary>
    /// 対象となるバーチャルスティック
    /// </summary>
    Joystick joystick = default;


    /// <summary>
    /// コンストラクタ
    /// joystick:対称のジョイスティックコンポーネント
    /// isUse:バーチャル入力の使用可否
    /// </summary>
    /// <param name="joystick">対称のジョイスティックコンポーネント</param>
    /// <param name="isUse">バーチャル入力の使用可否</param>
    public VirtualStickInput(Joystick joystick) : base()
    {
        this.joystick = joystick;
    }

    /// <summary>
    /// バーチャルスティックの入力管理
    /// </summary>
    override public void management()
    {
        //バーチャル入力を行わない、またはJoystickがなければ処理せず通過
        if (!isUseVirtualInput || joystick == null || joystick == default) return;

        //スティックの入力情報を取得
        hAxis = joystick.Horizontal;
        vAxis = joystick.Vertical;

        //操作入力フラグ
        bool isInput = false;

        //hv操作からaxis2dを算出するが、長さが1を超えるときは1に補正
        axis2d = new Vector2(hAxis, vAxis);
        if (axis2d.sqrMagnitude > 1.0f)
        {
            axis2d = axis2d.normalized;
            hAxis = axis2d.x;
            vAxis = axis2d.y;
        }


        float axisSqrMagnitudeDump = 0.0f;


        //入力動作があるかを判定()
        if (axis2d.sqrMagnitude > 0.0f)
        {
            //axis2dとbeforeAxisの大きさの差をとる
            axisSqrMagnitudeDump = beforeAxis.sqrMagnitude - axis2d.sqrMagnitude;

            //入力がスティックのDeadZoneを下回っていれば、未入力とする
            if (axis2d.sqrMagnitude < joystick.DeadZone)
            {
                isInput = true;

                //axis2dとbeforeAxisより角度差を算出
                //その差がマージンを超えていれば、長押しおよび2度押し
                float diff = Vector2.Angle(axis2d, beforeAxis);
                if (diff > NearAxisMargin)
                {
                    count = 0.0f;
                }
            }

            beforeAxis = axis2d;
        }

        nowPushUp = PushType.noPush;
        if (axisSqrMagnitudeDump > 0.0f)
            nowPushUp = nowPushType;

        buttonManagement(isInput);

        nowPushDown = PushType.noPush;
        if (axisSqrMagnitudeDump < 0.0f)
            nowPushDown = nowPushType;
    }


}
*/