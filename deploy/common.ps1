$EnvironmentInformation = @{
    "dev" = @{
        EnvironmentIndicator = "d"
    };
    "qa" = @{
        EnvironmentIndicator = "q"
    };
    "prod" = @{
        EnvironmentIndicator = "p"
    }
}

$LocationInformation = @{
    "centralus" = @{
        LocationIndicator = "cus"
    };
    "eastus" = @{
        LocationIndicator = "eus"
    };
    "westus" = @{
        LocationIndicator = "wus"
    };
    "centralus2" = @{
        LocationIndicator = "cus2"
    };
    "eastus2" = @{
        LocationIndicator = "eus2"
    };
    "westus2" = @{
        LocationIndicator = "wus2"
    }
}

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
    
    if(-not ([string]::IsNullOrEmpty($LogFile))) {
        $logLine | Out-File -Append $LogFile -ErrorAction SilentlyContinue
    }
}