﻿/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BdsSoft.SharePoint.Linq;
using Microsoft.SharePoint;
using System.Reflection;

#endregion

namespace Tests
{
    public class SharePointListEntityTest
    {
        public SharePointListEntityTest()
            : base()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj.GetType() != this.GetType())
                return false;
            else
            {
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    FieldAttribute fa = GetFieldAttribute(prop);
                    if (fa != null)
                    {
                        object o1 = prop.GetValue(obj, null);
                        object o2 = prop.GetValue(this, null);
                        if (!object.Equals(o1, o2))
                            return false;
                    }
                }

                return true;
            }
        }

        public override int GetHashCode()
        {
            int hash = 0;

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                FieldAttribute fa = GetFieldAttribute(prop);
                if (fa != null)
                {
                    object o = prop.GetValue(this, null);
                    if (o != null)
                        hash = hash ^ o.GetHashCode();
                }
            }
            return hash;
        }

        public static void Add(SelfDestructingList lst, object e)
        {
            Add(lst.List, e);
        }

        public static void Add(SPList lst, object e)
        {
            SPListItem item = lst.Items.Add();

            foreach (PropertyInfo prop in e.GetType().GetProperties())
            {
                FieldAttribute fa = GetFieldAttribute(prop);
                if (fa != null && !fa.PrimaryKey && !fa.ReadOnly)
                {
                    object o = prop.GetValue(e, null);
                    if (o == null)
                        continue;

                    if (fa.FieldType == FieldType.URL)
                    {
                        UrlValue uVal = (UrlValue)o;
                        SPFieldUrlValue val = new SPFieldUrlValue();
                        val.Url = uVal.Url;
                        val.Description = uVal.Description;
                        item[fa.Field] = val;
                    }
                    else
                        item[fa.Field] = o;
                }
            }

            item.Update();
        }

        public static SPList CreateList<T>(SPWeb web)
        {
            ListAttribute la = GetListAttribute(typeof(T));

            SPList lst;
            try
            {
                lst = web.Lists[la.List];
                if (lst != null)
                    lst.Delete();
            }
            catch { }

            web.Lists.Add(la.List, "", SPListTemplateType.GenericList);
            lst = web.Lists[la.List];
            lst.OnQuickLaunch = true;
            lst.Update();

            return lst;
        }

        public static SelfDestructingList Create<T>(SPWeb web) where T : new()
        {
            SPList lst = CreateList<T>(web);

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                FieldAttribute fa = GetFieldAttribute(prop);
                if (fa != null && !fa.PrimaryKey)
                {
                    bool nullable = false;
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        nullable = true;

                    lst.Fields.Add(fa.Field, (SPFieldType)fa.FieldType, !nullable); //TODO: add extended information
                    lst.Views[0].ViewFields.Add(lst.Fields[fa.Field]);
                }
            }

            return new SelfDestructingList() { List = lst };
        }

        private static ListAttribute GetListAttribute(Type t)
        {
            ListAttribute[] la = t.GetCustomAttributes(typeof(ListAttribute), false) as ListAttribute[];
            if (la != null && la.Length != 0)
                return la[0];
            else
                throw new InvalidOperationException("Missing ListAttribute on the entity type.");
        }

        private static FieldAttribute GetFieldAttribute(PropertyInfo member)
        {
            FieldAttribute[] fa = member.GetCustomAttributes(typeof(FieldAttribute), false) as FieldAttribute[];
            if (fa != null && fa.Length != 0 && fa[0] != null)
                return fa[0];
            else
                return null;
        }
    }

    class CustomList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Field> Fields { get; set; }
    }

    class Field
    {
        public string Name { get; set; }
        public SPFieldType Type { get; set; }
        public bool Required { get; set; }
    }

    public class SelfDestructingList : IDisposable
    {
        public SPList List { get; set; }

        public void Dispose()
        {
            List.Delete();
        }
    }
}
