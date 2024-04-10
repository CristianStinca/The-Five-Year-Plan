using System;
using System.IO;
using System.Text.Json;
using TFYP.Model;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace YourGameNamespace.Persistence
{
    public static class Database
    {
        /// TO DO:
        /// var gameModel = new Gamemodel{ we will populate game model with current state }
        /// to save --> Database.Save(gameModel);
        /// to load --> var gameModel = Database.Read();
        /// 
        /// 


        private const string DefaultFilename = "data.json";

        public static void Save(GameModel gameModel, string filename = "data.json")
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(gameModel, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, jsonString);
                //Console.WriteLine($"Serialized data is saved in {filename}");
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred while saving the game model.");
                Console.WriteLine(ex.Message);
            }
        }

        public static GameModel Read(string filename = "data.json")
        {
            try
            {
                string jsonString = File.ReadAllText(filename);
                GameModel gameModel = JsonSerializer.Deserialize<GameModel>(jsonString);
                //Console.WriteLine($"Serialized data is read from {filename}");
                return gameModel;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File {filename} not found.");
                return null;
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred while reading the game model.");
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("An error occurred while deserializing the game model.");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        
    }
}
