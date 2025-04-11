# Script to kill any running BiddingManagementSystem processes

Write-Host "Stopping any running BiddingManagementSystem processes..."

$processes = Get-Process | Where-Object { $_.ProcessName -like "*BiddingManagement*" }

if ($processes.Count -eq 0) {
    Write-Host "No BiddingManagementSystem processes found."
} else {
    foreach ($process in $processes) {
        Write-Host "Stopping process: $($process.ProcessName) (PID: $($process.Id))"
        Stop-Process -Id $process.Id -Force
    }
    Write-Host "All BiddingManagementSystem processes have been stopped."
}

Write-Host "Done!" 