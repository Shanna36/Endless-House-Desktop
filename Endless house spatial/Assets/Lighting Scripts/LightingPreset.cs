using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject {
     public Gradient AmbientColor;
     public Gradient DirectionalColor;
     public Gradient FogColor; 
}
