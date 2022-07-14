param(
    [Parameter(Mandatory=$true)]
    $Environment,
    [Parameter(Mandatory=$false)]
    [switch] $Local,
    [Parameter(Mandatory=$false)]
    $LogFile = ""
)

function Log {
    param(
        [Parameter(Mandatory = $false, ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $false, ValueFromRemainingArguments = $false)]
        [ValidateSet("ERR", "WARN", "DBG", "INFO")]
        $Severity = "INFO",
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromRemainingArguments = $true)]
        $Message
    )

    $date = Get-Date -Format "yyyy/MM/dd hh:mm:ss"
    $logLine = "[$date] [PID:${$PID}] [$Severity] :: $Message"
    
    if($Severity -eq "ERR") {
        Write-Host $logLine -BackgroundColor Red
    } else {
        Write-Host $logLine
    }
    
    if($LogFile -ne "") {
        $logLine | Out-File -Append $LogFile -ErrorAction SilentlyContinue
    }
}

$regions = @("mjldcus")

foreach ($region in $regions) {
    try {
        New-AzResourceGroupDeployment -Name "RegDCUS-Dep" -ResourceGroupName $region -TemplateFile "deploy-region.bicep" -environmentName $Environment -ErrorAction Stop
    } catch {
        Log -Severity ERR "Failed to deploy to region $region."
        Log -Severity ERR "Exception: $_"
    }
}

Log -Severity INFO "END - deploy-region.ps1"
