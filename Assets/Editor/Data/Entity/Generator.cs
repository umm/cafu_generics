using System;
using System.Collections.Generic;
using System.IO;
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

        public static void Generate<T>() where T : ScriptableObject {
            Generate(typeof(T));
        }

        public static void Generate(Type type) {
            string path = ResolvePath(type);
            AssetDatabase.CreateAsset(CreateInstance(type), path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
        }

        private static string ResolvePath(Type type) {
            string directoryName = "Assets";
            if (Selection.objects.Length != 0) {
                string path = AssetDatabase.GetAssetPath(Selection.objects[0]);
                if (AssetDatabase.IsValidFolder(path)) {
                    // ディレクトリの場合は、それをそのまま採用
                    directoryName = path;
                } else if (!string.IsNullOrEmpty(path)) {
                    // 空文字ではない場合は、ディレクトリ以外を選択していると見なす
                    // Hierarchy 上の GameObject などを選択している場合、空文字が返される
                    directoryName = Path.GetDirectoryName(path);
                }
            }
            // ReSharper disable once AssignNullToNotNullAttribute
            string basePath = Path.Combine(directoryName, type.Name);
            // ファイルが存在しないなら確定
            if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(string.Format("{0}{1}", basePath, EXTENSION)) == null) {
                return string.Format("{0}{1}", basePath, EXTENSION);
            }
            // ファイルが存在する場合はサフィックスとして数字を付ける
            int index = 0;
            while (AssetDatabase.LoadAssetAtPath<ScriptableObject>(string.Format("{0} {1}{2}", basePath, ++index, EXTENSION)) != null) {
            }
            return string.Format("{0} {1}{2}", basePath, index, EXTENSION);
        }
        private void OnGUI() {
            GUILayout.Label("Generate");
            EditorGUI.indentLevel++;
            SelectedIndex = EditorGUILayout.Popup("Entity", SelectedIndex, EntityTypeNameList.ToArray());
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            if (GUILayout.Button("Generate")) {
                Generate(EntityTypeList[SelectedIndex]);
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
            AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
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