using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

// ReSharper disable UseStringInterpolation

namespace CAFU.Generics.Data.Entity {

    public class Generator : EditorWindow {

        private const string EXTENSION = ".asset";

        private static List<string> EntityTypeNameList { get; set; }

        private static List<Type> EntityTypeList { get; set; }

        private int SelectedIndex { get; set; }

        [MenuItem("Window/CAFU/Entity Generator")]
        public static void Open() {
            GetWindow<Generator>("Entity Generator");
        }

        private void OnGUI() {
            GUILayout.Label("Generate");
            EditorGUI.indentLevel++;
            this.SelectedIndex = EditorGUILayout.Popup("Entity", this.SelectedIndex, EntityTypeNameList.ToArray());
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate")) {
                Core.Data.Entity.Generator.Generate(EntityTypeList[this.SelectedIndex]);
            }
        }

        private void OnEnable() {
            CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
            OnCompilationFinished(string.Empty, null);
        }

        private void OnDisable() {
            CompilationPipeline.assemblyCompilationFinished -= OnCompilationFinished;
        }

        private static void OnCompilationFinished(string value, CompilerMessage[] messages) {
            EntityTypeList = new List<Type>();
            EntityTypeNameList = new List<string>();
            Assembly
                .GetAssembly(typeof(GenericEntity))
                .GetTypes()
                // ScriptableObjectGenericEntity の継承型のうち、Generic 型でないモノを作成対象とみなす
                .Where(x => x.IsSubclassOf(typeof(ScriptableObjectGenericEntity)) && !x.IsGenericType)
                .ToList()
                .ForEach(
                    (x) => {
                        EntityTypeList.Add(x);
                        EntityTypeNameList.Add(x.Name);
                    }
                );
        }

    }

}