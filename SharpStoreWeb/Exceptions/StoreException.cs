using SharpStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using jcReactive.Common;
namespace SharpStoreWeb.Exceptions
{
    public class StoreException : Exception
    {
        public StoreException(params string[] messages) : base(string.Join(" ", messages))
        {

        }

        public StoreException(List<string> messages, params string[] parameters) : base(FormatLocalizedMessage(messages, parameters))
        {

        }

        private static string FormatLocalizedMessage(List<string> messages, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            List<string> localizedParameters = new List<string>();
            messages.ForEach(message =>
            {
                var localized = Localizations.ResourceManager.GetString(message);

                sb.Append(sb.Length == 0 ? $"{localized} " : $"{localized.ToLower()} ");
            });
            parameters.ToList().ForEach(parameter =>
            {
                string p = parameter;
                try
                {
                    
                    p = Localizations.ResourceManager.GetString(parameter).ToLower();
                }
                catch (Exception) { }
                finally
                {
                    localizedParameters.Add(p);
                }

            });
            return sb.ToString().Fmt(localizedParameters.ToArray());


        }
    }
}