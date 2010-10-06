require 'albacore'

task :default => [:build]

msbuild :build do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

xunit :test do |xunit|
  xunit.path_to_command = "src/PagedList.Tests/bin/debug/xunit.console.exe"
  xunit.assembly = "src/PagedList.Tests/bin/debug/PagedList.Tests.dll"
end

msbuild :release do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end