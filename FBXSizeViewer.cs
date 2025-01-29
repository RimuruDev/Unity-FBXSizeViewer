#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace AbyssMoth.Internal.Codebase.Tools.Editor
{
    public sealed class FBXSizeViewer : EditorWindow
    {
        private Vector2 scrollPosition;
        private Language currentLanguage = Language.Russian;

        private static readonly string[] labelsRU = { "Размер .fbx файлов в проекте", "Файл", "Размер", "Смена языка" };
        private static readonly string[] labelsEN = { "FBX file size in project", "File", "Size", "Switch Language" };
        private static readonly string[] sizes = { "B", "KB", "MB", "GB" };

        private string[] Labels =>
            currentLanguage == Language.Russian
                ? labelsRU
                : labelsEN;

        [MenuItem("Tools/AbyssMoth/FBX Size Viewer")]
        public static void ShowWindow() =>
            GetWindow<FBXSizeViewer>("FBX Size Viewer");

        private void OnGUI()
        {
            GUILayout.Label(Labels[0], EditorStyles.boldLabel);

            if (GUILayout.Button(Labels[3]))
                currentLanguage = currentLanguage == Language.Russian
                    ? Language.English
                    : Language.Russian;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Labels[1], EditorStyles.boldLabel, GUILayout.Width(250));
            EditorGUILayout.LabelField(Labels[2], EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            var fbxFiles = Directory.GetFiles(Application.dataPath, "*.fbx", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.Length)
                .ToArray();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));

            foreach (var fileInfo in fbxFiles)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(Path.GetFileName(fileInfo.Name), GUILayout.Width(250));
                EditorGUILayout.LabelField(FormatBytes(fileInfo.Length));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private static string FormatBytes(long bytes)
        {
            var order = 0;
            double len = bytes;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        private enum Language
        {
            Russian = 0,
            English = 1
        }
    }
}
#endif
