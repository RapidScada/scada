/*
 * Copyright 2018 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaData.Dao
 * Summary  : The class to simplify work with parameters of an SQL command
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2018
 */

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Scada.Dao
{
    /// <summary>
    /// The class to simplify work with parameters of an SQL command.
    /// <para>Класс для упрощения работы с параметрами SQL-команды.</para>
    /// </summary>
    public class RequestParams
    {
        /// <summary>
        /// Parameter of SQL command.
        /// </summary>
        protected class Param
        {
            /// <summary>
            /// Gets or sets the statement to insert into an SQL clause.
            /// </summary>
            public string SqlStatement { get; set; }
            /// <summary>
            /// Gets or sets the parameter name of used by a command.
            /// </summary>
            public string ParameterName { get; set; }
            /// <summary>
            /// Gets or sets the parameter value.
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// Adds the parameter to the command.
            /// </summary>
            public virtual void AddToCommand(DbCommand command)
            {
                DbParameter dbParam = command.CreateParameter();
                dbParam.ParameterName = ParameterName;
                dbParam.Value = Value;
                command.Parameters.Add(dbParam);
            }
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RequestParams()
        {
            ParamList = new List<Param>();
        }


        /// <summary>
        /// Gets or sets the list of the request parameters.
        /// </summary>
        protected List<Param> ParamList { get; set; }


        /// <summary>
        /// Adds a new parameter with the specified properties.
        /// </summary>
        public void Add(string sqlStatement, string parameterName, object value, bool condition = true)
        {
            if (condition)
            {
                ParamList.Add(new Param()
                {
                    SqlStatement = sqlStatement,
                    ParameterName = parameterName,
                    Value = value
                });
            }
        }

        /// <summary>
        /// Builds a string contains WHERE clause that includes the parameters.
        /// </summary>
        public string BuildWhereClause()
        {
            if (ParamList.Count > 0)
            {
                StringBuilder sb = new StringBuilder("WHERE ");
                bool first = true;

                foreach (Param param in ParamList)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append(" AND ");

                    sb.Append(param.SqlStatement);
                }

                return sb.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Adds the parameters to the command.
        /// </summary>
        public void AddToCommand(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            foreach (Param param in ParamList)
            {
                param.AddToCommand(command);
            }
        }
    }
}
