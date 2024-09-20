---
Title: Fix changes for statiq web sites
Lead: .NET version changes and markdown frontmatter
Published: 12/27/2022
Tags: [statiq, git, ci/cd]
---

I had probelms with my azure CI/CD action because M$ allowed their certificates for the code signatures on .NET version 5.0 lapse.   Any .NET 5.0 dependencies would fail with untrusted signer errors.   I had to fiure out how to change the version of .NET I was using with statiq.

This is what I did.

1. change the version of .net installed on my machine (.NET 7.0)
1. changed the version of .net required for my project output

```xml
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp7.0</TargetFramework>
  </PropertyGroup>
```

1. change the .net version in the action yml in github

```yml
    steps:
      - uses: actions/checkout@v2
        with:
          submodules: true
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: 7.0.100
      - run: dotnet restore
      - run: dotnet build --configuration Release --no-restore
      - run: dotnet run --output output
```

1. rebuild the site

---

Also I realized that my site did not build properly.   Frontmatter data from the the markdown documents were being rendered in the document and the frontmatter data was not being read by the system.

It turns out the latest version of Statiq does not repect the "---" single bracket model for frontmatter.   You need to use bracketed bracketed syntax for XML, C# or Blazor comments.   If you want to use "---" you need to include that markup at the top **AND** bottom of your frontmatter block.

---

1. then i ran like heck !!
