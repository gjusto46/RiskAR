using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InfoObject", menuName = "Info Object")]
public class ObjectInfoData : ScriptableObject
{
    public string title;
    [TextArea(6,5)]
    public string description;
}
