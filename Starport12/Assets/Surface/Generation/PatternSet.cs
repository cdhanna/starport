using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPatterns", menuName = "Map/Map Patterns")]
public class PatternSet : ScriptableObject {

    public List<MapPattern> Patterns = new List<MapPattern>();


}
