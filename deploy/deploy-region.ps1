param(
    [Parameter(Mandatory=$true)]
    [string]
    $Environment,

    [Parameter(Mandatory=$false)]
    [switch]
    $Local,

    [Parameter(Mandatory=$false)]
    [string]
    $LogFile = "",

    [Parameter(Mandatory=$false)]
    [string]
    $RGPrefix = "mjl",

    [Parameter(Mandatory=$true)]
    [string[]] $RGLocations
    
)

# Source Common
. .\common.ps1

# Deploy Resources via Bicep
foreach ($location in $RGLocations) {
    try {
        $envIndicator = $EnvironmentInformation[$Environment].EnvironmentIndicator
        $locIndicator = $LocationInformation[$location].LocationIndicator
        $rgName = "$rgPrefix$envIndicator$locIndicator"

        New-AzResourceGroupDeployment -Name "RegDep" -ResourceGroupName $rgName -TemplateFile "deploy-region.bicep" -environmentName $Environment -ErrorAction Stop
    } catch {
        Log -Severity ERR "Failed to deploy to region $region."
        Log -Severity ERR "Exception: $_"
    }
}

Log -Severity INFO "END - deploy-region.ps1"