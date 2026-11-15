using UnityEngine;

[ExecuteAlways]
public class ScreenTransition : MonoBehaviour
{

    [SerializeField] string VariableToModif = "_FullScreenTransition";
    [Range(0f, 1f)]
    public float progress;

    void LateUpdate()
    { 
        Apply(); 
    
    }
    void OnDidApplyAnimationProperties() => Apply();

    void OnValidate()
    { 
        Apply(); 
    
    }



    void OnDisable()
    {
        Shader.SetGlobalFloat(Shader.PropertyToID(VariableToModif), 0f);
    }

    void Apply()
    {
        Shader.SetGlobalFloat(Shader.PropertyToID(VariableToModif), progress);
    }
}