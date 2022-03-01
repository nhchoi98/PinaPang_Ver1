
namespace Alarm
{
    public enum Event_Alarm
    {
        BADGE_ALARM_TRUE,
        BADGE_ALARM_FALSE,
        SKIN_ALARM_ON,
        DETERMINE_SKIN_ALARM,
        SHOP_ALARM,
        QUEST_ALARM_FALSE,
        QUEST_ALARM_TRUE,
        QUEST_REWARD_TRUE,
        QUEST_REWARD_FALSE,
        EXCHANGE_ALARM,
        AVATAR_ALARM_ON,
        AVATAR_ALARM_OFF,
        BALL_ALARM_ON,
        BALL_ALARM_OFF,
        LINE_ALARM_ON,
        LINE_ALARM_OFF,
        ASTRO_ALARM,
        SCIENCE_ALARM,
        BEAR_ALARM,
        PARTY_ALARM,
        SHOP_ALARM_ON,
        SHOP_ALARM_OFF,
        TOY_ALARM_ON,
        TOY_ALARM_OFF
    }
    
    
    /// <summary>
    /// 알람 시스템과 관련된 Mediator
    /// </summary>
    public interface IAlarmMediator
    {
        public void Event_Receieve(Event_Alarm _event, int index = -1);
    }
}
