using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA2DGame
{
    /// <summary>
    /// シーンインターフェース
    /// </summary>
    interface IScene 
    {
        /// 初期化
        void Initialize();

        /// 更新 
        void Update();

        /// 描画
        void Draw();

        /// 終了判定
        bool IsEnd();

        /// 次のシーン
        Scene Next();

        /// コンテンツのロード
        void LoadContent();

        /// コンテンツコレクションのクリア
        void ClearContentCollection();
    }
}
