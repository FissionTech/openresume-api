[CmdletBinding()]
param (
    [Parameter(Mandatory = $false)]
    [string] 
    $SubscriptionId
)

# Include common.ps1
. .\common.ps1

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

try {
    New-AzSubscriptionDeployment -Name "GlobDeploy" -TemplateFile .\deploy-global.bicep -environmentName "dev" -globalLocation "centralus" -ErrorAction Stop
    Log -Severity "INFO" "Created global deployment successfully."
} catch {
    Log -Severity "ERR" "Unable to create global deployment."
    Log -Severity "ERR" $_
    exit 1
}

try {
    .\deploy-region.ps1 -Environment "dev" -RegionResourceGroups "centralus","eastus2","westus2" -ErrorAction Stop
} catch {
    Log -Severity "ERR" "Failed to create regional deployment."
    Log -Severity "ERR" $_
    exit 1
}

Log -Severity "INFO" "Successfully deployed."
