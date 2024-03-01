using UnityEngine;

// Corrected the attribute name to [ExecuteAlways] and added "System.Serializable" for good practice.
[System.Serializable]
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{ 
    // References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    // Variables
    // Corrected the spelling of "SerializeField" and ensured that the TimeOfDay variable can be edited in the inspector
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update()
    {
        if (Preset == null)
            return; 

        // If the game is playing, we update the time of day and adjust the lighting accordingly.
        if (Application.isPlaying)
        {
            // Corrected "TimeOfDay.deltaTime" to "Time.deltaTime"
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; // Clamp within a 24-hour cycle
            UpdateLighting(TimeOfDay / 24f); 
        }
        else
        {
            // Allows for the lighting to be updated in the editor.
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    // This method updates the lighting according to the time of day.
    private void UpdateLighting(float timePercent)
    {
        // Set the ambient and fog lighting based on the time of day.
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If we have a directional light reference, set its color and rotation.
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            // This will rotate the light between sunrise and sunset angles.
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    // This method is called when the script is loaded or a value is changed in the Inspector.
    private void OnValidate()
    {
        // Corrected the logic here; it should be if DirectionalLight is null, then try to assign it.
        if (DirectionalLight == null)
        {
            // Try to automatically find a directional light to use if one wasn't set.
            if (RenderSettings.sun != null)
            {
                DirectionalLight = RenderSettings.sun; 
            }
            else
            {
                // Find all lights in the scene and set DirectionalLight to the first directional light found.
                Light[] lights = FindObjectsOfType<Light>();
                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        DirectionalLight = light;
                        return;
                    }
                }
            }
        }
    }
}
