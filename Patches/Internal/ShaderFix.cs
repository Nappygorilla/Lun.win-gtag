using HarmonyLib;
using UnityEngine;

namespace StupidTemplate.Patches.Internal
{
    [HarmonyPatch(typeof(GameObject), "CreatePrimitive")]
    public class ShaderFix : MonoBehaviour
    {
        private static void Postfix(GameObject __result)
        {
            if (__result == null)
                return;

            Renderer renderer = __result.GetComponent<Renderer>();
            if (renderer == null || renderer.material == null)
                return;

            Shader shader = Shader.Find("GorillaTag/UberShader");
            if (shader != null)
                renderer.material.shader = shader;

            renderer.material.color = Color.black;
        }
    }
}