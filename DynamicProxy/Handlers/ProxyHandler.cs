﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicProxy.Handlers
{
    //public class ProxyHandler : DelegatingHandler
    //{
    //    private MetadataProvider _metadataProvider;
    //    HttpConfiguration _config;
    //    public ProxyHandler(HttpConfiguration config)
    //    {
    //        _metadataProvider = new MetadataProvider(config);
    //        _config = config;
    //    }

    //    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    //    {
    //        return await Task.Run(() =>
    //        {
    //            var metadata = _metadataProvider.GetMetadata(request);

    //            if (request.Headers.Any(h => h.Key == "X-Proxy-Type" && h.Value.Contains("metadata")))
    //            {
    //                return request.CreateResponse(System.Net.HttpStatusCode.OK, metadata);
    //            }

    //            var template = new JsProxyTemplate(metadata);
    //            var js = new StringContent(template.TransformText());

    //            js.Headers.ContentType = new MediaTypeHeaderValue("application/javascript");
    //            js.Headers.Expires = DateTime.Now.AddDays(30).ToUniversalTime();

    //            var result = new HttpResponseMessage { Content = js }; ;
    //            result.Headers.CacheControl = CacheControlHeaderValue.Parse("public");

    //            return result;
    //        });
    //    }
    //}
}
