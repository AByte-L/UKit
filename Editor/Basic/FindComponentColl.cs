
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AByte.UKit
{
    [CreateAssetMenu(menuName = "UKit/组件查找集合")]
    public class FindComponentColl : SerializedScriptableObject
    {
        [TableList]
        public List<FindItem> items;
        public FindComponentItem AddItem;

        [ShowIf("@AddItem.typeInfo!=null")]
        [ShowInInspector]
        public void Add()
        {
            if (AddItem.typeInfo == null) return;
            if (AddItem.memberInfo == null) return;
            items.Add(new FindItem()
            {
                AssemblyName = AddItem.typeInfo.Assembly.FullName,
                className = AddItem.typeInfo.FullName,
                memberType = AddItem.memberType,
                valueType = AddItem.memberInfo.Split('.')[0],
                ValueName = AddItem.memberInfo.Split('.')[1]
            });
        }

    }

    /// <summary>
    /// 成员类型
    /// </summary>
    [Serializable]
    public enum MemberType
    {

        /// <summary>
        /// 属性
        /// </summary>
        Property,
        /// <summary>
        /// 字段
        /// </summary>
        Field

    }


    [TableList]
    [Serializable, ReadOnly]
    public class FindItem
    {
        public string AssemblyName;
        public string className;
        public MemberType memberType;
        public string valueType;
        public string ValueName;
        public string CombineID => $"{className}({ValueName})";

    }


    [Serializable]
    [TableList]
    public class FindComponentItem
    {

#pragma warning disable CS0649
        [OnValueChanged("OnValueChanged")]
        [LabelText("选择类型")]
        [ValueDropdown("GetFilteredTypeList")]
        [ShowInInspector]
        public Type typeInfo;
#pragma warning restore CS0649

        [Space]
        [ShowIf("@this.typeInfo!=null")]
        [OnValueChanged("OnValueChanged")]
        [LabelText("成员类型"), LabelWidth(100)] public MemberType memberType;

        [LabelText("选择成员")]
        [ValueDropdown("GetFilteredMembersList")]
        [ShowIf("@this.typeInfo!=null")]
        [ShowInInspector]
        [NonSerialized]
        public string memberInfo;

        private void OnValueChanged()
        {
            if (memberInfo == null) return ;
            if (typeInfo == null)
            {
                memberInfo = null; return ;
            }
            var arr = GetFilteredMembersList().ToList();
            if (arr.Contains(memberInfo) == false)
            {
                memberInfo = null; return;
            }
        }

        /// <summary>
        /// typeName类型过滤
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetFilteredTypeList()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsGenericTypeDefinition)
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => t.IsPublic);
            return types;
        }

        private IEnumerable<string> GetFilteredMembersList()
        {
            if (typeInfo == null) yield return null;
            if (memberType == MemberType.Field)
            {
                var fieldInfos = typeInfo.GetFields();
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    yield return fieldInfos[i].FieldType.Name + "." + fieldInfos[i].Name;
                }
            }
            else if (memberType == MemberType.Property)
            {
                var propertyInfos = typeInfo.GetProperties();
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    yield return propertyInfos[i].PropertyType.Name + "." + propertyInfos[i].Name;
                }
            }

        }

    }

}
