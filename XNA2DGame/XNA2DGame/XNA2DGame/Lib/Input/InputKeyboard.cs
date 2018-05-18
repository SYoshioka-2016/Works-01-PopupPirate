using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// キーボード入力クラス
    /// シングルトン
    /// </summary>
    class InputKeyboard
    {
        // メンバー変数
        private bool[] oldState;  // 前回の入力情報

        // 自己インスタンス
        private static readonly InputKeyboard instance = new InputKeyboard();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static  InputKeyboard() { }
        private InputKeyboard() { oldState = new bool[255]; }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static InputKeyboard Instance
        {
            get { return instance; }
        }



        /// <summary>
        /// キーの状態列挙
        /// </summary>
        public enum KS 
        {
            RELEASE,    // 離している
            DOWN,       // 押した
            PRESS,      // 押している
            UP,         // 離した
        }



        /// <summary>
        /// キーの状態取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KS State( Keys key )
        {
            KeyboardState   keyState    = Keyboard.GetState();          // キーの状態
            bool            isKeyDown   = keyState.IsKeyDown( key );    // キーの押下状態判定


            // キーの状態判定
            KS state = ReturnKey( isKeyDown, key );

            // キー入力情報の登録
            SetOldState( isKeyDown, key );


            // キーの状態を返す
            return state;
        }



        /// <summary>
        /// キーの状態判定
        /// </summary>
        /// <param name="vk"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private KS ReturnKey( bool vk, Keys key )
        {
            // 前回が押されていなくて、現在が押されている場合は「押した」
            if ( !oldState[ (int)key ] &&  vk ) return KS.DOWN;

            // 前回が押されていて、現在が押されている場合は「押している」
            if (  oldState[ (int)key ] &&  vk ) return KS.PRESS;

            // 前回が押されていて、現在が押されていない場合は「離した」
            if (  oldState[ (int)key ] && !vk ) return KS.UP;

            // 前回が押されていなくて、現在が押されていない場合は「離している」
            return KS.RELEASE;
        }



        /// <summary>
        /// キー入力情報の登録
        /// </summary>
        /// <param name="vk"></param>
        /// <param name="key"></param>
        private void SetOldState( bool vk, Keys key )
        {
            oldState[ (int)key ] = vk;
        }
    }
}
