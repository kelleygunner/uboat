using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using Zenject;

namespace Game.Code.Base.InstallersUtils
{
    public class ProjectContextInstallerValidator : MonoBehaviour
    {
        [Button]
        public void SetInstallers()
        {
            
            var context = GetComponent<ProjectContext>();
            var list = context.Installers as List<MonoInstaller>;
            list.Clear();

            var installers = GetComponentsInChildren<MonoInstaller>();
            list.AddRange(installers);

            PrefabUtility.SavePrefabAsset(gameObject);
        }
    }
}
