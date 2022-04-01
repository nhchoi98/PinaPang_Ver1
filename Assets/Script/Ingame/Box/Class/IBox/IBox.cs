
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
        OBSTACLE_TRI4,
        PLUSBALL
    }
    /// <summary>
    /// 박스의 피격시 행동, 파괴시 행동을  결정지어주는 Interface
    /// </summary>
    public interface IBox
    {
        public delegate void event_delegate(object obj, DestroyArgs args); // 콤보를 띄울 위치를 담고있는 delegate
        public void Attack(Vector2 pos, bool is_item = false); // 박스 피격시의 액션을 결정지어주는 함수 
        public void Item_Attack(); // 아이템 피격시 공격 유무를 결정지어주는 함수 
        public void Set_HP(int HP); // HP를 Set할 수 있게 만들어주는 함수 
        public void Set_Type(blocktype type); // 삼각형? 사각형? 원? 인지 구분지어주는 함수. ( 퀘스트 용 )
        public void Set_Candle(int candle_type); // 캔들박스인지? 아닌지? candle type = 어떤 문자가 나와야되는지?
        public void Set_ColorType(int colorType, Transform particle); // 박스 파괴시 파티클의 색상을 지정해주는 함수  + 어떤 파티클 쓸 지를 지정해줌. .
        public void Set_Event(event_delegate _event, ref Progetile_Particle script );
        public int Get_Candle();
        public int Get_HP();
        public int whichRow(); // 열 정보 반환해주는 함수 
        public void Set_Row(int value);
        public int Get_Row();
        public blocktype Get_Type(); 
        public Vector2 Get_Position();

    }

}
