using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Request_Sender
{
    /// <summary>
    /// form for saving data for ewquest HTTP
    /// </summary>
    public  class RequestForm : IDisposable
    {
        /// <summary>
        /// dictonary for caching data
        /// </summary>
        private Dictionary<string, string> _data = new Dictionary<string, string>(0);
        /// <summary>
        /// get the data form
        /// </summary>
        /// <returns>dictonary data</returns>
        public Dictionary<string, string> GetData ()
        {
            return data;
        }
       
        /// <summary>
        /// Generate GET URL with dictonary data Example: example.com/?item=value
        /// </summary>
        /// <param name="URL">URL for generating</param>
        /// <returns>GET URL</returns>
        public string GenerateGETRequest (string URL)
        {
            string result = URL + "?";
            int i = 0;
            foreach (KeyValuePair<string, string> entry in data)
            {
                result += entry.Key + "=" + entry.Value;

                if (i < data.Count)
                {
                    i++;
                    result += "&";
                }
            }
          result = result.Remove(result.Length - 1);
            return result;
        }
        /// <summary>
        /// removing data with element
        /// </summary>
        /// <param name="formElement">element target</param>
        public void Remove(RequestFormElement formElement)
        {
            try
            {
            data.Remove(formElement.name);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// for clear in memoey
        /// </summary>
        public void Dispose()
        {
            _data.Clear();
            _data = null;
        }
        
         /// <summary>
        /// add element data in dictory with RequestFormElement
        /// </summary>
        /// <param name="formElement">element</param>
        public void Add (RequestFormElement formElement) =>  _data.Add(formElement.name, formElement.value);
    }
}
