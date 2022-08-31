using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using Sirenix.OdinInspector;

/// <summary>
/// Game Manager
/// </summary>
public class Akasha : PersistentSingleton<Akasha>
{
    #region Const Vars
    private const string KEYBINDS = "Keybinds";
    #endregion

    [SerializeField, FoldoutGroup("Input")] private EventSystem _eventSystem;
    [SerializeField, FoldoutGroup("Input")] private InputSystemUIInputModule _inputModule;
    [SerializeField, FoldoutGroup("Input")] private InputActionAsset _inputActionAsset;

    private PlayerInput _playerInput;

    protected void OnEnable()
    {
        EventManager.AddListener<PlayerSpawnEvent>(OnPlayerSpawned);
        EventManager.AddListener<PlayerDespawnEvent>(OnPlayerDespawned);
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<PlayerSpawnEvent>(OnPlayerSpawned);
        EventManager.RemoveListener<PlayerDespawnEvent>(OnPlayerDespawned);
    }

    public void Initialize()
    {
        LoadKeybinds();
    }

    public void SaveKeybinds()
    {
        string keybinds = _inputActionAsset.ToJson();
        PlayerPrefs.SetString(KEYBINDS, keybinds);
    }

    private void LoadKeybinds()
    {
        string keybinds = PlayerPrefs.GetString(KEYBINDS, string.Empty);
        if (!string.IsNullOrEmpty(keybinds))
        {
            _inputActionAsset.LoadFromJson(keybinds);
        }
    }

    private void OnPlayerSpawned(PlayerSpawnEvent e, object t) => _playerInput = e.PlayerInput;
    private void OnPlayerDespawned(PlayerDespawnEvent e, object t) => _playerInput = null;
}
