﻿using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "selectable";

    private ISelectionResponse _selectionResponse;

    private Transform _selection;
    
    private void Awake()
    {
        _selectionResponse = GetComponent<ISelectionResponse>();
    }

    private void Update()
    {
        if (_selection != null) _selectionResponse.OnDeselect(_selection);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        _selection = null;
        if (Physics.Raycast(ray, out var hit, 2f))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                _selection = selection;
            }
        }

        if (_selection != null)
        {
            _selectionResponse.OnSelect(_selection);
        }
    }
}