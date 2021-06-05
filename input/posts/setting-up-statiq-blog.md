Title: Static Web Sites on Azure
Lead: Blasting a new static website on azure with CI/CD is easier than i thought it would be
Published: 5/31/2021
Tags: [git, statiq, ci/cd]
---

#### Create a Statiq.Web project {.bg-dark .text-light .p-2}

1. Create a statiq project based on Statiq.Wewb ([*here*](https://github.com/statiqdev/Statiq.Web#readme))
2. Add theme like CleanBlog ([*here*](https://github.com/statiqdev/CleanBlog)) (optional)
3. Check into github (I know there are other git repositories, but this is microsoft ... things work better when you swim in the same pond) ([*tutorial*](https://guides.github.com/activities/hello-world))

#### Create a Static Web resource in Azure {.bg-dark .text-light .p-2}

1. Create a static website (template is in the Web Category)
5. Select resource group (create one if necessary)
6. Pick a unique name
7. Pick Hosting plan (free is good)
8. Pick a region for functions api (close to home)
9. Log into GitHub and authorize Azure Static Web Sites to access repositories
10. Choose Organization, Repository, Branch
11. Build Presets: Custom
12. App Location: /output (this is where statiq puts static content)
13. Ignore API and Output locations
14. Review + Create

#### Finish the build  {.bg-dark .text-light .p-2}

1. Back to your repository in github
2. Click Actions -> Azure Static Web Apps CI/CD (added by M$ in previous step)
2. Click the yml link right below the workflow name looks like "azure-static-web-apps-gentle-glacier-0a429ea0f.yml" 
2. Edit the jobs definition to add build instructions


```
jobs:
  build_and_deploy_job:
    if: github.event_name == 'push' || 
        (github.event_name == 'pull_request' && 
         github.event.action != 'closed')
    runs-on: ubuntu-latest
    name: Build and Deploy Job
    steps:
      - uses: actions/checkout@v2
        with:
          submodules: true
```

##### add:

```
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: 5.0.100
      - run: dotnet restore
      - run: dotnet build --configuration Release --no-restore
      - run: dotnet run --output output
```

##### before:

```
      - name: Build And Deploy
        id: builddeploy
        uses: Azure/static-web-apps-deploy@v1
```

#### Save and enjoy!!  {.bg-dark .text-light .p-2}
