require 'albacore' # >= 0.2.7
require 'fileutils'

pagedlist_version = "1.5.0.0"
pagedlist_mvc_version = "3.3.0.0"

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

assemblyinfo :generate_pagedlist_assemblyinfo do |asm|
  asm.version = pagedlist_version
  asm.company_name = "Troy Goode"
  asm.product_name = "PagedList"
  asm.title = "PagedList"
  asm.description = "PagedList makes it easier for .Net developers to write paging code. It allows you to take any IEnumerable(T) and by specifying the page size and desired page index, select only a subset of that list. PagedList also provides properties that are useful when building UI paging controls."
  asm.copyright = "MIT License"
  asm.guid = "1d709432-45fa-4475-a403-b2310a47d0a6"
  asm.custom_attributes :CLSCompliant => true, :ComVisible => false, :AllowPartiallyTrustedCallers
  asm.output_file = "src/PagedList/Properties/AssemblyInfo.cs"
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