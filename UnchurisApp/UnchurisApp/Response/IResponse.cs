using System;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnchurisApp.Response {
  public interface IResponse {
    void WriteTrue(HttpResponseBase response);
    void WriteFalse(HttpResponseBase response);
    void WriteList<T>(HttpResponseBase response, String Value, IList<T> list);
    void WriteObject<T>(HttpResponseBase response, String Value, T obj);
    void WriteList<T>(HttpResponseBase response, IList<T> Value);
    void WriteObject<T>(HttpResponseBase response, T Value);
  }
}
