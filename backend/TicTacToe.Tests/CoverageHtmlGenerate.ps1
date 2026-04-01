# Limpa TestResults e CoverageReports antes de gerar novos arquivos
Write-Host "Limpando resultados anteriores...`n"
Remove-Item -Path "$PSScriptRoot\TestResults" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "$PSScriptRoot\CoverageReports" -Recurse -Force -ErrorAction SilentlyContinue

# Verifica se o ReportGenerator está instalado
$reportGeneratorInstalled = dotnet tool list -g | Select-String "dotnet-reportgenerator-globaltool"

if (-not $reportGeneratorInstalled) {
    Write-Host "ReportGenerator não encontrado. Instalando...`n"
    dotnet tool install -g dotnet-reportgenerator-globaltool
} else {
    Write-Host "ReportGenerator já está instalado.`n"
}

# Roda os testes com cobertura a partir da raiz da solução para garantir
# a resolução correta das dependências pelo coverlet
Write-Host "Executando testes e coletando cobertura...`n"
$solutionRoot = Split-Path $PSScriptRoot -Parent
Push-Location $solutionRoot
dotnet test "$PSScriptRoot" --collect:"XPlat Code Coverage;Format=cobertura" --settings "$PSScriptRoot\coverlet.runsettings" --results-directory "$PSScriptRoot\TestResults"
Pop-Location

# Gera o relatório de cobertura
Write-Host "Gerando relatório HTML...`n"
reportgenerator -reports:"$PSScriptRoot\TestResults\*\coverage.cobertura.xml" -targetdir:"$PSScriptRoot\CoverageReports" -reporttypes:"Html"

# Cria um atalho para CoverageReports/index.html
Write-Host "Criando atalho para o relatório...`n"

# Caminho do atalho e do relatório
$shortcutPath = Join-Path -Path $PSScriptRoot -ChildPath "CoverageReport.lnk"
$targetPath = Join-Path -Path $PSScriptRoot -ChildPath "CoverageReports\index.html"

# Criando o atalho corretamente
$wScriptShell = New-Object -ComObject WScript.Shell
$shortcut = $wScriptShell.CreateShortcut($shortcutPath)
$shortcut.TargetPath = $targetPath
$shortcut.Save()

Write-Host "Atalho criado com sucesso: $shortcutPath`n"

# Remove a pasta TestResults
Write-Host "Removendo pasta TestResults...`n"
Remove-Item -Path "$PSScriptRoot\TestResults" -Recurse -Force
Write-Host "Pasta TestResults removida.`n"

Write-Host "✅ Processo concluído! Abra 'CoverageReport.lnk' para ver o relatório.`n"

