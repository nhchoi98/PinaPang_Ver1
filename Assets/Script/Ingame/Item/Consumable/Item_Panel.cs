
using UnityEngine;
using UnityEngine.UI;
namespace Item
{
    public class Item_Panel : MonoBehaviour
    {

        [Header("Panel")]
        public GameObject mergepanel, plusPanel;

        [Header("Button")] public GameObject mergeBtn, lineBtn;
        public void OnEnable()
        {
            mergeBtn.GetComponent<Button>().interactable = false;
            lineBtn.GetComponent<Button>().interactable = true;
            mergepanel.SetActive(true);
            plusPanel.SetActive(false);
            Time.timeScale= 0f;
        }

        public void Exit()
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
            
        }

        public void OnClick_Merge()
        {
            mergeBtn.GetComponent<Button>().interactable = false;
            lineBtn.GetComponent<Button>().interactable = true;
            mergepanel.SetActive(true);
            plusPanel.SetActive(false);
        }
        
        public void OnClick_Line()
        {
            mergeBtn.GetComponent<Button>().interactable = true;
            lineBtn.GetComponent<Button>().interactable = false;
            mergepanel.SetActive(false);
            plusPanel.SetActive(true);
        }
    }
}
