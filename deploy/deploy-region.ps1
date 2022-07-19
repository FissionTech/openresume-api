param(
    [Parameter(Mandatory=$true)]
    $Environment,
    [Parameter(Mandatory=$false)]
    [switch] $Local,
    [Parameter(Mandatory=$false)]
    $LogFile = "",
    [Parameter(Mandatory=$false)]
    $RGPrefix = "mjl",
    [Parameter(Mandatory=$true)]
    [string[]] $RGLocations
)

. .\common.ps1

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