using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShoppifyMiddleware.Services
{
    public static class ReflectionService
    {
        /// <summary>
        /// Checks if object is null
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static bool Is(this object A)
        {
            return A != null;
        }
        
        /// <summary>
        /// It will copy properties from caller object to the object assigned as Source
        /// </summary>
        /// <typeparam name="TSelf">Object from which we're copying properties</typeparam>
        /// <typeparam name="TSource">Object to which we're copying properties</typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void CopyPropertiesTo<TSelf, TSource>(this TSelf A, TSource B)
        {
            var AProperties = A.GetType().GetProperties();
            var BProperties = B.GetType().GetProperties();

            foreach (var item in AProperties)
            {
                var targetProperty = BProperties.Where(f => f.Name == item.Name).FirstOrDefault();
                if (targetProperty != null)
                {
                    if (item.PropertyType.Name != "Nullable`1")
                    {
                        if (targetProperty.PropertyType.Name == item.PropertyType.Name)
                        {
                            targetProperty.SetValue(B, item.GetValue(A));
                        }
                        else if (targetProperty.PropertyType.Name.ToUpper().Contains("DATETIME") && item.PropertyType.Name.ToUpper().Contains("STRING"))
                        {
                            var objData = item.CustomAttributes.Where(m => m.AttributeType.FullName == "System.Web.Mvc.AdditionalMetadataAttribute").FirstOrDefault();
                            if (objData != null)
                            {
                                var formatObject = objData.ConstructorArguments.Where(f => Convert.ToString(f.Value) == "FORMAT").FirstOrDefault();
                                if (formatObject != null)
                                {
                                    targetProperty.SetValue(B, (object)(DateTime.ParseExact(Convert.ToString(item.GetValue(A)),
                                        Convert.ToString(objData.ConstructorArguments.ElementAtOrDefault(1).Value), new System.Globalization.CultureInfo("en-US"))));
                                }
                            }
                        }
                        else if (targetProperty.PropertyType.Name.ToUpper().Contains("STRING") && item.PropertyType.Name.ToUpper().Contains("DATETIME"))
                        {
                            var objData = targetProperty.CustomAttributes.Where(m => m.AttributeType.FullName == "System.Web.Mvc.AdditionalMetadataAttribute").FirstOrDefault();
                            if (objData != null)
                            {
                                var formatObject = objData.ConstructorArguments.Where(f => Convert.ToString(f.Value) == "FORMAT").FirstOrDefault();
                                if (formatObject != null)
                                {
                                    targetProperty.SetValue(B, (object)(((DateTime)item.GetValue(A)).ToString(Convert.ToString(objData.ConstructorArguments.ElementAtOrDefault(1).Value))));
                                    //targetProperty.SetValue(B, (object)(DateTime.ParseExact(Convert.ToString(item.GetValue(A)),
                                    //    Convert.ToString(objData.ConstructorArguments.ElementAtOrDefault(1).Value), new System.Globalization.CultureInfo("en-US"))));
                                }
                            }
                        }
                        else if (targetProperty.PropertyType.Name.ToUpper().Contains("INT32") && item.PropertyType.Name.ToUpper().Contains("STRING"))
                        {
                            targetProperty.SetValue(B, int.Parse(Convert.ToString(item.GetValue(A))));
                        }
                    }
                    else
                    {
                        if (targetProperty.PropertyType.Name == item.PropertyType.GenericTypeArguments[0].Name)
                        {
                            targetProperty.SetValue(B, item.GetValue(A));
                        }
                        else if (targetProperty.PropertyType.Name.ToUpper() == "STRING" && item.PropertyType.GenericTypeArguments[0].Name.ToUpper() == "DATETIME")
                        {
                            var objData = targetProperty.CustomAttributes.Where(m => m.AttributeType.FullName == "System.Web.Mvc.AdditionalMetadataAttribute").FirstOrDefault();
                            var valueItem = item.GetValue(A);
                            if (objData != null)
                            {
                                var formatObject = objData.ConstructorArguments.Where(f => Convert.ToString(f.Value) == "FORMAT").FirstOrDefault();
                                if (formatObject != null && valueItem != null)
                                {
                                    targetProperty.SetValue(B, (object)(string.Format("{0:" + objData.ConstructorArguments.ElementAtOrDefault(1).Value + "}", valueItem)));
                                }
                            }
                            else
                            {
                                targetProperty.SetValue(B, valueItem != null ? item.GetValue(A).ToString() : "");
                            }
                        }
                    }
                }
            }
        }
    }
}
