/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : KpHttpNotif
 * Summary  : Represents a parameterized string
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System.Collections.Generic;
using System.Web;

namespace Scada.Comm.Devices.HttpNotif
{
    /// <summary>
    /// Represents a parameterized string.
    /// <para>Представляет параметризованную строку.</para>
    /// </summary>
    internal class ParamString
    {
        /// <summary>
        /// Represents a string parameter.
        /// </summary>
        public class Param
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public Param(string name)
            {
                Name = name;
                PartIndices = new List<int>();
            }

            /// <summary>
            /// Gets the parameter name.
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Gets the parameter indices among the parts of a string.
            /// </summary>
            public List<int> PartIndices { get; }
        }


        /// <summary>
        /// The default character that marks the beginning of a parameter.
        /// </summary>
        public const char DefaultParamBegin = '{';
        /// <summary>
        /// The default character that marks the end of a parameter.
        /// </summary>
        public const char DefaultParamEnd = '}';


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParamString(string sourceString, char paramBegin, char paramEnd)
        {
            Parse(sourceString, paramBegin, paramEnd);
        }


        /// <summary>
        /// Gets the string parts.
        /// </summary>
        public string[] StringParts { get; private set; }

        /// <summary>
        /// Gets the parameters accessed by name.
        /// </summary>
        public Dictionary<string, Param> Params { get; private set; }


        /// <summary>
        /// Parses the specified string, creates string parts and parameters.
        /// </summary>
        private void Parse(string sourceString, char paramBegin, char paramEnd)
        {
            List<string> stringParts = new List<string>();
            Dictionary<string, Param> stringParams = new Dictionary<string, Param>();

            // split the string into parts separated by curly braces { and }
            if (!string.IsNullOrEmpty(sourceString))
            {
                int ind = 0;
                int len = sourceString.Length;

                while (ind < len)
                {
                    int braceInd1 = sourceString.IndexOf(paramBegin, ind);
                    if (braceInd1 < 0)
                    {
                        stringParts.Add(sourceString.Substring(ind));
                        break;
                    }
                    else
                    {
                        int braceInd2 = sourceString.IndexOf(paramEnd, braceInd1 + 1);
                        int paramNameLen = braceInd2 - braceInd1 - 1;

                        if (paramNameLen <= 0)
                        {
                            stringParts.Add(sourceString.Substring(ind));
                            break;
                        }
                        else
                        {
                            string paramName = sourceString.Substring(braceInd1 + 1, paramNameLen);

                            if (!stringParams.TryGetValue(paramName, out Param param))
                            {
                                param = new Param(paramName);
                                stringParams.Add(paramName, param);
                            }

                            stringParts.Add(sourceString.Substring(ind, braceInd1 - ind));
                            param.PartIndices.Add(stringParts.Count);
                            stringParts.Add(""); // empty parameter value
                            ind = braceInd2 + 1;
                        }
                    }
                }
            }

            StringParts = stringParts.ToArray();
            Params = stringParams;
        }

        /// <summary>
        /// Sets the parameter value escaped by the specified method.
        /// </summary>
        private void SetParam(string name, string value, EscapingMethod escapingMethod)
        {
            if (Params.TryGetValue(name, out Param param))
            {
                if (escapingMethod == EscapingMethod.EncodeUrl)
                    value = HttpUtility.UrlEncode(value); // or WebUtility.UrlEncode or Uri.EscapeDataString
                else if (escapingMethod == EscapingMethod.EncodeJson)
                    value = HttpUtility.JavaScriptStringEncode(value, false);

                foreach (int index in param.PartIndices)
                {
                    StringParts[index] = value;
                }
            }
        }

        /// <summary>
        /// Resets the parameter values.
        /// </summary>
        public void ResetParams(IDictionary<string, string> args, EscapingMethod escapingMethod)
        {
            // clear all parts
            foreach (Param param in Params.Values)
            {
                foreach (int index in param.PartIndices)
                {
                    StringParts[index] = "";
                }
            }

            // set new values
            if (args != null)
            {
                foreach (var arg in args)
                {
                    SetParam(arg.Key, arg.Value, escapingMethod);
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("", StringParts);
        }
    }
}
