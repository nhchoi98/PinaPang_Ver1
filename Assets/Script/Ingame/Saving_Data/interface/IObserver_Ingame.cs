
namespace Ingame_Data
{
    public interface IObserver_Ingame
    {
        public abstract void Update_Status(); // 상태가 업데이트 되었음을 알리는 함수
        public abstract void LoadData_ToIngame(); // 이어하기를 해서 들어온 경우, 이 함수를 통해 뭔가를 만들어줌. 
    }
}
