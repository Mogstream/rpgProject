using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.Saving
{
    public class SavingSystemMe : MonoBehaviour
    {       

        string path;

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("LastSceneBuildIndex"))
            {
                int buildIndex = (int)state["LastSceneBuildIndex"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }            
            RestoreState(state);
        }
        public void Save(string saveFile)
        { 
            Dictionary<string, object> state = LoadFile(saveFile);         
            CaptureState(state);
            SaveFile(saveFile, state);            
        }        

        public void Load(string saveFile)
        { 
            RestoreState(LoadFile(saveFile));
        }        

        private void SaveFile(string saveFile, object state)
        {
            path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream =  File.Open(path, FileMode.Create))
            {                
                BinaryFormatter formatter = new BinaryFormatter();                
                formatter.Serialize(stream, state);
            } 
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }    
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {                
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            } 
        }

        private void CaptureState(Dictionary<string, object> state)
        {            
            foreach ( SaveableEntityMe saveable in FindObjectsOfType<SaveableEntityMe>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();                
            }  

            state["LastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex; 
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntityMe saveable in FindObjectsOfType<SaveableEntityMe>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                saveable.RestoreState(state[id]);
                }
            }
        }    

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");            
        }       
    } 
}