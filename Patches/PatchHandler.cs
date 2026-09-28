
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace StupidTemplate.Patches
{
    public class PatchHandler
    {
        public static bool IsPatched { get; private set; }
        public static int PatchErrors { get; private set; }
        public static bool HasPatchErrors => PatchErrors > 0;
        public static IReadOnlyList<string> PatchFailures => patchFailures.AsReadOnly();

        public static void PatchAll()
        {
            if (!IsPatched)
            {
                PatchErrors = 0;
                patchFailures.Clear();
                instance ??= new Harmony(PluginInfo.GUID);

                Type[] types;
                try
                {
                    types = Assembly.GetExecutingAssembly().GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                    Debug.LogError("One or more plugin types could not be loaded while discovering Harmony patches.");
                    foreach (Exception loaderException in ex.LoaderExceptions ?? Array.Empty<Exception>())
                        Debug.LogError(loaderException);
                }

                foreach (Type type in types.Where(t => t != null && t.IsClass && t.GetCustomAttribute<HarmonyPatch>() != null))
                {
                    try
                    {
                        instance.CreateClassProcessor(type).Patch();
                    }
                    catch (Exception ex)
                    {
                        PatchErrors++;
                        string failure = $"{type.FullName}: {ex.Message}";
                        patchFailures.Add(failure);
                        Debug.LogError($"Failed to patch {failure}");
                    }
                }

                IsPatched = true;

                if (PatchErrors == 0)
                    Debug.Log("Patch pass complete: all discovered Harmony patches applied successfully.");
                else
                    Debug.LogWarning($"Patch pass complete with {PatchErrors} error(s). Plugin remains running with a partial patch set.");
            }
        }

        public static void UnpatchAll()
        {
            if (instance == null)
                return;

            try
            {
                if (IsPatched)
                    instance.UnpatchSelf();
            }
            finally
            {
                IsPatched = false;
                instance = null;
            }
        }

        public static void ApplyPatch(Type targetClass, string methodName, MethodInfo prefix = null, MethodInfo postfix = null, Type[] parameterTypes = null)
        {
            var original =
                parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null);

            if (original == null)
                throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            instance.Patch(original,
                prefix: prefix != null ? new HarmonyMethod(prefix) : null,
                postfix: postfix != null ? new HarmonyMethod(postfix) : null);
        }

        public static void RemovePatch(Type targetClass, string methodName, Type[] parameterTypes = null)
        {
            var original =
                parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null);
            if (original == null)
                throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            instance.Unpatch(original, HarmonyPatchType.All, instance.Id);
        }

        private static Harmony instance;
        private static readonly List<string> patchFailures = new List<string>();
        public const string InstanceId = PluginInfo.GUID;
    }
}
