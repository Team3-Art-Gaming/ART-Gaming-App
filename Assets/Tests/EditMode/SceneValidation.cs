using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Tests
{
    public class SceneValidation
    {
        private Scene _scene;

        [Test]
        public void TestAll()
        {
            LoadScene();

            SceneHasTransitioner();
            SceneButtonInteract();
        }

        //Simple check to validate if the scene has the component "navControllerTester" which is auto loaded in play to
        //transition to other scenes in the game. If scene does not contain "navControllerTester", returns in error.
        [UnityTest]
        public IEnumerator SceneHasTransitioner()
        {
            yield return null;

            Debug.Log("This Scene: " + _scene.buildIndex);
            Assert.IsNotNull(GameObject.Find("SceneManager").GetComponent<navControllerTester>());
            Debug.Log("This GameObject: " + GameObject.Find("SceneManager").GetComponent<navControllerTester>() + "From Scene: " + _scene.buildIndex);
            
        }

        //Constructs a list of buttons per scene. Each button MUST have an active onClick event.
        //onClick events determine navigation, requests from db, and get from db respective to their
        //titled name. If buttons do not have an event, returns in error.
        [UnityTest]
        public IEnumerator SceneButtonInteract()
        {
            List<Button> buttons = new List<Button>();
            yield return null;

            //Debug.Log("This Scene: " + _scene.buildIndex);
            foreach(var objs in _scene.GetRootGameObjects())
            {
                int count = objs.transform.childCount;
                for(int i=0; i<count; i++)
                {
                    if(objs.transform.GetChild(i).GetComponent<Button>())
                    {
                        //Debug.Log("Child of obj: " + objs.name + " is a button: " + objs.transform.GetChild(i).name);
                        buttons.Add(objs.transform.GetChild(i).GetComponent<Button>());
                    }
                }
            }

            foreach(var button in buttons)
            {
                Assert.IsTrue(button.IsInteractable(), button.name + " is interactable");
                Assert.IsTrue(button.IsActive(), button.name + " is active");
                Assert.AreEqual(button.onClick.GetPersistentEventCount(), 1, button.name + "has exactly ONE intended job");
            }

        }

        [OneTimeSetUp]
        public void LoadScene()
        {
            _scene = SceneManager.GetSceneAt(0);
        }

       
    }
}
