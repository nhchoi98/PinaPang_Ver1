using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이버 텍스트와 이름을 리턴시켜주는 함수 
/// </summary>
public class Flavor_Text: MonoBehaviour
{
    public Text description;
    public Text title;
    public Text quantity;
    public Image image;

    public GameObject descriptionPanel;
    public void OnClick_Collection(int index)
    {
        flavor(index);
        name(index);
        Set_Img(index);
        descriptionPanel.SetActive(true);
    }

    public void OnClick_Close_Btn()
    {
        descriptionPanel.SetActive(false);
    }
    
    private void Set_Img(int index)
    {
        Sprite img = Resources.Load<Sprite>("Lobby/Collection/Theme0/Collection_"+index.ToString());
        image.sprite = img;
    }
    

    /// <summary>
    /// flavor 내용을 리턴시켜주는 함수 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private void flavor(int index)
    {
        string value;
        switch (index)
        {
            default:
                value = null;
                break;
            
            case 12:
                value = "A sad pig with no full memories.";
                break;
            
            case 13:
                value = "This small thing will be a dream car.";
                break;
            
            case 14:
                value = "The engine sounds cute, but it looks mature.";
                break;
            
            case 3:
                value = "Build and Destroy, Build and Destroy, .... It looks like my art design!";
                break;
            
            case 4:
                value = "There are a lot of colorful beads. Math is not important to children.";
                break;
            
            case 5:
                value = "The first musical instrument of one's life";
                break;
            
            case 15:
                value = "chuga-chuga";
                break;
            
            case 16:
                value = "vroom-";
                break;
            
            case 17:
                value = "It'll dig anything out.";
                break;
            
            case 9:
                value = "Violent but most popular dinosaur.";
                break;
            
            case 10:
                value = "Dinosaurs protecting themselves with horns on their heads.";
                break;
            
            case 11:
                value = "Dinosaurs that can eat the leaves of tall trees with their long necks.";
                break;
            
            case 6:
                value = "Don't ride standing up. UH...it's impossible.";
                break;
            
            case 7:
                value = "If you rock too much, you might fall, so be careful.";
                break;
            
            case 8:
                value = "Every children has a dream of climbing on an elephant and sliding on its nose at least once.";
                break;
            
            case 23:
                value = "Who am I? Dream Car Owner.";
                break;
            
            case 22:
                value = ".. .----. -- / .--. ..- .--. .--. -.-- -.-.--";
                break;
            
            case 0:
                value = "Many people think bears are cute because of the doll's cuteness.";
                break;
            
            case 1:
                value = "The only thing we have to fear is fear it self.";
                break;
            
            case 2:
                value = "I AM PUPPY!";
                break;
            
            case 24:
                value = "Who am I? Dream House Owner.";
                break;
            
            case 19:
                value = "True love is putting someone else before yourself.";
                break;
            
            case 18:
                value = "All grown-ups were once children first. But few of them remember it.";
                break;
            
            case 20:
                value = "All grown-ups were once children first.";
                break;
            
            case 21:
                value = "If you've tested your skills, you can use this magic wand.";
                break;
        }

        description.text = value;
    }
    
    /// <summary>
    /// name을 리턴시켜주는 함수 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private void name(int index)
    {
        string value;
        switch (index)
        {
            default:
                value = null;
                break;
            
            case 12:
                value = "Piggy Bank";
                break;
            
            case 13:
                value = "Red Mini Car";
                break;
            
            case 14:
                value = "White Mini Car";
                break;
            
            case 3:
                value = "Stacker";
                break;
            
            case 4:
                value = "Abacus";
                break;
            
            case 5:
                value = "Rattle";
                break;
            
            case 15:
                value = "Toy Train";
                break;
            
            case 16:
                value = "Toy Truck";
                break;
            
            case 17:
                value = "Toy Fork Crane";
                break;
            
            case 9:
                value = "Tyrannosaurus Toy";
                break;
            
            case 10:
                value = "Triceratops Toy";
                break;
            
            case 11:
                value = "Brachiosaurus Toy";
                break;
            
            case 6:
                value = "Baby Swing";
                break;
            
            case 7:
                value = "Rocking Horse";
                break;
            
            case 8:
                value = "Slide";
                break;
            
            case 23:
                value = "Toy Car";
                break;
            
            case 22:
                value = "Robot Puppy";
                break;
            
            case 0:
                value = "Bear Doll";
                break;
            
            case 1:
                value = "Rabbit Doll";
                break;
            
            case 2:
                value = "Puppy Doll";
                break;
            
            case 24:
                value = "Doll House";
                break;
            
            case 19:
                value = "Princess Doll";
                break;
            
            case 18:
                value = "Prince Doll";
                break;
            
            case 20:
                value = "Playing House Kit";
                break;
            
            case 21:
                value = "Magic wand";
                break;
            
        }

        title.text = value;
    }
}
