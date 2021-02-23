using System;
using UnityEditor;
using UnityEngine;

namespace UI.Editor
{
    // [CustomEditor(typeof(SpriteButton), true)]
    // [CanEditMultipleObjects]
    public class SpriteButtonEditor : UnityEditor.Editor
    {
        private SpriteButton _spriteButton;
        private SerializedProperty _interactableProperty;

        private void OnEnable()
        {
            _spriteButton = (SpriteButton) target;
            _interactableProperty = serializedObject.FindProperty("interactable");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            _spriteButton.Interactable = _interactableProperty.boolValue;
            serializedObject.ApplyModifiedProperties();

        }
    }
}
