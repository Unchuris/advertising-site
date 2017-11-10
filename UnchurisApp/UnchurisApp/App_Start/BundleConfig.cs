using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Optimization.React;

namespace UnchurisApp.App_Start {
  public static class BundleConfig {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles) {
      bundles.Add(new Bundle("~/bundles/main").Include(
          "~/Scripts/dist/clientBundle.js"
      ));

      // Force minification/combination even in debug mode
      BundleTable.EnableOptimizations = false;
    }
  }
}