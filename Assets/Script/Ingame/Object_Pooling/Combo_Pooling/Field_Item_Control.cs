using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Ingame
{
    public class Field_Item_Control : MonoBehaviour, IRazer
    {
        [SerializeField] public Transform Object_Pool, Activating_pool;
        public Canvas canvas;
        public bool flag;
        public Image razer;
        private Sequence comboSequence;
        public bool isRaw;
        
        
        private void Awake()
        {

        }

        public void ItemActive(float which_position)
        {
            if (flag) return;
            flag = true;
            if (isRaw)
                transform.position = new Vector2(transform.position.x, which_position);
            
            else
                transform.position = new Vector2( which_position, transform.position.y);
            
            canvas.enabled = true;
            /*
            transform.SetParent(Activating_pool);
            StartCoroutine(RazerAnimation());
            */
            
        }

        public void ItemDisable()
        {
            canvas.enabled = false;
            transform.SetParent(Object_Pool);
            transform.SetAsLastSibling();
            flag = false;
        }

        IEnumerator RazerAnimation()
        {
            comboSequence.Restart();
            yield return null;
        }
    }
}
