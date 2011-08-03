require 'albacore' # >= 0.2.7
require 'fileutils'

task :default => [:build]

msbuild :build do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

xunit :test => :build do |xunit|
  xunit.command = "src/PagedList.Tests/Dependencies/xunit-1.8/xunit.console.clr4.exe"
  xunit.assembly = "src/PagedList.Tests/bin/debug/PagedList.Tests.dll"
end

msbuild :release => :test do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

nugetpack :package_pagedlist => :test do |nuget|
	nuget.nuspec = './src/PagedList/PagedList.csproj -Prop Configuration=Release'
	nuget.output = './packages/'
end

#HACK: remove once http://nuget.codeplex.com/workitem/1349 is fixed
task :prepare_package_pagedlistmvc do
  content_directory = './src/PagedList.Mvc.Example/Content/'
  script_directory = './src/PagedList.Mvc.Example/Scripts/PagedList/'

  content_directory_out = './src/PagedList.Mvc/Content/Content/'
  script_directory_out = './src/PagedList.Mvc/Content/Scripts/PagedList/'

  FileUtils.mkdir_p content_directory_out
  FileUtils.mkdir_p script_directory_out

  FileUtils.cp content_directory + 'PagedList.css', content_directory_out + 'PagedList.css'
  FileUtils.cp script_directory + 'PagedList.Mvc.js', script_directory_out + 'PagedList.Mvc.js'
  FileUtils.cp script_directory + 'PagedList.Mvc.Template.html', script_directory_out + 'PagedList.Mvc.Template.html'
end

nugetpack :package_pagedlistmvc => :prepare_package_pagedlistmvc do |nuget|
	nuget.nuspec = './src/PagedList.Mvc/PagedList.Mvc.csproj -Prop Configuration=Release'
	nuget.output = './packages/'
end

task :package => [:package_pagedlist, :package_pagedlistmvc] do
end