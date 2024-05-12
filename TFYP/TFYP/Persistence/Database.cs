using System;
using System.IO;
using ProtoBuf;
using TFYP.Model;
using TFYP.Model.Common;

namespace TYFP.Persistence
{
    public static class Database
    {
        private static string GetSaveFilePath(int slot)
        {
            string baseDirectory = @"C:\Users\nikol\Desktop\tfyp\TFYP\TFYP";
            string persistenceFolder = Path.Combine(baseDirectory, "Persistence");
            string filename = $"save{slot}.bin"; // Change extension to .bin for binary files

            if (!Directory.Exists(persistenceFolder))
            {
                Directory.CreateDirectory(persistenceFolder);
            }

            string fullPath = Path.Combine(persistenceFolder, filename);
            return fullPath;
        }

        public static void Save(GameModel gameModel, int slot)
        {
            string filePath = GetSaveFilePath(slot);
            try
            {
                using (var file = File.Create(filePath))
                {
                    Serializer.Serialize(file, gameModel);
                    Console.WriteLine($"Game saved successfully in slot {slot}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save game model in slot {slot}: " + ex.Message);
            }
        }
        public static GameModel Read(int slot)
        {
            string filePath = GetSaveFilePath(slot);
            try
            {
                using (var file = File.OpenRead(filePath))
                {
                    var gameModel = Serializer.Deserialize<GameModel>(file);
                    Console.WriteLine($"Game loaded successfully from slot {slot}.");
                    return gameModel;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the game model from slot {slot}: " + ex.Message);
                return null;
            }
        }
        
    }
}
