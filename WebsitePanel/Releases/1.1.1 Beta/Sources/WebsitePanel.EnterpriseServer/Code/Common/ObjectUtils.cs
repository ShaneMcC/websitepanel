// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using WebsitePanel.Providers;

namespace WebsitePanel.EnterpriseServer
{
	/// <summary>
	/// Summary description for ObjectUtils.
	/// </summary>
	public class ObjectUtils
	{
        public static DT ConvertObject<ST, DT>(ST so)
        {
            Dictionary<string, PropertyInfo> sProps = GetTypePropertiesHash(typeof(ST));
            Dictionary<string, PropertyInfo> dProps = GetTypePropertiesHash(typeof(DT));

            DT dobj = (DT)Activator.CreateInstance(typeof(DT));

            // copy properties
            foreach (string propName in sProps.Keys)
            {
                if (dProps.ContainsKey(propName) && sProps[propName].Name != "Item")
                {
					if (sProps[propName].CanRead)
					{
						object val = sProps[propName].GetValue(so, null);
						if (dProps[propName] != null)
						{
							if (val != null && dProps[propName].CanWrite)
							{
								dProps[propName].SetValue(dobj, val, null);
							}
						}
					}
                }
            }
            return dobj;
        }

		private static Hashtable typeProperties = new Hashtable();

        public static Hashtable GetObjectProperties(object obj, bool persistentOnly)
        {
            Hashtable hash = new Hashtable();

            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance
                | BindingFlags.Public);
            foreach (PropertyInfo prop in props)
            {
                // check for persistent attribute
                object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
                if (!persistentOnly || (persistentOnly && attrs.Length > 0))
                {
                    object val = prop.GetValue(obj, null);
                    string s = "";
                    if (val != null)
                    {
                        if (prop.PropertyType == typeof(string[]))
                            s = String.Join(";", (string[])val);
                        else if (prop.PropertyType == typeof(int[]))
                        {
                            int[] ivals = (int[])val;
                            string[] svals = new string[ivals.Length];
                            for (int i = 0; i < svals.Length; i++)
                                svals[i] = ivals[i].ToString();
                            s = String.Join(";", svals);
                        }
                        else
                            s = val.ToString();
                    }

                    // add property to hash
                    hash.Add(prop.Name, s);
                }
            }

            return hash;
        }

		public static void FillCollectionFromDataSet<T>(List<T> list, DataSet ds)
		{
			if(ds.Tables.Count == 0)
				return;

            FillCollectionFromDataView<T>(list, ds.Tables[0].DefaultView);
		}

        public static void FillCollectionFromDataView<T>(List<T> list, DataView dv)
		{
            Type type = typeof(T);

			PropertyInfo[] props = GetTypeProperties(type);

			foreach(DataRowView dr in dv)
			{
				// create an instance
				T obj = (T)Activator.CreateInstance(type);
				list.Add(obj);

				// fill properties
				for(int i = 0; i < props.Length; i++)
				{
					string propName = props[i].Name;
					if(dv.Table.Columns[propName] == null)
						continue;

					object propVal = dr[propName];
					if(propVal == DBNull.Value)
						props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
					else
					{
						try
						{
							// try implicit type conversion
							props[i].SetValue(obj, propVal, null);
						}
						catch
						{
							// convert to string and then set property value
							try
							{
								string strVal = propVal.ToString();
								props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
							}
							catch
							{
								// skip property init
							}
						}
					}
				} // for properties
			} // for rows
		}

        public static List<T> CreateListFromDataReader<T>(IDataReader reader)
        {
            List<T> list = new List<T>();
            FillCollectionFromDataReader<T>(list, reader);
            return list;
        }

        public static List<T> CreateListFromDataSet<T>(DataSet ds)
        {
            List<T> list = new List<T>();
            FillCollectionFromDataSet<T>(list, ds);
            return list;
        }

		public static void FillCollectionFromDataReader<T>(List<T> list, IDataReader reader)
		{
            Type type = typeof(T);

			try
			{
				// get type properties
				PropertyInfo[] props = GetTypeProperties(type);

				// iterate through reader
				while(reader.Read())
				{
					T obj = (T)Activator.CreateInstance(type);
					list.Add(obj);

					// set properties
					for(int i = 0; i < props.Length; i++)
					{
						string propName = props[i].Name;
						
						try
						{

							object propVal = reader[propName];
							if(propVal == DBNull.Value)
								props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
							else
							{
								try
								{
									// try implicit type conversion
									props[i].SetValue(obj, propVal, null);
								}
								catch
								{
									// convert to string and then set property value
									try
									{
										string strVal = propVal.ToString();
										props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
									}
									catch
									{
										// skip property init
									}
								}
							}
						}
						catch{} // just skip
					} // for properties
				}
			}
			finally
			{
				reader.Close();
			}
		}

		public static T FillObjectFromDataView<T>(DataView dv)
		{
            Type type = typeof(T);
			T obj = default(T);

			// get type properties
			PropertyInfo[] props = GetTypeProperties(type);

			// iterate through reader
			foreach(DataRowView dr in dv)
			{
				obj = (T)Activator.CreateInstance(type);

				// set properties
				for(int i = 0; i < props.Length; i++)
				{
					string propName = props[i].Name;
					
					try
					{
						// verify if there is such a column
						if (!dr.Row.Table.Columns.Contains(propName.ToLower()))
						{
							// if not, we move to another property
							// because this one we cannot set
							continue;
						}

						object propVal = dr[propName];
						if(propVal == DBNull.Value)
							props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
						else
						{
							try
							{
								string strVal = propVal.ToString();

								//convert to DateTime 
								if (props[i].PropertyType.UnderlyingSystemType.FullName == typeof(DateTime).FullName)
								{
									DateTime date = DateTime.MinValue;
									if (DateTime.TryParse(strVal, out date))
									{
										props[i].SetValue(obj, date, null);
									}
								}
								else
								{
									//Convert generic
									props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
								}
							}
							catch
							{
								// skip property init 
							}
						}
					}
					catch{} // just skip
				} // for properties
			}

			return obj;
		}

		public static T FillObjectFromDataReader<T>(IDataReader reader)
        {
            Type type = typeof(T);

            T obj = default(T);

			try
			{
				// get type properties
				PropertyInfo[] props = GetTypeProperties(type);

				// iterate through reader
				while(reader.Read())
				{
					obj = (T)Activator.CreateInstance(type);

					// set properties
					for(int i = 0; i < props.Length; i++)
					{
						string propName = props[i].Name;

						try
						{
							if (!IsColumnExists(propName, reader.GetSchemaTable()))
							{
								continue;
							}
							object propVal = reader[propName];
                            
							if(propVal == DBNull.Value)
								props[i].SetValue(obj, GetNull(props[i].PropertyType), null);
							else
							{
								try
								{
									//try string first
									if (props[i].PropertyType.UnderlyingSystemType.FullName == typeof(String).FullName)
									{
										props[i].SetValue(obj, propVal.ToString(), null);
									}
									else
									{
										// then, try implicit type conversion
										props[i].SetValue(obj, propVal, null);
									}
								}
								catch
								{
									// convert to string and then set property value
									try
									{
										string strVal = propVal.ToString();
										props[i].SetValue(obj, Cast(strVal, props[i].PropertyType), null);
									}
									catch
									{
										// skip property init
									}
								}
							}
						}
						catch{} // just skip
					} // for properties
				}
			}
			finally
			{
				reader.Close();
			}

			return obj;
		}

        private static Hashtable propertiesCache = new Hashtable();

        public static object CreateObjectFromDataview(Type type, DataView dv,
            string nameColumn, string valueColumn, bool persistentOnly)
        {
            // create hash of properties from datareader
            Hashtable propValues = new Hashtable();
            foreach (DataRowView dr in dv)
            {
                if (propValues[dr[nameColumn]] == null)
                    propValues.Add(dr[nameColumn], dr[valueColumn]);
            }

            return CreateObjectFromHash(type, propValues, persistentOnly);
        }

        public static object CreateObjectFromDataReader(Type type, IDataReader reader,
            string nameColumn, string valueColumn, bool persistentOnly)
        {
            // create hash of properties from datareader
            Hashtable propValues = new Hashtable();
            while (reader.Read())
            {
                if (propValues[reader[nameColumn]] == null)
                    propValues.Add(reader[nameColumn], reader[valueColumn]);
            }
            reader.Close();

            return CreateObjectFromHash(type, propValues, persistentOnly);
        }

        public static object CreateObjectFromHash(Type type, Hashtable propValues, bool persistentOnly)
        {
            // create object
            object obj = Activator.CreateInstance(type);

            CreateObjectFromHash(obj, propValues, persistentOnly);

            return obj;
        }

        public static void CreateObjectFromHash(object obj, Hashtable propValues, bool persistentOnly)
        {
            Type type = obj.GetType();

            // get all property infos
            Hashtable props = null;
            if (propertiesCache[type.Name] != null)
            {
                // load properties from cache
                props = (Hashtable)propertiesCache[type.Name];
            }
            else
            {
                // create properties cache
                props = new Hashtable();
                PropertyInfo[] objProps = type.GetProperties(BindingFlags.Instance
                    //| BindingFlags.DeclaredOnly
                    | BindingFlags.Public);
                foreach (PropertyInfo prop in objProps)
                {
                    // check for persistent attribute
                    object[] attrs = prop.GetCustomAttributes(typeof(PersistentAttribute), false);
                    if (!persistentOnly || (persistentOnly && attrs.Length > 0))
                    {
                        // add property to hash
                        props.Add(prop.Name, prop);
                    }
                }

                // add to cache
                propertiesCache.Add(type.Name, props);
            }

            // fill properties
            foreach (string propName in propValues.Keys)
            {
                // try to locate specified property
                if (props[propName] != null)
                {
                    // set property
                    // we support:
                    //	String
                    //	Int32
                    //	Boolean
                    //	Float
                    PropertyInfo prop = (PropertyInfo)props[propName];
                    string val = propValues[propName].ToString();
                    if (prop.PropertyType == typeof(String))
                        prop.SetValue(obj, val, null);
                    else if (prop.PropertyType == typeof(Int32))
                        prop.SetValue(obj, Int32.Parse(val), null);
                    else
                        if (prop.PropertyType == typeof(long))
                            prop.SetValue(obj, long.Parse(val), null);
                    else 
                        if (prop.PropertyType == typeof(Boolean))
                        prop.SetValue(obj, Boolean.Parse(val), null);
                    else if (prop.PropertyType == typeof(Single))
                        prop.SetValue(obj, Single.Parse(val), null);
                    else if (prop.PropertyType.IsEnum)
                        prop.SetValue(obj, Enum.Parse(prop.PropertyType, val, true), null);
                    else 
                        if (prop.PropertyType == typeof(Guid))
                            prop.SetValue(obj, new Guid(val), null);
                     else
                        if (prop.PropertyType == typeof(string[]))
                    {
                        if (val == "")
                            prop.SetValue(obj, new string[0], null);
                        else
                            prop.SetValue(obj, val.Split(';'), null);
                    }
                    else if (prop.PropertyType == typeof(int[]))
                    {
                        string[] svals = val.Split(';');
                        int[] ivals = new int[svals.Length];

                        for (int i = 0; i < svals.Length; i++)
                            ivals[i] = Int32.Parse(svals[i]);

                        if (val == "")
                            ivals = new int[0];

                        prop.SetValue(obj, ivals, null);
                    }
                }
            }
        }

        private static Dictionary<string, PropertyInfo> GetTypePropertiesHash(Type type)
        {
            Dictionary<string, PropertyInfo> hash = new Dictionary<string, PropertyInfo>();
            PropertyInfo[] props = GetTypeProperties(type);
            foreach (PropertyInfo prop in props)
                hash.Add(prop.Name, prop);
            return hash;
        }

		private static PropertyInfo[] GetTypeProperties(Type type)
		{
			string typeName = type.AssemblyQualifiedName;
			if(typeProperties[typeName] != null)
				return (PropertyInfo[])typeProperties[typeName];

			PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			typeProperties[typeName] = props;
			return props;
		}

		public static object GetNull(Type type)
		{
			if(type == typeof(string))
				return null;
			if(type == typeof(Int32))
				return 0;
			if(type == typeof(Int64))
				return 0;
			if(type == typeof(Boolean))
				return false;
			if(type == typeof(Decimal))
				return 0M;
			else
				return null;
		}

		public static object Cast(string val, Type type)
		{
			if(type == typeof(string))
				return val;
			if(type == typeof(Int32))
				return Int32.Parse(val);
			if(type == typeof(Int64))
				return Int64.Parse(val);
			if(type == typeof(Boolean))
				return Boolean.Parse(val);
			if(type == typeof(Decimal))
				return Decimal.Parse(val);
            if(type == typeof(string[]) && val != null)
            {
                return val.Split(';');
            }
            if (type.IsEnum)
                return Enum.Parse(type, val, true);

            if (type == typeof(int[]) && val != null)
            {
                string[] sarr = val.Split(';');
                int[] iarr = new int[sarr.Length];
                for (int i = 0; i < sarr.Length; i++)
                    iarr[i] = Int32.Parse(sarr[i]);
                return iarr;
            }
			else
				return val;
		}

		public static string GetTypeFullName(Type type)
		{
			return type.FullName + ", " + type.Assembly.GetName().Name;
		}


		#region Helper Functions

		/// <summary>
		/// This function is used to determine whether IDataReader contains a Column.
		/// </summary>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="schemaTable">The schema <see cref="DataTable"/> that decribes result-set <see cref="IDataReader"/> contains.</param>
		/// <returns>True, when required column exists in the <paramref name="schemaTable"/>. Otherwise, false.</returns>
		/// <remark>
		/// The followin example shows how to look for the "Role" column in the <see cref="IDataReader"/>.
		/// <example>
		/// IDataReader reader = ....
		/// if (!IsColumnExists("Role", reader.GetSchemaTable())
		/// {
		///		continue;
		/// }
		/// 
		/// object roleValue = reader["Role"];
		/// </example>
		/// </remark>
		static bool IsColumnExists(string columnName, DataTable schemaTable)
		{
			foreach (DataRow row in schemaTable.Rows)
			{
				if (String.Compare(row[0].ToString(), columnName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}

			return false;
		}

		#endregion
	}
}
