using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[System.Serializable]
public class SoundDataRow:SoDataRow
{
    public AudioClip audioClip;
   

    [MinMaxSlider(0,2)]
    public Vector2 pitch = new Vector2(1, 1);
    [MinMaxSlider(0,1)]
    public Vector2 volume = new Vector2(1, 1);

    
    

}

[CreateAssetMenu(menuName = "SoDataTable/AudioTable")]
public class SoundTable : SoDataTable<SoundDataRow>
{
   
}
