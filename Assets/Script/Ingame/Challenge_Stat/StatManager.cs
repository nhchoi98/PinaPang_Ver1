using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge
{
    public class StatManager
    {
        public int box_remove { get; private set;}
        public int pinata_remove { get; private set;}
        public int revive_count { get; private set; }
        public int combo_count { get; private set; }
        public int item_count { get; private set; }

        public void Set_Box_Remove() => box_remove += 1;
        public void Set_pinata_remove() => pinata_remove += 1;
        public void Set_revive() => revive_count += 1;
        public void Set_Combo() => combo_count+=1;
        public void Set_item_count() => item_count += 1;

    }
}
