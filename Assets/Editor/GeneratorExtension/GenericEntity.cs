using CAFU.Generator;
using CAFU.Generator.Enumerates;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Generics.GeneratorExtension
{
    [PublicAPI]
    internal enum EntityType
    {
        None = 0,
        GenericEntity,
        ScriptableObjectGenericEntity,
    }

    [InitializeOnLoad]
    public class GenericEntity : IClassStructureExtension
    {
        private EntityType EntityType { get; set; }

        static GenericEntity()
        {
            var instance = new GenericEntity();
            GeneratorWindow.RegisterAdditionalOptionRenderDelegate(LayerType.Entity, instance);
            GeneratorWindow.RegisterAdditionalStructureExtensionDelegate(LayerType.Entity, instance);
        }

        public void OnGUI()
        {
            EntityType = (EntityType)EditorGUILayout.EnumPopup("Entity Type", EntityType);
        }

        public void Process(Parameter parameter)
        {
            if (EntityType > EntityType.None)
            {
                parameter.UsingList.Add("CAFU.Generics.Data.Entity");
                parameter.BaseClassName = EntityType.ToString();
            }
        }
    }
}