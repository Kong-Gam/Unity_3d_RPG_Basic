using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Defines.Scenes.Game;
    }

    public override void Clear()
    {

    }
}
