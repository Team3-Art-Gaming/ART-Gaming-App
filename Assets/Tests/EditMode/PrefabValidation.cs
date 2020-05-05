using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Runtime.Versioning;

namespace Tests
{
    public class PrefabValidation
    {
        private Scene _scene;
        private Object[] prefabsList;
        private List<Button> buttons;

        [OneTimeSetUp]
        public void LoadScene()
        {
            _scene = SceneManager.GetSceneAt(0);
            prefabsList = Resources.LoadAll("TestPrefabs",typeof(GameObject));
            buttons = new List<Button>();
        }

        [UnityTest]
        public IEnumerator testComponentScript()
        {
            yield return null;

            foreach (var prefab in prefabsList)
            {
                GameObject testPrefab = Object.Instantiate(prefab) as GameObject;

                if (prefab.name == "Friend Entry")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<Friend_Entry>());
                }
                else if (prefab.name == "HomeSessionMaps")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<ActiveGamePrefab>());
                }
                else if (prefab.name == "Launch_Toggle")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<Launch_Entry>());
                }
                else if (prefab.name == "LIVE_Arena")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<LiveArenaPrefab>());
                }
                else if (prefab.name == "MapEntry")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<Map_Entry>());
                }
                else if (prefab.name == "Pop-Up")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<Pop_Up_Screen>());
                }
                else if (prefab.name == "SessionNamePopUp")
                {
                    Assert.IsNotNull(testPrefab.GetComponent<LaunchSessionNamePopUp>());
                }
            }
        }
    }
}
