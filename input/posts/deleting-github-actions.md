---
Title: Deleting old CI/CD Actions for Azure in GitHub
Lead: There is no delete in team
Published: 12/27/2022
Tags: [powershell, git]
---

I had to redo my azure CI/CD action because M$ removed my old static web site (cleaning out old MSDN licensed resources).   I now had two actions in my repository and only one worked.   There does not seem to be any delete function in the github interface for actions.

This is how I deleted the old action.

1. disable the bad actor
1. install snap gh on linux
1. gh auth
1. run the following powershell script
1. delete the yml file for the old action in .github/workflows

```powershell
    $repo = "myrepo.isnowhere.org"
    $org = "myGitUser"

    $workflow_ids=($(gh api repos/$org/$repo/actions/workflows | jq '.workflows[] | select(.["state"] | contains("disabled_manually")) | .id'))

    foreach ($wf in $workflow_ids) {
        write-host "Listing runs for the workflow ID $wf"
        $run_ids = ( $(gh api repos/$org/$repo/actions/workflows/$wf/runs --paginate | jq '.workflow_runs[].id') )
        foreach ($r in $run_ids) {
            write-host "Deleting Run ID $r"
            gh api repos/$org/$repo/actions/runs/$r -X DELETE >/dev/null
        }
    }
```

## time for tika
