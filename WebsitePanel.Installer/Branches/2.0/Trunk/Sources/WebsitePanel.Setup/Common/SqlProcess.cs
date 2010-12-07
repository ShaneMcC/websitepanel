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
// - Neither  the  appPoolName  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
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
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Shows sql script process.
	/// </summary>
	public sealed class SqlProcess
	{
		private ProgressBar progressBar;
		private string scriptFile; 
		private string connectionString;
		private string database;
		
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param appPoolName="bar">Progress bar.</param>
		/// <param appPoolName="file">Sql script file</param>
		/// <param appPoolName="connection">Sql connection string</param>
		/// <param appPoolName="db">Sql server database appPoolName</param>
		public SqlProcess(ProgressBar bar, string file, string connection, string db)
		{
			this.progressBar = bar;
			this.scriptFile = file;
			this.connectionString = connection;
			this.database = db;
		}

		/// <summary>
		/// Executes sql script file.
		/// </summary>
		internal void Run()
		{
			int commandCount = 0;
			int i = 0;
			string sql = string.Empty;

			try
			{
				using (StreamReader sr = new StreamReader(scriptFile))
				{
					while( null != (sql = ReadNextStatementFromStream(sr))) 
					{
						commandCount++;
					}					
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Can't read SQL script " + scriptFile, ex);
			}

			Log.WriteInfo(string.Format("Executing {0} database commands", commandCount));

			progressBar.Minimum = 0;
			progressBar.Maximum = 100;
			progressBar.Value = 0;

			SqlConnection connection = new SqlConnection(connectionString);

			try
			{
				// iterate through "GO" delimited command text
				using (StreamReader reader = new StreamReader(scriptFile))
				{
					SqlCommand command = new SqlCommand();
					connection.Open();
					command.Connection = connection;
					command.CommandType	= System.Data.CommandType.Text;
                    command.CommandTimeout = 600;
				
					while( null != (sql = ReadNextStatementFromStream(reader))) 
					{
						sql = ProcessInstallVariables(sql);
						command.CommandText = sql;
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error executing SQL command: " + sql, ex);
                        }

						i++;
						if ( commandCount != 0)
						{
							progressBar.Value = Convert.ToInt32(i*100/commandCount);
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Can't run SQL script "+scriptFile, ex);
			}

			connection.Close();
		}

		private string ReadNextStatementFromStream(StreamReader reader)
		{
			StringBuilder sb = new StringBuilder();
			string lineOfText;
	
			while(true) 
			{
				lineOfText = reader.ReadLine();
				if( lineOfText == null ) 
				{
					if( sb.Length > 0 ) 
					{
						return sb.ToString();
					}
					else 
					{
						return null;
					}
				}

				if(lineOfText.TrimEnd().ToUpper() == "GO") 
				{
					break;
				}
				
				sb.Append(lineOfText + Environment.NewLine);
			}

			return sb.ToString();
		}

		private string ProcessInstallVariables(string input)
		{
			//replace install variables
			string output = input;
			output = output.Replace("${install.database}", database);
			return output;
		}
	}
}
