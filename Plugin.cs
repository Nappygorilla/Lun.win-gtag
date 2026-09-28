using BepInEx;
using UnityEngine;

namespace StupidTemplate
{
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private bool spawnHooked;

        private void Awake()
        {
            try
            {
                Configuration.Initialize(Config);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[{PluginInfo.Name}] Configuration initialization failed: {ex}");
                return;
            }

            try
            {
                if (!spawnHooked)
                {
                    GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
                    spawnHooked = true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[{PluginInfo.Name}] Failed to register player-spawn callback: {ex}");
            }
        }

        public void OnPlayerSpawned()
        {
            try
            {
                Patches.PatchHandler.PatchAll();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[{PluginInfo.Name}] Failed to apply patches: {ex}");
            }
        }

        private void Start()
        {
            Debug.Log($"Loaded: {PluginInfo.Name} v{PluginInfo.Version}");
        }

        private void OnDestroy()
        {
            try
            {
                Patches.PatchHandler.UnpatchAll();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[{PluginInfo.Name}] Failed to clean up patches: {ex}");
            }
        }
    }
}
