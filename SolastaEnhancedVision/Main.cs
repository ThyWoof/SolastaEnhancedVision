using System;
using UnityModManagerNet;
using HarmonyLib;
using System.Reflection;

namespace SolastaEnhancedVision
{
    public class Main
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        public static void Error(Exception ex)
        {
            if (logger != null) logger.Error(ex.ToString());
        }
        public static void Error(string msg)
        {
            if (logger != null) logger.Error(msg);
        }

        public static UnityModManager.ModEntry.ModLogger logger;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Error(ex);
                throw ex;
            }
            return true;
        }

        [HarmonyPatch(typeof(MainMenuScreen), "RuntimeLoaded")]
        static class MainMenuScreen_RuntimeLoaded_Patch
        {
            static void Postfix()
            {
                try
                {
                    var human = DatabaseRepository.GetDatabase<CharacterRaceDefinition>().GetElement("Human");
                    var senseSuperiorDarkvision = DatabaseRepository.GetDatabase<FeatureDefinition>().GetElement("SenseSuperiorDarkvision");
                    var featureUnlockSenseSuperiorDarkvision = new FeatureUnlockByLevel(senseSuperiorDarkvision, 1);
                    human.FeatureUnlocks.Add(featureUnlockSenseSuperiorDarkvision);
                }
                catch (Exception ex)
                {
                    Error(ex);
                    throw ex;
                }

            }
        }
    }
}
