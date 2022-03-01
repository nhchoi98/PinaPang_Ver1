
using UnityEngine;

public class Collection_Ani_Control : MonoBehaviour
{
    public Animator ani;

    public void hide()
    {
        ani.SetBool("Arrive",false);
    }
    // Start is called before the first frame update
}
