
using Badge;
using UnityEngine;

public class Badge_Lower_Control : MonoBehaviour
{
    [SerializeField] private Badge_UI _badgeUI;
    public AudioSource click;
    public void OnClick_Exit()
    {
        click.Play();
        this.gameObject.SetActive(false);
        
    }
}
