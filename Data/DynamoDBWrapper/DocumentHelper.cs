// <copyright file="DocumentHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Amazon.DynamoDBv2.DocumentModel;

    public static class DocumentHelper
    {
        /// <summary>
        /// Converts the user defined type to A collection of attribute key-value pairs
        /// </summary>
        /// <typeparam name="T">Entity that represents the custom type input.</typeparam>
        /// <param name="tObject">Entity that represents the custom type</param>
        /// <param name="removeNulls">Specified whether to remove the fields that are assigned null.</param>
        /// <param name="removeZeros">Specified whether to remove the fields that are assigned Zero.</param>
        /// <returns>A list of attribute key-value pairs</returns>
        public static Document ToDocument<T>(this T tObject, bool removeNulls = true, bool removeZeros = true)
        {
            var dictionary = tObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(tObject, null));
            if (removeNulls)
            {
                dictionary = dictionary.Where(x => x.Value != null).ToDictionary(y => y.Key, y => y.Value);
            }

            var document = new Document();
            foreach (var entry in dictionary)
            {
                if (entry.Value == null)
                {
                    continue;
                }

                var type = entry.Value.GetType();

                if (type == typeof(string))
                {
                    document.Add(entry.Key, (string)entry.Value);
                }
                else if (type == typeof(int) || type == typeof(int?) || type.IsEnum)
                {
                    if (removeZeros)
                    {
                        if ((int)entry.Value != 0)
                        {
                            document.Add(entry.Key, (int)entry.Value);
                        }
                    }
                    else
                    {
                        document.Add(entry.Key, (int)entry.Value);
                    }
                }
                else if (type == typeof(bool) || type == typeof(bool?))
                {
                    document.Add(entry.Key, new DynamoDBBool((bool)entry.Value));
                }
                else if (type == typeof(List<string>))
                {
                    document.Add(entry.Key, (List<string>)entry.Value);
                }
                else if (type == typeof(List<int>))
                {
                    var dynamoDBList = new DynamoDBList(new List<DynamoDBEntry>());

                    foreach (var i in (List<int>)entry.Value)
                    {
                        dynamoDBList.Add(i);
                    }

                    document.Add(entry.Key, dynamoDBList);
                }
                else if (type == typeof(DateTime))
                {
                    document.Add(entry.Key, (DateTime)entry.Value);
                }
                else if (type == typeof(float) || type == typeof(float?))
                {
                    if (removeZeros)
                    {
                        if ((float)entry.Value != 0)
                        {
                            document.Add(entry.Key, (float)entry.Value);
                        }
                    }
                    else
                    {
                        document.Add(entry.Key, (float)entry.Value);
                    }
                }
                else if (type == typeof(double) || type == typeof(double?))
                {
                    if (removeZeros)
                    {
                        if ((double)entry.Value != 0)
                        {
                            document.Add(entry.Key, (double)entry.Value);
                        }
                    }
                    else
                    {
                        document.Add(entry.Key, (double)entry.Value);
                    }
                }
                else if (type == typeof(uint?) || type == typeof(uint?))
                {
                    if (removeZeros)
                    {
                        if ((uint)entry.Value != 0)
                        {
                            document.Add(entry.Key, (uint)entry.Value);
                        }
                    }
                    else
                    {
                        document.Add(entry.Key, (uint)entry.Value);
                    }
                }
                else if (type.BaseType != null && type.BaseType.IsAssignableFrom(typeof(T)))
                {
                    if (type.BaseType == typeof(object) && type.Name == "List`1")
                    {
                        var dynamoDbList = new DynamoDBList(new List<Document>());
                        var genericList = entry.Value as IEnumerable;
                        if (genericList != null)
                        {
                            foreach (var test in genericList)
                            {
                                var dbEntry = test;
                                if (dbEntry != null)
                                {
                                    dynamoDbList.Add(ToDocument(dbEntry, removeNulls));
                                }
                            }
                        }

                        document.Add(entry.Key, dynamoDbList);
                    }
                    else if (type.BaseType != null)
                    {
                        var docEntry = entry.Value;
                        if (docEntry != null)
                        {
                            document.Add(entry.Key, ToDocument(docEntry, removeNulls));
                        }
                    }
                }
            }

            return document;
        }

        public static T ToObject<T>(this Document document, string className = "", string classNamespace = "")
        {
            return (T)ToObjectType<T>(document, className, classNamespace);
        }

        private static PropertyInfo GetPropertyInfo(string key, Type type)
        {
            var propertyinfo = type.GetProperties().SingleOrDefault(x => x.Name == key);

            return propertyinfo;
        }

        private static object ToObjectType<T>(Document document, string className = "", string classNamespace = "")
        {
            var thisNamespace = !string.IsNullOrEmpty(classNamespace) ? classNamespace : typeof(T).Namespace;

            object classObject = null;

            if (string.IsNullOrEmpty(className))
            {
                classObject = Activator.CreateInstance(typeof(T));
            }
            else
            {
                var rootObject = Activator.CreateInstance(typeof(T));

                var fullyQualifiedName = rootObject.GetType().AssemblyQualifiedName.Replace(rootObject.GetType().Name, className);

                if (rootObject.GetType().IsGenericType)
                {
                    Type elementType = rootObject.GetType().GetGenericArguments()[0];
                    classObject = Activator.CreateInstance(elementType);
                }
                else
                {
                    Type elementType = Type.GetType(fullyQualifiedName);
                    classObject = Activator.CreateInstance(elementType);
                }
            }

            if (document != null)
            {
                foreach (var entry in document)
                {
                    var propertyInfo = GetPropertyInfo(entry.Key, classObject.GetType());

                    if (propertyInfo != null)
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsString());
                        }
                        else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?) || propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsInt());
                        }
                        else if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsBoolean());
                        }
                        else if (propertyInfo.PropertyType == typeof(List<string>))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsListOfString());
                        }
                        else if (propertyInfo.PropertyType == typeof(List<int>))
                        {
                            List<int> integerList = new List<int>();
                            foreach (var dynamoDbEntry in entry.Value.AsListOfDynamoDBEntry())
                            {
                                integerList.Add(dynamoDbEntry.AsInt());
                            }

                            propertyInfo.SetValue(classObject, integerList);
                        }
                        else if (propertyInfo.PropertyType == typeof(float) || propertyInfo.PropertyType == typeof(float?))
                        {
                            propertyInfo.SetValue(classObject, (float)entry.Value.AsDouble());
                        }
                        else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsDouble());
                        }
                        else if (propertyInfo.PropertyType == typeof(uint))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsUInt());
                        }
                        else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsLong());
                        }
                        else if (propertyInfo.PropertyType == typeof(ulong) || propertyInfo.PropertyType == typeof(ulong?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsULong());
                        }
                        else if (propertyInfo.PropertyType == typeof(char) || propertyInfo.PropertyType == typeof(char?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsChar());
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                        {
                            propertyInfo.SetValue(classObject, entry.Value.AsDateTime());
                        }
                        else
                        {
                            Type valueType = entry.Value.GetType();

                            if (valueType == typeof(DynamoDBList))
                            {
                                var dynamoDbList = entry.Value as DynamoDBList;
                                var subType = propertyInfo.PropertyType.GetGenericArguments()[0];
                                var subClassName = subType.Name;

                                var genericList = typeof(List<>);
                                var stringList = genericList.MakeGenericType(subType);
                                var list = Activator.CreateInstance(stringList);

                                var addMethod = stringList.GetMethod("Add");

                                foreach (var dbEntry in dynamoDbList.Entries)
                                {
                                    var tmpDoc = dbEntry as Document;
                                    var subObject = ToObjectType<T>(tmpDoc, subClassName);
                                    addMethod.Invoke(list, new object[] { subObject });
                                }

                                propertyInfo.SetValue(classObject, list);
                            }
                            else if (valueType == typeof(Document))
                            {
                                var tmpDoc = entry.Value as Document;
                                var subDocuement = ToObjectType<T>(
                                    tmpDoc,
                                    propertyInfo.PropertyType.Name,
                                    propertyInfo.PropertyType.Namespace);
                                propertyInfo.SetValue(classObject, subDocuement);
                            }
                        }
                    }
                }
            }

            return classObject;
        }
    }
}
