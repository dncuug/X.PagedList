require 'albacore'

task :default => [:build]

msbuild :build do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

xunit :test => :build do |xunit|
  xunit.path_to_command = "src/PagedList.Tests/bin/debug/xunit.console.exe"
  xunit.assembly = "src/PagedList.Tests/bin/debug/PagedList.Tests.dll"
end

msbuild :release => :test do |msb|
  msb.path_to_command =  File.join(ENV['windir'], 'Microsoft.NET', 'Framework',  'v4.0.30319', 'MSBuild.exe')
  msb.properties :configuration => :Release
  msb.targets :Clean, :Rebuild
  msb.solution = "src/PagedList.sln"
end

task :package_pagedlist => :release do
  require 'fileutils'
  build_directory = './src/PagedList/bin/Release/'
  output_directory = './packages/PagedList/lib/40/'
  FileUtils.mkdir_p output_directory
  FileUtils.cp build_directory + 'PagedList.dll', output_directory + 'PagedList.dll'
  FileUtils.cp build_directory + 'PagedList.pdb', output_directory + 'PagedList.pdb'
  FileUtils.cp build_directory + 'PagedList.xml', output_directory + 'PagedList.xml'
end

task :package_pagedlistmvc => :release do
  require 'fileutils'

  build_directory = './src/PagedList.Mvc/bin/Release/'
  content_directory = './src/PagedList.Mvc.Example/Content/'
  lib_output_directory = './packages/PagedList.Mvc/lib/40/'
  content_output_directory = './packages/PagedList.Mvc/content/Content/'

  FileUtils.mkdir_p lib_output_directory
  FileUtils.mkdir_p content_output_directory

  FileUtils.cp build_directory + 'PagedList.Mvc.dll', lib_output_directory + 'PagedList.Mvc.dll'
  FileUtils.cp build_directory + 'PagedList.Mvc.pdb', lib_output_directory + 'PagedList.Mvc.pdb'
  FileUtils.cp build_directory + 'PagedList.Mvc.xml', lib_output_directory + 'PagedList.Mvc.xml'
  FileUtils.cp content_directory + 'PagedList.css', content_output_directory + 'PagedList.css'
end

task :package => [:package_pagedlist, :package_pagedlistmvc] do
end