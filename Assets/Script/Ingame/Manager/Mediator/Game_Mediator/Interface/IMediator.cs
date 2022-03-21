

namespace Ingame
{
    public enum Event_num
    {
        BALL_DOWN, // 
        USER_DIE,
        CHARATER_ARRIVE,
        CHARATER_MOVE,
        New_block,
        AVATAR_SET,
        BEAR_SKILL,
        #region Background_Set
        BGM_SET_START,
        #endregion

        #region BOX
        BOX_SPAWN,
        MOVE_DOWN,
        MOVE_DONE,
        BOX_REMOVE,
        BADGE_BOX_REMOVE,
        PINATA_SPAWN,
        PINATA_DIE,
        COLLECTION_GET,
        #endregion

        #region Data
        LOAD_DATA,
        INIT_DATA,
        #region SAVE
        SAVE_BALL,
        SAVE_BLOCK,
        #endregion

        #region LOAD
        LOAD_BLOCK,
        LOAD_BALL,
        #endregion
        #endregion

        #region Item
        CROSS_ITEM,
        #endregion

        #region Quest
        SET_BOX,
        SET_REVIVE,
        SET_TRIANGLE,
        SET_PINATA,
        #endregion

        #region Launch_Condition
        Launch_Green,
        Launch_Red,
        Abort_Launch,
        Launch_MOTION, // 캐릭터 발사 모션에 맞추어 공을 발사해주게 하는 이벤트 
        SET_LAUNCH_INFO,
        Abort_Launch_PINATA,
        SET_NEW_STAGE,
        #endregion

        #region Tutitoal
        Tutorial_INIT, // 처음 게임 입장시 튜토리얼에 관련된 내용으로 초기화 
        Tutorial_Skip, // 
        Tutorial_First,
        Tutorial_Basic_Done,
        TUTORIAL_CHAR,
        TUTORIAL_PINATA,
        #endregion

        #region Ad_Ingame
        BANNER,
        SPEED_AD,
        LINE_AD,
        CROSS_AD,
        MOMMY_AD,
        REVIVE_AD,
        #endregion

        #region Ad_Lobby
        QUEST_RESET,
        SHOP_DAILY,
        LINE_PURCHASE,
        BATTERY,
        ATTENDANCE
        #endregion
    }
    
    
    public interface IMediator
    {
        /// <summary>
        /// 중재자가 다른 컴포넌트로 부터 이벤트를 받게 되면, 실행되는 함수. 주로 다른 컴포넌트와 다른 컴포넌트 사이를 이어주어 명령을 전달해주는 핵심함수이다.  
        /// </summary>
        /// <param name="eventNum"></param>
        public void Event_Receive(Event_num eventNum);
    }
}
