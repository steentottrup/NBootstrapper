# NBootstrapper

NBootstrapper is a set of tools useful when working with ASP.Net MVC3 and Twitter Bootstrap.

## Features

Currently NBootstrapper only provides a partial set of HtmlHelper extension methods useful when working with Twitter Bootstrap.

NBootstrapper also allows you to completely override the rendering logic of all the extension methods. Simply implement `IHtmlRenderer` (or `DefaultHtmlRenderer`) and set `BootstrapHelperConfiguration.HtmlRenderer` to your implementation.
 
## Installation

Until the project matures you will need to download the code and install it yourself (there isn't much point to making a NuGet package at the moment). 
 
 1. Add a reference to the compiled assembly or add the files directly into your project.
 2. Open the 'web.config' under the 'Views' folder.
 3. Locate `<add namespace="System.Web.Mvc.Html" />` and replace it with `<add namespace="NBootstrapper.Mvc.Html" />`.

Keep in mind that because the extension methods are incomplete you will lose some functionality.

## Contributing

I would be happy for people to contribute. I am also quite a busy person so I open to someone stepping up and taking over the project.