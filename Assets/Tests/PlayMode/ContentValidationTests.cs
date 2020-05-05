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
    public class ContentValidationTests
    {
        private Scene _scene;
        private List<Image> contentList;

        [OneTimeSetUp]
        public void LoadScene()
        {
            _scene = SceneManager.GetSceneAt(0);
        }

        [UnityTest]
        public IEnumerator testListContents()
        {
            contentList = new List<Image>();
            foreach (var objs in _scene.GetRootGameObjects())
            {
                int count = objs.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    if (objs.transform.GetChild(i).GetComponent<Image>())
                    {
                        Debug.Log("Child of obj: " + objs.name + " is an image: " + objs.transform.GetChild(i).name);
                        contentList.Add(objs.transform.GetChild(i).GetComponent<Image>());
                    }
                }
            }

            yield return null;

            foreach (var child in contentList)
            {
                Debug.Log(child.name);
            }
        }
    }
}
