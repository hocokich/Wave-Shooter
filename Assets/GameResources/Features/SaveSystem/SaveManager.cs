namespace WaveShooter.Features.SaveSystem
{
    using UnityEngine;

    /// <summary>
    /// Статичный класс менеджера сохранений.
    /// </summary>
    public static class SaveManager
    {
        private const string KEY_HAS_SAVE = "Save_HasData";
        
        public static bool HasSave() => PlayerPrefs.GetInt(KEY_HAS_SAVE, 0) == 1;

        public static void SaveGame(float time, int health, int ammoMag, int ammoTotal, int enemiesLeft, int killed, int active)
        {
            PlayerPrefs.SetInt(KEY_HAS_SAVE, 1);
            
            PlayerPrefs.SetFloat("Save_Time", time);
            PlayerPrefs.SetInt("Save_Health", health);
            PlayerPrefs.SetInt("Save_AmmoMag", ammoMag);
            PlayerPrefs.SetInt("Save_AmmoTotal", ammoTotal);
            PlayerPrefs.SetInt("Save_EnemiesLeft", enemiesLeft);
            PlayerPrefs.SetInt("Save_Killed", killed);
            PlayerPrefs.SetInt("Save_Active", active);
            
            PlayerPrefs.Save();
            Debug.Log("Игра сохранена.");
        }

        public static void DeleteSave()
        {
            PlayerPrefs.DeleteKey(KEY_HAS_SAVE);
            PlayerPrefs.Save();
        }

        public static float GetTime() => PlayerPrefs.GetFloat("Save_Time", 60f);
        public static int GetHealth() => PlayerPrefs.GetInt("Save_Health", 100);
        public static int GetAmmoMag() => PlayerPrefs.GetInt("Save_AmmoMag", 20);
        public static int GetAmmoTotal() => PlayerPrefs.GetInt("Save_AmmoTotal", 60);
        public static int GetEnemiesLeft() => PlayerPrefs.GetInt("Save_EnemiesLeft", 1000);
        public static int GetKilled() => PlayerPrefs.GetInt("Save_Killed", 0);
        public static int GetActive() => PlayerPrefs.GetInt("Save_Active", 0);
    }
}