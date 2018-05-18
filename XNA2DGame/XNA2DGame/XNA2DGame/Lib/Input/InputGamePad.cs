using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// ゲームパッド入力クラス
    /// シングルトン
    /// </summary>
    class InputGamePad
    {
        // メンバー変数
        private Dictionary< int, bool > oldState;  // 前回の入力情報

        // 自己インスタンス
        private static readonly InputGamePad instance = new InputGamePad();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static  InputGamePad() { }
        private InputGamePad() { oldState = new Dictionary< int, bool >(); }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static InputGamePad Instance 
        {
            get { return instance; }
        }



        /// <summary>
        /// ボタンの状態列挙
        /// </summary>
        public enum BS
        {
            RELEASE,    // 離している
            DOWN,       // 押した
            PRESS,      // 押している
            UP,         // 離した
        }



        /// <summary>
        /// ボタンの状態取得
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <param name="button">ボタン</param>
        /// <returns>ボタンの状態</returns>
        public BS State( Buttons button, PlayerIndex playerIdx = PlayerIndex.One )
        {
            GamePadState    gamePadState = GamePad.GetState( playerIdx );           // ゲームパッドの状態
            bool            isButtonDown = gamePadState.IsButtonDown( button );     // ボタンの押下状態判定


            // 前回の入力情報にそのボタンのキーが無ければ追加
            if ( !oldState.ContainsKey( (int)button ) ) 
            { 
                oldState.Add( (int)button, isButtonDown );
            }

            // ボタンの状態判定
            BS buttonState = ReturnButton( isButtonDown, button );

            // ボタン入力情報の登録
            SetOldState( isButtonDown, button );


            // ボタンの状態を返す
            return buttonState;
        }



        /// <summary>
        /// ボタンの状態判定
        /// </summary>
        /// <param name="vk">(現在の)ボタンの押下状態</param>
        /// <param name="button">ボタン</param>
        /// <returns>ボタンの状態</returns>
        private BS ReturnButton( bool vk, Buttons button )
        {
            // 前回が押されていなくて、現在が押されている場合は「押した」
            if ( !oldState[ (int)button ] &&  vk ) return BS.DOWN;

            // 前回が押されていて、現在が押されている場合は「押している」
            if (  oldState[ (int)button ] &&  vk ) return BS.PRESS;

            // 前回が押されていて、現在が押されていない場合は「離した」
            if (  oldState[ (int)button ] && !vk ) return BS.UP;

            // 前回が押されていなくて、現在が押されていない場合は「離している」
            return BS.RELEASE;
        }



        /// <summary>
        /// ボタン入力情報の登録
        /// </summary>
        /// <param name="vk">(現在の)ボタンの押下状態</param>
        /// <param name="button">ボタン</param>
        private void SetOldState( bool vk, Buttons button )
        {
            oldState[ (int)button ] = vk;
        }



        /// <summary>
        /// ゲームパッドの接続状況
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <returns>ゲームパッドの接続状況</returns>
        public bool IsConnected( PlayerIndex playerIdx = PlayerIndex.One )
        {
            return GamePad.GetState( playerIdx ).IsConnected;
        }



        /// <summary>
        /// 振動モーターの速度設定
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <param name="left">左の振動モーターの速度</param>
        /// <param name="right">右の振動モーターの速度</param>
        public void Vibration( PlayerIndex playerIdx = PlayerIndex.One, float left = 1.0f, float right = 1.0f )
        {
            GamePad.SetVibration( playerIdx, left, right );
        }



        /// <summary>
        /// 左スティックのベクトル
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <returns>左スティックのベクトル</returns>
        public Vector2 LeftThumbStickVector( PlayerIndex playerIdx = PlayerIndex.One ) 
        {
            return GamePad.GetState( playerIdx ).ThumbSticks.Left;
        }



        /// <summary>
        /// 右スティックのベクトル
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <returns>右スティックのベクトル</returns>
        public Vector2 RightThumbStickVector( PlayerIndex playerIdx = PlayerIndex.One ) 
        {
            return GamePad.GetState( playerIdx ).ThumbSticks.Right;
        }



        /// <summary>
        /// 左のトリガーの位置
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <returns>左のトリガーの位置</returns>
        public float LeftTrigger( PlayerIndex playerIdx = PlayerIndex.One ) 
        {
            return GamePad.GetState( playerIdx ).Triggers.Left;
        }



        /// <summary>
        /// 右のトリガーの位置
        /// </summary>
        /// <param name="playerIdx">プレイヤーインデックス</param>
        /// <returns>右のトリガーの位置</returns>
        public float RightTrigger( PlayerIndex playerIdx = PlayerIndex.One ) 
        {
            return GamePad.GetState( playerIdx ).Triggers.Right;
        }



        /// <summary>
        /// 解放
        /// </summary>
        public void Unload() 
        {
            oldState.Clear();
        }
    }
}
