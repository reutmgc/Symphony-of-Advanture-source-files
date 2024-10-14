using UnityEngine;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif
// MAKING IT SO THE SCRIPTABLE OBJECT BEHAVES AS IT WOULD IN A BUILD, I.E. RESETS ITESELF WITH EACH GAME START
public class MyScriptableObject : ScriptableObject
{
    public string GlobalID= null; // for this gameobject, persistent throughout the project 

#if UNITY_EDITOR
    private string defaultStateInJson;
    virtual protected void OnEnable()
    {
        if (GlobalID == null)
            GlobalID = GlobalObjectId.GetGlobalObjectIdSlow(this).ToString();
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    virtual protected void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }
    void OnPlayModeChanged(PlayModeStateChange change)
    {
        // the order here matters, because on disable gets called before on enabled at the start of the game 
        if (change == PlayModeStateChange.EnteredPlayMode)
            Save();
        else if (change == PlayModeStateChange.ExitingPlayMode)
            ResetOnExitPlay();
    }
    virtual public void Save()
    {
        defaultStateInJson = JsonUtility.ToJson(this);
        if (GlobalID == "GlobalObjectId_V1-3-fa1d302f03dd61a418cbb828dfb5d1ea-11400000-0")
            Debug.LogError("Saving companion");

    }

    virtual public void ResetOnExitPlay()
    {
        JsonUtility.FromJsonOverwrite(defaultStateInJson, this);
        if (GlobalID == "GlobalObjectId_V1-3-fa1d302f03dd61a418cbb828dfb5d1ea-11400000-0")
            Debug.LogError("Ressetting"); 
    }

#endif
}


