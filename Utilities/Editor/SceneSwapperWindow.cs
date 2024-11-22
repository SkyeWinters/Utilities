using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Editor
{
    /// <summary>
    /// Provides a custom unity editor window to facilitate quick transitioning between scenes.
    /// Included Options:
    ///     - Ability to search for scenes
    ///     - Ability to click button to jump to that scene
    ///     - A List of all scenes located in Assets/Scenes and sub folders
    ///     - Ability to refresh the scene list
    /// </summary>
    public class SceneSwapperWindow : EditorWindow
    {
        private const string SCENE_FOLDER = "Assets/Scenes/";
        private const string FILE_EXTENSION = ".unity";

        private readonly Dictionary<string, SceneGroup> _currentScenes = new();
        private readonly DirectoryInfo _sceneDirectory = new("Assets/Scenes");
        private string _searchFilter = "";
        private Vector2 _scrollPosition;
        
        private static void SwapToScene(string sceneName)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(NameWithoutExtension(sceneName));
            }
            else if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(SCENE_FOLDER + sceneName, OpenSceneMode.Single);
            }
        }

        [MenuItem("Tools/Scene Swapper", priority = 0)]
        public static void ShowWindow()
        {
            var window = (SceneSwapperWindow)GetWindow(typeof(SceneSwapperWindow), false, "Scene Swapper");
            window.RefreshList();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            
            _searchFilter = EditorGUILayout.TextField("Search", _searchFilter);
            DrawButton("Refresh Scene List", RefreshList);
            
            GUILayout.Space(10);
            DrawHorizontalLine();
            GUILayout.Space(10);
            
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            
            if (!_currentScenes.Keys.Any(FolderHaveScenes))
            {
                var label = string.IsNullOrEmpty(_searchFilter) ? $"No scenes found in {_sceneDirectory}, please refresh" 
                    : "No scenes found with the current search filter.";
                EditorGUILayout.LabelField(label);
            }
            else if (_currentScenes.Count == 1)
            {
                DisplayScenesInFolder(_currentScenes.First().Value);
            }
            else
            {
                DisplayAllFolders();
            }

            GUILayout.EndScrollView();
        }

        private void DisplayAllFolders()
        {
            foreach (var sceneFolder in _currentScenes.Keys.Where(FolderHaveScenes))
            {
                var folder = _currentScenes[sceneFolder];
                folder.IsExpanded = EditorGUILayout.Foldout(folder.IsExpanded, sceneFolder);
                if (!folder.IsExpanded) continue;

                DisplayScenesInFolder(folder);
            }
        }
        
        private void DisplayScenesInFolder(SceneGroup scenes)
        {
            foreach (var scene in scenes.Scenes.Where(SceneMatchSearch))
            {
                DrawButton(NameWithoutExtension(scene), () => SwapToScene(scene));
            }
        }
        
        private bool FolderHaveScenes(string folder)
        {
            return _currentScenes[folder].Scenes.Any(SceneMatchSearch);
        }
        
        private bool SceneMatchSearch(string sceneName)
        {
            return sceneName.Contains(_searchFilter, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private static void DrawButton(string label, Action onClick)
        {
            if (GUILayout.Button(label))
            {
                onClick();
            }
        }

        private static void DrawHorizontalLine()
        {
            var color = GUI.color;
            GUI.color = Color.black;
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUI.color = color;
        }

        private void RefreshList()
        {
            _currentScenes.Clear();
            AddScenesInFolder(_sceneDirectory, _sceneDirectory.FullName);
        }
        
        private void AddScenesInFolder(DirectoryInfo directory, string basePath)
        {
            var relativePath = directory.FullName.Replace(basePath, "").TrimStart(Path.DirectorySeparatorChar);
            relativePath = string.IsNullOrEmpty(relativePath) ? "Root" : relativePath;
            
            var filesInFolder = directory.GetFiles().Where(x => x.Name.EndsWith(FILE_EXTENSION)).Select(x => x.Name).ToList();
            if (filesInFolder.Count > 0)
            {
                _currentScenes[relativePath] = new SceneGroup { IsExpanded = true, Scenes = filesInFolder };
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                AddScenesInFolder(subDirectory, basePath);
            }
        }

        private static string NameWithoutExtension(string name)
        {
            return name.Remove(name.Length - FILE_EXTENSION.Length);
        }

        private class SceneGroup
        {
            public List<string> Scenes { get; set; }
            public bool IsExpanded { get; set; }
        }
    }
}