
using UnityEngine;
using UnityEngine.UI;

namespace Box
{
    public class Boxselect : MonoBehaviour, IBoxselect
    {
        public int type;
        public Animator gridAni;
        public void Activate_Grid()
        {
            Set_Which_Type(true);
            gridAni.enabled = true;
            gridAni.SetTrigger("selecting");
            
        }

        public void Deactivate_Grid()
        {
            Set_Which_Type(false);
            gridAni.SetTrigger("done");
            switch (type)
            {
                default:
                    transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
                    break;
                
                case 1:
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
                
                case 2:
                    transform.GetChild(6).GetChild(0).gameObject.SetActive(false);
                    break;
                
            }
            
            gridAni.enabled = false;
        }

        private void Set_Which_Type(bool is_active)
        {
            if (is_active)
            {
                switch (type)
                {
                    default: // 일반 박스들  
                        for (int i = 0; i < 5; i++)
                            transform.GetChild(i).gameObject.GetComponent<Canvas>().sortingLayerName = "Panel";
                        break;

                    case 1: // 장애물 박스 
                        gameObject.GetComponent<Canvas>().sortingLayerName = "Panel";
                        break;
                        
                    case 2: // 2배 박스 
                        for (int i = 0; i < 5; i++)
                            transform.GetChild(i).gameObject.GetComponent<Canvas>().sortingLayerName = "Panel";

                        transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Panel";
                        break;

                }
            }

            else
            {
                switch (type)
                {
                    default: // 일반 박스들  
                        for (int i = 0; i < 5; i++)
                            transform.GetChild(i).gameObject.GetComponent<Canvas>().sortingLayerName = "Default";
                        break;

                    case 1:
                        gameObject.GetComponent<Canvas>().sortingLayerName = "Default";
                        break;
                        
                    case 2:
                        for (int i = 0; i < 5; i++)
                            transform.GetChild(i).gameObject.GetComponent<Canvas>().sortingLayerName = "Default";
                        
                        transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        break;

                }
                
            }

        }
    }
}
