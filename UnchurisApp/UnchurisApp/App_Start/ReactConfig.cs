using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnchurisApp.ReactConfig), "Configure")]

namespace UnchurisApp {
	public static class ReactConfig {
		public static void Configure() {
      ReactSiteConfiguration.Configuration = new ReactSiteConfiguration();
      ReactSiteConfiguration.Configuration
               .AddScript("~/Scripts/dist/serverBundle.js");
    }
  }
}