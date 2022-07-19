[CmdletBinding()]
param (
    [Parameter(Mandatory = $false)]
    [string] 
    $SubscriptionId,
    [Parameter(Mandatory = $false)]
    [switch]
    $SkipRegions
)

# Include common.ps1
. .\common.ps1

# ---------------------------------------------------------------------------
# Azure Context Switch
# ---------------------------------------------------------------------------
if(![string]::IsNullOrEmpty($SubscriptionId)) {
    try { 
        Set-AzContext -Subscription $SubscriptionId -ErrorAction Stop
    } catch {
        Log -Severity "ERR" "Unable to set Azure Context to Subscription with ID $SubscriptionId."
        $defContext = Get-AzContext
        $prompt = Read-Host -Prompt "Continue with default subscription $($defContext.Subscription.Id)? (y/n): "

        if($prompt.ToLower() -ne "y" -or $prompt.ToLower() -ne "yes") {
            Log -Severity "INFO" "User has chosen to continue with default subscription."
        } else {
            Log -Severity "INFO" "User has chosen not to continue with default subscription, or input not recognized."
            Log -Severity "INFO" "Please run Get-AzContext -ListAvailable to see all available contexts."
            exit 1
        }
    }
}

# ---------------------------------------------------------------------------
# Global Deployment to CentralUS
# ---------------------------------------------------------------------------
try {
    $loc = "centralus"
    New-AzSubscriptionDeployment -Name "GlobDeploy" -TemplateFile .\deploy-global.bicep -Location $loc -environmentName "dev" -globalLocation $loc -ErrorAction Stop
    Log -Severity "INFO" "Created global deployment successfully."
} catch {
    Log -Severity "ERR" "Unable to create global deployment."
    Log -Severity "ERR" $_
    exit 1
}

# ---------------------------------------------------------------------------
# Regional Deployments
# ---------------------------------------------------------------------------
# Guard for -SkipRegions switch
if($SkipRegions) {
    Log -Severity "INFO" "Deployment run with '-SkipRegions' switch. No further actions necessary, terminating script."
    Log -Severity "INFO" "Successfully deployed."
    exit 1
}

# Code Below only run if -SkipRegions is NOT present

try {
    .\deploy-region.ps1 -Environment "dev" -RegionResourceGroups "centralus","eastus2","westus2" -ErrorAction Stop
} catch {
    Log -Severity "ERR" "Failed to create regional deployment."
    Log -Severity "ERR" $_
    exit 1
}

Log -Severity "INFO" "Successfully deployed."
