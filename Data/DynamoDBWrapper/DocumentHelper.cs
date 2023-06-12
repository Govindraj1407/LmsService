// <copyright file="DocumentHelper.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Assists with transposing documents
   /// </summary>
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
         Dictionary<string, object> dictionary = tObject.GetType()
             .GetProperties(BindingFlags.Instance | BindingFlags.Public)
             .ToDictionary(prop => prop.Name, prop => prop.GetValue(tObject, null));
         if (removeNulls)
         {
            dictionary = dictionary.Where(x => x.Value != null).ToDictionary(y => y.Key, y => y.Value);
         }

         Document document = new Document();
         foreach (KeyValuePair<string, object> entry in dictionary)
         {
            if (entry.Value == null)
            {
               continue;
            }

            Type type = entry.Value.GetType();

            if (type == typeof(string))
            {
               document.Add(entry.Key, (string)entry.Value);
            }
            else if (type == typeof(int) || type == typeof(int?) || type.IsEnum)
            {
               int val = (int)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
               long val = (long)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(ulong) || type == typeof(ulong?))
            {
               ulong val = (ulong)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
               float val = (float)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
               double val = (double)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(uint) || type == typeof(uint?))
            {
               uint val = (uint)entry.Value;
               if (!removeZeros || val != 0)
                  document.Add(entry.Key, val);
            }
            else if (type == typeof(char))
            {
               document.Add(entry.Key, (char)entry.Value);
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
               DynamoDBList dynamoDBList = new DynamoDBList(new List<DynamoDBEntry>());

               foreach (int i in (List<int>)entry.Value)
               {
                  dynamoDBList.Add(i);
               }

               document.Add(entry.Key, dynamoDBList);
            }
            else if (type == typeof(DateTime))
            {
               document.Add(entry.Key, (DateTime)entry.Value);
            }
            else if (type.BaseType != null && type.BaseType.IsAssignableFrom(typeof(T)))
            {
               if (type.BaseType == typeof(object) && type.Name == "List`1")
               {
                  DynamoDBList dynamoDbList = new DynamoDBList(new List<Document>());
                  IEnumerable genericList = entry.Value as IEnumerable;
                  if (genericList != null)
                  {
                     foreach (object test in genericList)
                     {
                        object dbEntry = test;
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
                  object docEntry = entry.Value;
                  if (docEntry != null)
                  {
                     document.Add(entry.Key, ToDocument(docEntry, removeNulls));
                  }
               }
            }
         }

         return document;
      }

      /// <summary>
      /// Converts document to another object type
      /// </summary>
      /// <typeparam name="T">Object type</typeparam>
      /// <param name="document">Document</param>
      /// <param name="className">Class name</param>
      /// <returns>Object</returns>
      public static T ToObject<T>(this Document document, string className = "")
      {
         return (T)ToObjectType<T>(document, className);
      }

      private static PropertyInfo GetPropertyInfo(string key, Type type)
      {
         PropertyInfo propertyinfo = type.GetProperties().SingleOrDefault(x => x.Name == key);

         return propertyinfo;
      }

      private static object ToObjectType<T>(Document document, string className = "")
      {
         object classObject = null;

         if (string.IsNullOrEmpty(className))
         {
            classObject = Activator.CreateInstance(typeof(T));
         }
         else
         {
            object rootObject = Activator.CreateInstance(typeof(T));

            string fullyQualifiedName = rootObject.GetType().AssemblyQualifiedName.Replace(rootObject.GetType().Name, className);

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
            foreach (KeyValuePair<string, DynamoDBEntry> entry in document)
            {
               PropertyInfo propertyInfo = GetPropertyInfo(entry.Key, classObject.GetType());

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
                     foreach (DynamoDBEntry dynamoDbEntry in entry.Value.AsListOfDynamoDBEntry())
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
                        DynamoDBList dynamoDbList = entry.Value as DynamoDBList;
                        Type subType = propertyInfo.PropertyType.GetGenericArguments()[0];
                        string subClassName = subType.Name;

                        Type genericList = typeof(List<>);
                        Type stringList = genericList.MakeGenericType(subType);
                        object list = Activator.CreateInstance(stringList);

                        MethodInfo addMethod = stringList.GetMethod("Add");

                        foreach (DynamoDBEntry dbEntry in dynamoDbList.Entries)
                        {
                           Document tmpDoc = dbEntry as Document;
                           object subObject = ToObjectType<T>(tmpDoc, subClassName);
                           addMethod.Invoke(list, new object[] { subObject });
                        }

                        propertyInfo.SetValue(classObject, list);
                     }
                     else if (valueType == typeof(Document))
                     {
                        Document tmpDoc = entry.Value as Document;
                        object subDocument = ToObjectType<T>(
                            tmpDoc,
                            propertyInfo.PropertyType.Name);
                        propertyInfo.SetValue(classObject, subDocument);
                     }
                  }
               }
            }
         }

         return classObject;
      }
   }
}
