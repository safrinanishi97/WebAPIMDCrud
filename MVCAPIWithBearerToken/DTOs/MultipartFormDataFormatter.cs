using MVCAPIWithBearerToken.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace MVCAPIWithBearerToken.DTOs
{
    public class MultipartFormDataFormatter : MediaTypeFormatter
    {
        public MultipartFormDataFormatter()
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers. MediaTypeHeaderValue("multipart/form-data"));
        }
        public override bool CanReadType(Type type)
        {
            return type == typeof(OrderRequest);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var multiparData = await content.ReadAsMultipartAsync();
            var orderData=new OrderRequest();
            foreach (var item in multiparData.Contents)
            {
                var fieldName= item.Headers.ContentDisposition.Name.Trim('\"');
                if (fieldName == "Order")
                {
                    var orderContent = await item.ReadAsStringAsync();
                    orderData.Order = JsonConvert.DeserializeObject<Order>(orderContent);
                }
                else if (fieldName == "ImageFile") 
                {
                    orderData.ImageFile = await item.ReadAsByteArrayAsync();
                    orderData.ImageFilename = item.Headers.ContentDisposition.FileName;
                }
            }
            return orderData;
        }
    }
}