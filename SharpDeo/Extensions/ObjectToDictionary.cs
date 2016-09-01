using SharpDeo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharpDeo.Extensions {
    // http://stackoverflow.com/questions/11576886/how-to-convert-object-to-dictionarytkey-tvalue-in-c
    // http://stackoverflow.com/questions/6445045/c-sharp-getting-all-the-properties-of-an-object
    public static class ObjectToDictionary {
        // json might be faster
        public static Dictionary<string, string> ToDictionaryOfString<T>(this T source) {
            return source?.GetType ().GetProperties ().ToDictionary (
                property => property.Name,
                property => property.GetValue (source).ToString ());
        }
    }
}