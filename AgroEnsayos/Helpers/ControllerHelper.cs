using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AgroEnsayos.Helpers
{
    public static partial class ControllerHelper
    {
        public static string ToJson(this object _object)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_object);
        }

        public static T FromJSon<T>(this string _object)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_object);
        }

        public static object FromJSon(this string _object)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(_object);
        }
    }

    public class JsonResultObject
    {
        public string message { get; set; }
        public object result { get; set; }
        public bool haserror { get; set; }
    }
}