

namespace Alarm
{
    public interface IAlarmComponent
    {
        public void Set_Mediator(IAlarmMediator mediator);
        public void Event_Receieve(Event_Alarm _event, int index = -1);
        public void First_Set();
        public bool Get_Alarm();
    }
}
