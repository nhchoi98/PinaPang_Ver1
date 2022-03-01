using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Fire
{
    public class Flame_Down : MonoBehaviour
    {
        public int type;
        public Image _Image;

        private void OnEnable()
        {
            _Image.color = new Color(1f, 1f, 1f, 1f);
        }

        public void Ignition()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Ignition");
            if (type == 1)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Ignition");
            }

        }

        public void Extinquish()
        {
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("out");
            if (type == 1) transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("out");
            
            StartCoroutine(fade_out());
        }

        IEnumerator fade_out()
        {
            Color alpha_control;
            while (true)
            {
                _Image.color = new Color(1f, 1f, 1f, _Image.color.a - Time.unscaledDeltaTime);
                if (_Image.color.a < 0.05f)
                    break;
                
                yield return null;
            }
            transform.GetChild(0).gameObject.SetActive(false);
            if (type == 1) transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

    }
}
