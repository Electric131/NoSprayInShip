using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace NoSprayInShip
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "Electric.NoSprayInShip";
        private const string modName = "NoSprayInShip";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public void Awake()
        {
            harmony.PatchAll();
            Logger.LogInfo($"{modName} loaded!");
        }

        [HarmonyPatch(typeof(SprayPaintItem))]
        internal class SprayPaintItemPatch
        {

            [HarmonyPatch("StartSpraying")]
            [HarmonyPrefix]
            static bool StartSprayingPatch(SprayPaintItem __instance)
            {
                GameObject ship = GameObject.Find("ShipHull");
                if (ship)
                {
                    float distance = Vector3.Distance(ship.transform.position, __instance.playerHeldBy.thisPlayerBody.position);
                    Debug.Log(distance);
                    if (distance < 15) return false;
                }
                return true;
            }

            [HarmonyPatch("LateUpdate")]
            [HarmonyPrefix]
            static bool LateUpdatePatch(SprayPaintItem __instance)
            {
                GameObject ship = GameObject.Find("ShipHull");
                if (__instance && __instance.isHeld && ship)
                {
                    float distance = Vector3.Distance(ship.transform.position, __instance.playerHeldBy.thisPlayerBody.position);
                    if (distance < 15)
                    {
                        __instance.StopSpraying();
                    }
                }
                return true;
            }
        }
    }
}
