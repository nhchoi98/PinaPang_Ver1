
using Progetile;
using UnityEngine;
using Score;

namespace Block
{
    public enum blocktype
    {
        EMPTY,
        NORMAL_RECT,
        NORMAL_TRI1,
        NORMAL_TRI2,
        NORMAL_TRI3,
        NORMAL_TRI4,
        NORMAL_CIRCLE,
        X2_RECT,
        X2_TRI1,
        X2_TRI2,
        X2_TRI3,
        X2_TRI4,
        X2_CIRCLE,
        OBSTACLE_RECT,
        OBSTACLE_TRI1,
        OBSTACLE_TRI2,
        OBSTACLE_TRI3,
        OBSTACLE_TRI4
    }
    /// <summary>
    /// 박스의 피격시 행동, 파괴시 행동을  결정지어주는 Interface
    /// </summary>
    public interface IBox
    {
        public delegate void event_delegate(object obj, DestroyArgs args); // 콤보를 띄울 위치를 담고있는 delegate
        public abstract void Attack(Vector2 pos, bool is_item = false); // 박스 피격시의 액션을 결정지어주는 함수 
        public abstract void Item_Attack(); // 아이템 피격시 공격 유무를 결정지어주는 함수 
        public abstract void Set_HP(int HP); // HP를 Set할 수 있게 만들어주는 함수 
        public abstract void Set_Type(blocktype type);
        public void Set_Candle(int candle_type);
        public void Set_ColorType(int colorType, Transform particle);
        public void Set_Event(event_delegate _event, ref Progetile_Particle script );
        public bool Get_Candle();
        public int Get_HP();
        public int whichRow();
        public void Set_Row(int value);
        public blocktype Get_Type();
        public Vector2 Get_Position();

    }

}
