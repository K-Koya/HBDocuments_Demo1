using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのうちミナヒトが対象、子オブジェクトにあるコマンドをまとめ、使用する
/// </summary>
[RequireComponent(typeof(Animator))]
public class CommandHolderForMinahito : CommandHolderForMobs
{
    /// <summary>
    /// 鞭用アニメーター
    /// </summary>
    [SerializeField, Tooltip("鞭用に用いられるアニメーターをここにアタッチする")]
    protected Animator whipAnimator = default;

    // Start is called before the first frame update
    protected override void Start()
    {
        TimelineInit();
        status = GetComponent<Status>();
        characterAnimator = GetComponent<Animator>();
        aim = GetComponentInChildren<AimSystem>();

        //子オブジェクトからコマンドコンポーネントを取得し、種類によって分配する
        CommandAbstruct[] commands = GetComponentsInChildren<CommandAbstruct>();
        foreach (CommandAbstruct command in commands)
        {
            //各アニメーターのパスを提供
            command.Animator = characterAnimator;
            command.AccessoriesAnimators.Add(whipAnimator);

            switch (command.CommandType)
            {
                case CommandType.Attack:
                case CommandType.Support:
                case CommandType.Item:
                    {
                        commandList.Add(command);
                        break;
                    }
                case CommandType.Combo:
                    {
                        combo = (ComboCommand)command;
                        break;
                    }
                case CommandType.Slide:
                    {
                        dodge = command;
                        break;
                    }
                default: break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
