﻿using System;
using System.Reflection;
using UnityEngine;

namespace ParticleOverdrive.Misc
{
    public static class ReflectionUtil
    {
        [Obsolete]
        public static void SetPrivateField(this object obj, string fieldName, object value)
        {
            obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
        }

        [Obsolete]
        public static T GetPrivateField<T>(this object obj, string fieldName)
        {
            return (T)((object)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj));
        }

        [Obsolete]
        public static void SetPrivateProperty(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
        }

        [Obsolete]
        public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
        {
            obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
        }

        public static void SetField(this object obj, string fieldName, object value)
        {
            ((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);
        }

        public static object GetField(this object obj, string fieldName)
        {
            return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
        }

        public static T GetField<T>(this object obj, string fieldName)
        {
            return (T)((object)obj.GetField(fieldName));
        }

        public static void SetProperty(this object obj, string propertyName, object value)
        {
            ((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);
        }

        public static object GetProperty(this object obj, string propertyName)
        {
            return ((obj is Type) ? ((Type)obj) : obj.GetType()).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj);
        }

        public static Component CopyComponent(Component original, Type originalType, Type overridingType, GameObject destination)
        {
            var copy = destination.AddComponent(overridingType);

            Type type = originalType;
            while (type != typeof(MonoBehaviour))
            {
                CopyForType(type, original, copy);
                type = type.BaseType;
            }

            return copy;
        }

        private static void CopyForType(Type type, Component source, Component destination)
        {
            FieldInfo[] myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);

            foreach (FieldInfo fi in myObjectFields)
            {
                fi.SetValue(destination, fi.GetValue(source));
            }
        }
    }
}