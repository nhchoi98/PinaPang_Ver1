using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Destroy_Particle_Pooling : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;
	[SerializeField]
	public Transform Object_Pool, Activating_pool;
	void OnEnable()
	{
		if (Activating_pool != null)
		{
			this.transform.SetParent(Activating_pool);
			transform.SetAsFirstSibling();
		}

		StartCoroutine("CheckIfAlive");
	}



	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if (!ps.IsAlive(true))
			{
				if (OnlyDeactivate)
				{
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
					if (Object_Pool != null)
					{
						this.gameObject.transform.SetParent(Object_Pool);
						transform.SetAsLastSibling();
					}

					this.gameObject.SetActive(false);
					
#endif
				}
				else
					GameObject.Destroy(this.gameObject);
				break;
			}
		}
	}
	
}
