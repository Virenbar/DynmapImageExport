dotnet tool install -g minver-cli --version 2.5.0
$version = minver -t v -d preview
# $number = $version.Split('-')[0]
Write-Output "VERSION=$version"
"VERSION=$version" >> $env:GITHUB_ENV
Write-Output "{name}={$version}" >> $GITHUB_OUTPUT