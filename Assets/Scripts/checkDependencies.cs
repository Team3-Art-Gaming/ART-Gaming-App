using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System.Threading.Tasks;

public class checkDependencies : MonoBehaviour
{
    private async void Start()
    {
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
}
