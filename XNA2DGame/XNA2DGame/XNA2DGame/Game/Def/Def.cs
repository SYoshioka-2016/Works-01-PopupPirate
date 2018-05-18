using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA2DGame
{
    /// <summary>
    /// スクリーンクラス
    /// </summary>
    static class Screen
    {
        public const int Width  = 1024;  // 幅
        public const int Height =  768;  // 高さ
        //public const int Width  = 800;  // 幅
        //public const int Height = 600;  // 高さ
    }



    /// <summary>
    /// シーン列挙
    /// </summary>
    enum Scene 
    {
        Title,      // タイトル
        GamePlay,   // ゲームプレイ
        Ending,     // エンディング

        Unknown,
    }



    /// <summary>
    /// 行動列挙
    /// </summary>
    enum ActionMode 
    {
        STAND,      // 立ち
        JUMP,       // ジャンプ
        DAMAGE,     // ダメージ
    }



    /// <summary>
    /// 方向(向き)列挙
    /// </summary>
    enum Direction 
    {
        LEFT    = -1,   // 左
        RIGHT   = 1,    // 右
        DOWN,           // 上
        UP,             // 下

        UNKNOWN_DIRECTION,
    }
}
