using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA2DGame
{
    /// <summary>
    /// マウス入力クラス
    /// シングルトン
    /// </summary>
    class InputMouse
    {
        // メンバー変数
        private bool[]  oldState;   // 前回の入力情報
        private int     countTime;  // タイムカウンター

        // 自己インスタンス
        private static readonly InputMouse instance = new InputMouse();



        /// <summary>
        /// コンストラクタ
        /// </summary>
        static  InputMouse() { }
        private InputMouse() 
        {
            // マウスボタン列挙の列挙子名の配列
            string[] mbNamesArray = Enum.GetNames( typeof(MouseButton) );

            // 配列の要素数(列挙子の数)分、前回の入力情報を生成
            oldState = new bool[ mbNamesArray.Length ];
        }



        /// <summary>
        /// インスタンスゲッター
        /// </summary>
        public static InputMouse Instance 
        {
            get { return instance; }
        }



        /// <summary>
        /// マウスの状態列挙
        /// </summary>
        public enum MS
        {
            RELEASE,    // 離している
            DOWN,       // 押した
            PRESS,      // 押している
            UP,         // 離した
        }



        /// <summary>
        /// マウスボタン列挙
        /// </summary>
        public enum MouseButton 
        {
            LEFT_BUTTON,        // 左ボタン
            RIGHT_BUTTON,       // 右ボタン
            MIDDLE_BUTTON,      // 中央ボタン
            X_BUTTON_1,         // Xボタン1
            X_BUTTON_2,         // Xボタン2


            UNKNOWN,            // 不明
        }



        /// <summary>
        /// マウスボタンの状態取得
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public MS State( MouseButton mb ) 
        {
            MS      mouseState;                                     // ボタンの状態
            bool    isMouseButtonDown = IsMouseButtonDown( mb );    // マウスの状態


            // マウスボタンの状態判定
            mouseState = ReturnMouse( isMouseButtonDown, mb );

            // マウスボタン入力情報の登録
            SetOldState( isMouseButtonDown, mb );


            // マウスボタンの状態を返す
            return mouseState;
        }

        /// <summary>
        /// マウスボタンの入力判定
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        private bool IsMouseButtonDown( MouseButton mb ) 
        {
            ButtonState mouseBS;                            // ボタンの状態
            MouseState  mouseState = Mouse.GetState();      // マウスの状態


            // マウスボタンの状態の設定
            switch ( mb )
            {
                case MouseButton.LEFT_BUTTON:   mouseBS = mouseState.LeftButton;    break;
                case MouseButton.RIGHT_BUTTON:  mouseBS = mouseState.RightButton;   break;
                case MouseButton.MIDDLE_BUTTON: mouseBS = mouseState.MiddleButton;  break;
                case MouseButton.X_BUTTON_1:    mouseBS = mouseState.XButton1;      break;
                case MouseButton.X_BUTTON_2:    mouseBS = mouseState.XButton2;      break;

                default:                        return false;
            }


            // 押下状態ならtrue、遊離状態ならfalseを返す
            if ( mouseBS == ButtonState.Pressed )   return true;
            else                                    return false;
        }



        /// <summary>
        /// マウスボタンの状態判定
        /// </summary>
        /// <param name="vk"></param>
        /// <param name="mb"></param>
        /// <returns></returns>
        private MS ReturnMouse( bool vk, MouseButton mb )
        {
            // 前回が押されていなくて、現在が押されている場合は「押した」
            if ( !oldState[ (int)mb ] &&  vk ) return MS.DOWN;

            // 前回が押されていて、現在が押されている場合は「押している」
            if (  oldState[ (int)mb ] &&  vk ) return MS.PRESS;

            // 前回が押されていて、現在が押されていない場合は「離した」
            if (  oldState[ (int)mb ] && !vk ) return MS.UP;

            // 前回が押されていなくて、現在が押されていない場合は「離している」
            return MS.RELEASE;
        }



        /// <summary>
        /// マウスボタン入力情報の登録
        /// </summary>
        /// <param name="vk">(現在の)ボタンの押下状態</param>
        /// <param name="button">ボタン</param>
        private void SetOldState( bool vk, MouseButton mb )
        {
            oldState[ (int)mb ] = vk;
        }



        /// <summary>
        /// マウスカーソル座標ゲッター
        /// </summary>
        /// <returns></returns>
        public Vector2 MouseCursorPosition() 
        {
            Vector2     mcPos       = new Vector2( 0, 0 );      // マウスカーソルの座標
            MouseState  mouseState  = Mouse.GetState();         // マウスの状態
            int         mcPosXMax   = Screen.Width;             // マウスカーソルのX座標の最大
            int         mcPosXMin   = 0;                        // マウスカーソルのX座標の最小
            int         mcPosYMax   = Screen.Height;            // マウスカーソルのY座標の最大
            int         mcPosYMin   = 0;                        // マウスカーソルのY座標の最小


            // マウスカーソルの座標を取得
            mcPos.X = mouseState.X;
            mcPos.Y = mouseState.Y;


            // マウスカーソルの座標を調整
            if ( mcPos.X < mcPosXMin ) mcPos.X = mcPosXMin;
            if ( mcPos.X > mcPosXMax ) mcPos.X = mcPosXMax;
            if ( mcPos.Y < mcPosYMin ) mcPos.Y = mcPosYMin;
            if ( mcPos.Y > mcPosYMax ) mcPos.Y = mcPosYMax;


            return mcPos;
        }



        /// <summary>
        /// マウスカーソルが長方形の中に有るか判定
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public bool IsMouseCursorOnRect( Vector2 point1, Vector2 point2 ) 
        {
            Vector2     leftUpPos;          // 長方形の左上の座標
            Vector2     rightDownPos;       // 長方形の右下の座標

            Vector2     mcPos       = new Vector2( 0, 0 );      // マウスカーソルの座標
            MouseState  mouseState  = Mouse.GetState();         // マウスの状態


            // マウスカーソルの座標を取得
            mcPos.X = mouseState.X;
            mcPos.Y = mouseState.Y;


            // 長方形の左上と右下の座標を設定
            leftUpPos.X     = ( point1.X < point2.X )? point1.X : point2.X;
            leftUpPos.Y     = ( point1.Y < point2.Y )? point1.Y : point2.Y;
            rightDownPos.X  = ( point2.X > point1.X )? point2.X : point1.X;
            rightDownPos.Y  = ( point2.Y > point1.Y )? point2.Y : point1.Y;


            // マウスカーソルの座標が長方形の中に有るか判定
            if ( leftUpPos.X <= mcPos.X && mcPos.X <= rightDownPos.X &&
                 leftUpPos.Y <= mcPos.Y && mcPos.Y <= rightDownPos.Y )
            {
                // 有る
                return true;
            }
            else
            {
                // 無い
                return false;
            }
        }



        /// <summary>
        /// マウスカーソルが長方形の中に有るか判定
        /// 指定された時間(秒)まで有るか判定
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public bool IsMouseCursorOnRect( Vector2 point1, Vector2 point2, float seconds ) 
        {
            // 秒数が0未満なら判定は取らない(長方形の中に無いとする)
            if ( seconds < 0 ) return false;



            // マウスカーソルが長方形の中に有ればカウント
            if ( IsMouseCursorOnRect( point1, point2 ) )
            {
                ++countTime;
            }
            // 無ければタイムカウントを初期化
            else
            {
                countTime = 0;
            }



            // タイムカウントが指定した時間を超えれば有る
            if ( countTime >= seconds * 60 )
            {
                // タイムカウントを初期化
                countTime = 0;

                // 有る
                return true;
            }
            else
            {
                // 無い
                return false;
            }
        }
    }
}