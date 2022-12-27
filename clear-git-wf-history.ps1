$repo = "azure.isnowhere.org"
$org = "svenbot42"

$workflow_ids=($(gh api repos/$org/$repo/actions/workflows | jq '.workflows[] | select(.["state"] | contains("disabled_manually")) | .id'))

foreach ($wf in $workflow_ids) {
    write-host "Listing runs for the workflow ID $wf"
    $run_ids = ( $(gh api repos/$org/$repo/actions/workflows/$wf/runs --paginate | jq '.workflow_runs[].id') )
    foreach ($r in $run_ids) {
        write-host "Deleting Run ID $r"
        gh api repos/$org/$repo/actions/runs/$r -X DELETE >/dev/null
    }
}

