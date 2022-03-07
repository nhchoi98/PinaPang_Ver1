

namespace Ingame
{
    public enum Event_num
    {
        BALL_DOWN,
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
        Launch_MOTION,
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
        BATTERY
        #endregion
        
    }
    
    
    public interface IMediator
    {
        public void Event_Receive(Event_num eventNum);
    }
}
