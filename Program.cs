﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Statiq.Common;
using Statiq.App;
using Statiq.Web;

namespace MySite
{
  public class Program
  {
    public static async Task<int> Main(string[] args) =>
      await Bootstrapper
        .Factory
        .CreateWeb(args)
        .AddSetting(Keys.Host, new Uri(Constants.SiteUri).Host)
        .AddSetting(Keys.LinksUseHttps, true)
        .AddSetting(WebKeys.ExcludedPaths,
            	new List<NormalizedPath>
            	{
                	new NormalizedPath("input/admin"),
            	})
        // .AddSetting(
        //   Keys.DestinationPath,
        //   Config.FromDocument((doc, ctx) =>
        //   {
        //       // Only applies to the content pipeline
        //       if (ctx.PipelineName == nameof(Statiq.Web.Pipelines.Content))
        //       {
        //           return doc.Source.Parent.Segments.Last().SequenceEqual("posts".AsMemory())
        //               ? new NormalizedPath(Constants.BlogPath).Combine(doc.GetDateTime(WebKeys.Published).ToString("yyyy")).Combine(doc.GetString("Category")).Combine(doc.Destination.FileName.ChangeExtension(".html"))
        //               : doc.Destination.ChangeExtension(".html");
        //       }
        //       return doc.Destination;
        //   }))        
      .RunAsync();
}
}

