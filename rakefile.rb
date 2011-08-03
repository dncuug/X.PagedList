require 'albacore' # >= 0.2.7

task :default => [:build]

msbuild :build do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

xunit :test => :build do |xunit|
  xunit.command = "src/PagedList.Tests/bin/debug/xunit.console.exe"
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

task :prepare_package_pagedlistmvc => :release do
  require 'fileutils'

  build_directory = './src/PagedList.Mvc/bin/Release/'
  content_directory = './src/PagedList.Mvc.Example/Content/'
  script_directory = './src/PagedList.Mvc.Example/Scripts/PagedList/'

  lib_output_directory = './packages/PagedList.Mvc/lib/40/'
  content_output_directory = './packages/PagedList.Mvc/content/Content/'
  script_output_directory = './packages/PagedList.Mvc/content/Scripts/PagedList/'

  FileUtils.mkdir_p lib_output_directory
  FileUtils.mkdir_p content_output_directory
  FileUtils.mkdir_p script_output_directory

  FileUtils.cp build_directory + 'PagedList.Mvc.dll', lib_output_directory + 'PagedList.Mvc.dll'
  FileUtils.cp build_directory + 'PagedList.Mvc.pdb', lib_output_directory + 'PagedList.Mvc.pdb'
  FileUtils.cp build_directory + 'PagedList.Mvc.xml', lib_output_directory + 'PagedList.Mvc.xml'
  FileUtils.cp content_directory + 'PagedList.css', content_output_directory + 'PagedList.css'
  FileUtils.cp script_directory + 'PagedList.Mvc.js', script_output_directory + 'PagedList.Mvc.js'
  FileUtils.cp script_directory + 'PagedList.Mvc.Template.html', script_output_directory + 'PagedList.Mvc.Template.html'
end

nugetpack :package_pagedlistmvc => :prepare_package_pagedlistmvc do |nuget|
	nuget.nuspec = './packages/PagedList.Mvc/PagedList.Mvc.nuspec'
	nuget.output = './packages/'
end

task :package => [:package_pagedlist, :package_pagedlistmvc] do
end