using System;
using System.Collections;
using System.Collections.Generic;
using Item;
using UnityEngine;

namespace Ingame
{
    public class Train : MonoBehaviour
    {
        public AudioSource horn;


        private void OnEnable()
        {
            horn.Play();
        }

        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            GameObject obj = other.gameObject;
            if (obj.CompareTag("Box") || obj.CompareTag("Obstacle"))
            {
                obj.GetComponent<IDestroy_Action>().Destroy_Action();
            }

            else if (obj.CompareTag("pinata"))
            {
                obj.transform.gameObject.GetComponent<Pinata.Pinata_Down>().for_revive();
            }

            else if (obj.layer == 13)
                obj.GetComponent<Determine_Destroy>().Set_Animation();

            yield return null;
        }

    }
}
