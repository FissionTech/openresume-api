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