
using UnityEngine;

namespace Challenge
{
    public class Alarm_Control : MonoBehaviour
    {
        [SerializeField] private QuestManager _questManager;

        public void Exit()
        {
            _questManager.Another_info();
        }
    }
}
