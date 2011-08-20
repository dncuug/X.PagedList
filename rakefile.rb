require 'albacore' # >= 0.2.7
require 'fileutils'
load './version.rb'

task :default => [:build]

assemblyinfo :generate_pagedlist_assemblyinfo do |asm|
  asm.version = PAGEDLIST_VERSION
  asm.company_name = "Troy Goode"
  asm.product_name = "PagedList"
  asm.title = "PagedList"
  asm.description = "PagedList makes it easier for .Net developers to write paging code. It allows you to take any IEnumerable(T) and by specifying the page size and desired page index, select only a subset of that list. PagedList also provides properties that are useful when building UI paging controls."
  asm.copyright = "MIT License"
  asm.custom_attributes \
	:CLSCompliant => true,
	:ComVisible => false,
	:Guid => "1d709432-45fa-4475-a403-b2310a47d0a6",
	:AllowPartiallyTrustedCallers => nil,
	:AssemblyFileVersion => PAGEDLIST_VERSION,
	:AssemblyConfiguration => '',
	:AssemblyTrademark => '',
	:AssemblyCulture => ''
  asm.namespaces "System", "System.Security"
  asm.output_file = "src/PagedList/Properties/AssemblyInfo.cs"
end

assemblyinfo :generate_pagedlistmvc_assemblyinfo do |asm|
  asm.version = PAGEDLIST_MVC_VERSION
  asm.company_name = "Troy Goode"
  asm.product_name = "PagedList.Mvc"
  asm.title = "PagedList.Mvc"
  asm.description = "Asp.Net MVC HtmlHelper method for generating paging control for use with PagedList library."
  asm.copyright = "MIT License"
  asm.custom_attributes \
	:CLSCompliant => true,
	:ComVisible => false,
	:Guid => "eb684fee-2094-4833-ae61-f9bfcab34abd",
	:AllowPartiallyTrustedCallers => nil,
	:AssemblyFileVersion => PAGEDLIST_MVC_VERSION,
	:AssemblyConfiguration => '',
	:AssemblyTrademark => '',
	:AssemblyCulture => ''
  asm.namespaces "System", "System.Security"
  asm.output_file = "src/PagedList.Mvc/Properties/AssemblyInfo.cs"
end

nuspec :generate_pagedlist_nuspec do |nuspec|
  nuspec.title = "$id$"
  nuspec.id = "$id$"
  nuspec.version = "$version$"
  nuspec.authors = "$author$"
  nuspec.owners = "TroyGoode"
  nuspec.description = "$description$"
  nuspec.language = "en-US"
  nuspec.licenseUrl = "http://www.opensource.org/licenses/mit-license.php"
  nuspec.projectUrl = "http://github.com/TroyGoode/PagedList"
  nuspec.tags = "paging pager page infinitescroll ajax mvc"
  nuspec.output_file = "src/PagedList/PagedList.nuspec"
end

nuspec :generate_pagedlistmvc_nuspec do |nuspec|
  nuspec.title = "$id$"
  nuspec.id = "$id$"
  nuspec.version = "$version$"
  nuspec.authors = "$author$"
  nuspec.owners = "TroyGoode"
  nuspec.description = "$description$"
  nuspec.language = "en-US"
  nuspec.licenseUrl = "http://www.opensource.org/licenses/mit-license.php"
  nuspec.projectUrl = "http://github.com/TroyGoode/PagedList"
  nuspec.tags = "paging pager page infinitescroll ajax mvc"
  nuspec.dependency "PagedList", PAGEDLIST_VERSION
  nuspec.file "Content\\**\\*.*", "Content"
  nuspec.output_file = "src/PagedList.Mvc/PagedList.Mvc.nuspec"
end

msbuild :build => [:generate_pagedlist_assemblyinfo, :generate_pagedlistmvc_assemblyinfo, :generate_pagedlist_nuspec, :generate_pagedlistmvc_nuspec] do |msb|
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

nugetpack :package_pagedlist => :release do |nuget|
	nuget.nuspec = './src/PagedList/PagedList.csproj -Prop Configuration=Release'
	nuget.output = './packages/'
end

#HACK: remove once http://nuget.codeplex.com/workitem/1349 is fixed
task :prepare_package_pagedlistmvc => :release do
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

nugetpush :push_pagedlist => :package_pagedlist do |nuget|
	ver = String.new(PAGEDLIST_VERSION)
	ver.slice!(/(\.0)*$/)
	nuget.package = "./packages/PagedList.#{ver}.nupkg"
end

nugetpush :push_pagedlistmvc => :package_pagedlistmvc do |nuget|
	ver = String.new(PAGEDLIST_MVC_VERSION)
	ver.slice!(/(\.0)*$/)
	nuget.package = "./packages/PagedList.Mvc.#{ver}.nupkg"
end

task :push => [:push_pagedlist, :push_pagedlistmvc] do
end