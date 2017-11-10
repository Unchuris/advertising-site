using System;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnchurisApp.Response {
  public class ResponseHelperJSON : IResponse {
    public void WriteTrue(HttpResponseBase response) {
      response.ContentType = "application/json";
      response.Write(JsonConvert.SerializeObject(new { successful = true }));
      response.End();
    }

    public void WriteFalse(HttpResponseBase response) {
      response.ContentType = "application/json";
      response.Write(JsonConvert.SerializeObject(new { successful = false }));
      response.End();
    }

    public void WriteNull(HttpResponseBase response) {
      response.ContentType = "application/json";
      response.Write(JsonConvert.SerializeObject(new { result = "No data" }));
      response.End();
    }

    public void WriteList<T>(HttpResponseBase response, String Value, IList<T> list) {
      response.ContentType = "application/json";
      response.Write("{ \"" + Value + "\":" + JsonConvert.SerializeObject(list) + " }");
      response.End();
    }

    public void WriteObject<T>(HttpResponseBase response, String Value, T obj) {
      response.ContentType = "application/json";
      response.Write("{ \"" + Value + "\":" + JsonConvert.SerializeObject(obj) + " }");
      response.End();
    }

    public void WriteList<T>(HttpResponseBase response, IList<T> Value) {
      response.ContentType = "application/json";
      response.Write(JsonConvert.SerializeObject(Value));
      response.End();
    }

    public void WriteObject<T>(HttpResponseBase response, T Value) {
      response.ContentType = "application/json";
      response.Write(JsonConvert.SerializeObject(Value));
      response.End();
    }
  }
}