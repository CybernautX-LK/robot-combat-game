using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static List<Scene> loadedScenes = new List<Scene>();
    public static Scene currentScene { get; private set; }
    public static Scene previousScene { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public static void LoadSceneSingle(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static AsyncOperation LoadSceneSingleAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        return operation;
    }

    public static void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public static AsyncOperation LoadSceneAdditiveAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        return operation;
    }

    public static AsyncOperation UnloadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        return operation;
    }

    public static void UnloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.UnloadSceneAsync(currentScene);
    }

    public static Scene GetSceneByName(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        return scene;
    }



    private static void OnActiveSceneChanged(Scene scene01, Scene scene02)
    {
        currentScene = scene01;
        previousScene = scene02;
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        if (loadedScenes.Contains(scene))
            loadedScenes.Remove(scene);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        loadedScenes.Add(scene);
    }


}
